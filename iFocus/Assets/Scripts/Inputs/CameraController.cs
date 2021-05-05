using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Pyros;
using System;
using UnityEngine.UI;

/// <summary>
/// Use this class to make changes on the camera position, rotation, and zooming values.
/// </summary>
public class CameraController : MonoBehaviour
{
    /// <summary>
    /// Scene Main Camera.
    /// </summary>
    private Camera mainCamera;

    /// <summary>
    /// CineMachine FreeLookCamera.
    /// </summary>
    public CinemachineFreeLook CMCamera;

    [SerializeField] private SettingsSO _settings;

    /// <summary>
    /// The initial camera position.
    /// </summary>
    [SerializeField] private Vector3 _cameraOriginalPosition;

    /// <summary>
    /// The target that the CM Camera will follow.
    /// </summary>
    [SerializeField] private Transform _target;

    /// <summary>
    /// Rotation sensibility.
    /// </summary>
    [SerializeField] private float _rotationSensibility = 100;

    /// <summary>
    /// The zoom of the FreeLook Camera, based on the initial FieldOfView.
    /// </summary>
    [SerializeField] private float _cameraZoom = 40;

    /// <summary>
    /// Zoom Speed.
    /// </summary>
    [SerializeField] private int _zoomSpeed = 1;

    [SerializeField] private Vector3[] _targetPositions;

    private Vector3 _targetInitialPosition;

    private float _initialZoomValue = 1;
    private float _zoomFinalValue = 0;
    private Coroutine _ZoomCoroutine;

    private float _amountOfRotation = 0;
    private bool _isRotating = false;

    private void Awake()
    {
        mainCamera = Camera.main;
        Setups();
        SetTarget();
    }

    private void Setups()
    {
        _targetInitialPosition = _target.transform.position;
        _initialZoomValue = CMCamera.m_Lens.FieldOfView;
    }

    /// <summary>
    /// Returns the scene main camera.
    /// </summary>
    /// <returns></returns>
    public Camera GetCamera()
    {
        return Camera.main;
    }

    /// <summary>
    /// Returns the CineMachine FreeLook camera used in the scene.
    /// </summary>
    /// <returns></returns>
    public CinemachineFreeLook GetCMCamera()
    {
        return CMCamera;
    }

    /// <summary>
    /// Returns the current target the CM camera is following.
    /// </summary>
    /// <returns></returns>
    public Transform GetTarget()
    {
        return _target;
    }

    /// <summary>
    /// Sets the target you want the camera to follow.
    /// </summary>
    private void SetTarget()
    {
        // TODO: Revisar si es necesario hacer una sobrecarga del metodo para resivir otro target. De ser asi revisar su documentacion.
        CMCamera.Follow = _target;
        CMCamera.LookAt = _target;
    }

    /// <summary>
    /// Makes the camera go the the desired position.
    /// </summary>
    /// <param name="newPosition"></param>
    public Coroutine PositionAndRorationTransition(Vector3 newPosition)
    {
        return StartCoroutine(Transition(newPosition, _settings._transitionSpeed));
    }

    /// <summary>
    /// Makes the camera go the the desired position.
    /// </summary>
    /// <param name="newTransform"></param>
    public void PositionAndRorationTransition(Vector3 newPosition, float transitionTime)
    {
        StartCoroutine(Transition(newPosition, transitionTime));
    }

