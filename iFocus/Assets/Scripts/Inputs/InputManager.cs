using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// This class is used for managing all the inputs coming from the user and distributing them for their desired implementation.
/// </summary>
[RequireComponent(typeof(CameraController))]
[RequireComponent(typeof(RaycastSelectionHandler))]
public class InputManager : MonoBehaviour
{
    #region Dependencies

    /// <summary>
    /// The camera controller reference.
    /// </summary>
    CameraController cameraController;

    /// <summary>
    /// The RaycastSelectionHandler controller reference.
    /// </summary>
    RaycastSelectionHandler raySelector;

    #endregion

    #region Global Fields

    /// <summary>
    /// Input touch used for storing all the usefull first input values needed.
    /// </summary>
    Touch firstTouch;

    /// <summary>
    /// Input touch used for storing all the usefull second input values needed.
    /// </summary>
    Touch secondTouch;

    /// <summary>
    /// Is the UI selected. Its true if the input position is above a UI element.
    /// </summary>
    [SerializeField] private bool isUISelected;

    [SerializeField] private GraphicRaycaster _raycaster;

    #endregion

    #region Input Fields

    // ---------------------TAP---------------------------|

    [Header("Tap Settings")]
    [SerializeField] private float tapTimeThreshold = 0.5f;
    private Coroutine tapVerificationCoroutine;
    private float elapsedTime;
    private int tapFlag;
    private bool isTapping;

    // -------------------Panning-------------------------|

    [Header("Panning Settings")]
    private Vector3 initialPanningPosition;
    private Vector3 deltaPanningPosition;

    // ---------------------Zoom--------------------------|

    [Header("Zoom Settings")]
    [SerializeField] private float minScale = 2.0F;
    [SerializeField] private float maxScale = 5.0F;
    [SerializeField] private float minimumPinchSpeed = 5.0F;
    [SerializeField] private float varianceInDistances = 5.0F;
    [SerializeField] private float deadZone = 5f;
    private bool isZooming;

    private float touchDelta = 0.0F;
    private Vector2 previousDistance = new Vector2(0, 0);
    private Vector2 currentDistance = new Vector2(0, 0);
    private float speedTouch0 = 0.0F;
    private float speedTouch1 = 0.0F;

    #endregion

    #region Unity CallBacks

    private void Awake()
    {
        cameraController = GetComponent<CameraController>();
        raySelector = GetComponent<RaycastSelectionHandler>();

        //this.raycaster = GetComponent<GraphicRaycaster>();
    }

    void Update()
    {
        if (Input.touchCount == 1)
        {
            firstTouch = Input.GetTouch(0);

            //Set up the new Pointer Event
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            List<RaycastResult> results = new List<RaycastResult>();

            //Raycast using the Graphics Raycaster and mouse click position
            pointerData.position = firstTouch.position;
            this._raycaster.Raycast(pointerData, results);

            SetUISelected(results.Count > 0);

            if (firstTouch.phase == TouchPhase.Began)
            {
                StartTapVerification();
                tapFlag = 1;
            }
            if (firstTouch.phase == TouchPhase.Moved)
            {
                Swipe();
            }
            if (firstTouch.phase == TouchPhase.Ended)
            {
                tapFlag++;
            }
        }

        if (Input.touchCount == 2)
        {
            firstTouch = Input.GetTouch(0);
            secondTouch = Input.GetTouch(1);

            /*
            if (secondTouch.phase == TouchPhase.Began)
            {
                StartPanning();
            }
            if (secondTouch.phase == TouchPhase.Moved)
            {
                Pan();
            }
            */

            if (secondTouch.phase == TouchPhase.Moved && firstTouch.phase == TouchPhase.Moved)
            {
                ZoomUpdateInputs();
                ZoomVerification();

                
                if (isZooming)
                    Zoom();
                      
            }
            if (secondTouch.phase == TouchPhase.Ended)
            {
                isZooming = false;
            }
        }

    }

    #endregion

    #region Custom Methods