    /// <summary>
    /// Makes a Linear interpolation between to positions of the cameras target.
    /// </summary>
    /// <param name="newPosition"></param>
    /// <returns></returns>
    private IEnumerator Transition(Vector3 newPosition, float transitionTime)
    {
        Vector3 initialPosition = _target.position;
        float elapsedTime = 0;

        float xPosition, yPosition, zPosition;
        xPosition = initialPosition.x;
        yPosition = initialPosition.y;
        zPosition = initialPosition.z;

        while (elapsedTime <= transitionTime)
        {
            xPosition = Mathf.Lerp(initialPosition.x, newPosition.x, _settings._transitionCurve.Evaluate( elapsedTime / transitionTime));
            yPosition = Mathf.Lerp(initialPosition.y, newPosition.y, _settings._transitionCurve.Evaluate(elapsedTime / transitionTime));
            zPosition = Mathf.Lerp(initialPosition.z, newPosition.z, _settings._transitionCurve.Evaluate(elapsedTime / transitionTime));
            _target.position = new Vector3(xPosition, yPosition, zPosition);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _target.position = newPosition;
    }

    /// <summary>
    /// Input Zoom
    /// </summary>
    /// <param name="newZoomValue"></param>
    public void StartZoom(float newZoomValue)
    {
        if (_ZoomCoroutine != null)
        {
            Debug.Log("Ya se esta ejecutando el Zoom");
            _zoomFinalValue = CMCamera.m_Lens.FieldOfView + newZoomValue;
        }
        else
        {
            Debug.Log("Aun no se ejecuto");
            _ZoomCoroutine = StartCoroutine(PinchZoom(newZoomValue));
        }
    }

    /// <summary>
    /// Performs a Zoom on the camera filed of view.
    /// </summary>
    /// <param name="zoomValue"></param>
    public void Zoom(float zoomValue)
    {
        // TODO: Tanto el zoom como el rotate deberian ser corutinas, para poder lerpear.

        CMCamera.m_Lens.FieldOfView = Mathf.Clamp(CMCamera.m_Lens.FieldOfView + (zoomValue * _zoomSpeed), 15, 90);
    }

    /// <summary>
    /// Used for generic zooms executed by the CM camera.
    /// </summary>
    /// <param name="zoomValue"></param>
    /// <returns></returns>
    public Coroutine RunZoom(float zoomValue)
    {
        return StartCoroutine(ZoomCoroutine(zoomValue));
    }
    
    /// <summary>
    /// Used for generic zooms executed by the CM camera.
    /// </summary>
    /// <param name="amountOfZoom"></param>
    /// <returns></returns>
    private IEnumerator ZoomCoroutine(float amountOfZoom)
    {
        float elapsedTime = 0f;
        float _startingPoint = CMCamera.m_Lens.FieldOfView;
        _zoomFinalValue = amountOfZoom;

        while (elapsedTime <= _settings._zoomInTime)
        {
            CMCamera.m_Lens.FieldOfView = Mathf.Lerp(_startingPoint, _zoomFinalValue, _settings._zoomInCurve.Evaluate(elapsedTime / _settings._zoomInTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        CMCamera.m_Lens.FieldOfView = _zoomFinalValue;
    }

    /// <summary>
    /// Used only for zooms performed with touch inputs.
    /// </summary>
    /// <param name="zoomValue"></param>
    /// <returns></returns>
    public IEnumerator PinchZoom(float zoomValue)
    {
        float _startingPoint = CMCamera.m_Lens.FieldOfView;
        _zoomFinalValue = _startingPoint + zoomValue;

        while (SimulationManager._control.IsModeEndingOn())
        {
            Debug.Log("Is Ending Mode");
            while (!(CMCamera.m_Lens.FieldOfView <= (_zoomFinalValue + 0.1f) && CMCamera.m_Lens.FieldOfView >= (_zoomFinalValue - 0.1f)))
            {
                CMCamera.m_Lens.FieldOfView = Mathf.Lerp(CMCamera.m_Lens.FieldOfView, _zoomFinalValue, 0.05f);
                yield return null;
            }
            CMCamera.m_Lens.FieldOfView = _zoomFinalValue;
            yield return null;
        }
    }

    /// <summary>
    /// Performs a rotation arround the current target.
    /// </summary>
    /// <param name="delta"></param>
    public void Rotate(float delta)
    {
        if (_isRotating)
        {
            _amountOfRotation += delta;
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(Rotating(delta));
        }
    }

    private IEnumerator Rotating(float delta)
    {
        _amountOfRotation = CMCamera.m_XAxis.Value + delta;

        float deltaRotation = _amountOfRotation * 0.1f;

        while (_amountOfRotation >= _settings._rotationDeadPoint || _amountOfRotation <= -_settings._rotationDeadPoint)
        {
            CMCamera.m_XAxis.Value += (_amountOfRotation * _settings._rotationSpeedFactor);
            _amountOfRotation -= (_amountOfRotation * _settings._rotationSpeedFactor);
            yield return null;
        }
    }

    [ContextMenu("TestRotation")]
    public void TestRotation()
    {
        CMCamera.m_XAxis.Value += 180;
        
    }

    /// <summary>
    /// Moves the target to a new location giving the illusion of panning.
    /// </summary>
    /// <param name="newPosition"></param>
    public void Pan(Vector3 newPosition)
    {
        _target.position += newPosition;
    }

    /// <summary>
    /// Resets the camera to its original location.
    /// </summary>
    public Coroutine Reset()
    {
        StopAllCoroutines();

        StartCoroutine(ZoomCoroutine(_initialZoomValue));
        return StartCoroutine(Transition(_targetInitialPosition, _settings._transitionSpeed));
    }

    /// <summary>
    /// Returns the camera zoom to its original value.
    /// </summary>
    /// <returns></returns>
    public Coroutine ZoomOut()
    {
        return StartCoroutine(ZoomCoroutine(_initialZoomValue));
    }

    public Coroutine ZoomIn()
    {
        return StartCoroutine(ZoomCoroutine(_cameraZoom));
    }
}