    /// <summary>
    /// Saves the initiali position of the inputs.
    /// </summary>
    private void StartPanning()
    {
        initialPanningPosition = GetWorldPosition();
    }

    /// <summary>
    /// Handles the panning functionality.
    /// </summary>
    private void Pan()
    {
        deltaPanningPosition = initialPanningPosition - GetWorldPosition();
        cameraController.Pan(deltaPanningPosition);
    }

    /// <summary>
    /// Validates if the Zooming cand be done. Depending on the DeadZone.
    /// </summary>
    private void ZoomVerification()
    {
        if (Mathf.Abs(touchDelta) >= deadZone)
            isZooming = true;
    }

    /// <summary>
    /// Handles the input updates for the zooming funciotnality. Saves the distances between the two touches.
    /// </summary>
    private void ZoomUpdateInputs()
    {
        currentDistance = firstTouch.position - secondTouch.position; //current distance between finger touches
        previousDistance = ((firstTouch.position - firstTouch.deltaPosition) - (secondTouch.position - secondTouch.deltaPosition));

        touchDelta = currentDistance.magnitude - previousDistance.magnitude;
        speedTouch0 = firstTouch.deltaPosition.magnitude / firstTouch.deltaTime;
        speedTouch1 = secondTouch.deltaPosition.magnitude / secondTouch.deltaTime;
    }

    /// <summary>
    /// Call the camera to perform a zoom.
    /// </summary>
    private void Zoom()
    {
        if ((touchDelta + varianceInDistances <= 1) && (speedTouch0 > minimumPinchSpeed) && (speedTouch1 > minimumPinchSpeed))
        {
            cameraController.StartZoom(1 * -touchDelta);
            //cameraController.Zoom(1);
        }

        if ((touchDelta + varianceInDistances > 1) && (speedTouch0 > minimumPinchSpeed) && (speedTouch1 > minimumPinchSpeed))
        {
            cameraController.StartZoom(-1 * touchDelta);
            //cameraController.Zoom(-1);
        }
    }

    /// <summary>
    /// Gets the world position of the input relative to the camera's front angle.
    /// </summary>
    /// <returns></returns>
    private Vector3 GetWorldPosition()
    {
        if (Input.touchCount > 1)
        {
            Ray rayFromTouch = cameraController.GetCamera().ScreenPointToRay(Input.GetTouch(1).position);
            Plane ground = new Plane(cameraController.GetCamera().transform.forward, cameraController.GetTarget().position);
            float distance;
            ground.Raycast(rayFromTouch, out distance);
            return rayFromTouch.GetPoint(distance);
        }
        return Vector3.zero;
    }

    /// <summary>
    /// Calls the camera controller so it can do a rotation.
    /// </summary>
    private void Swipe()
    {
        cameraController.Rotate(firstTouch.deltaPosition.x);
    }
    
    /// <summary>
    /// Makes a tap.
    /// </summary>
    private void Tap()
    {
        Transform selectable;

        if (!isUISelected)
        {
            selectable = raySelector.Select(Camera.main, Input.mousePosition);

            if (selectable)
            {
                cameraController.PositionAndRorationTransition(selectable.position);
            }
        }

        isTapping = false;
    }

    private void SetUISelected(bool isSelected)
    {
        isUISelected = isSelected;
    }

    /// <summary>
    /// Validates if the tap was done in the specific time lapse.
    /// </summary>
    private void StartTapVerification()
    {
        if (tapVerificationCoroutine != null)
            StopCoroutine(tapVerificationCoroutine);

        tapVerificationCoroutine = StartCoroutine(TapVerification());
    }

    /// <summary>
    /// Validates if the tap was done in the specific time lapse.
    /// </summary>
    IEnumerator TapVerification()
    {
        elapsedTime = 0f;

        while (elapsedTime <= tapTimeThreshold)
        {
            if (tapFlag >= 2)
            {
                isTapping = true;
                Tap();
                break;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        tapFlag = 0;
        isTapping = false;
    }

    #endregion
}