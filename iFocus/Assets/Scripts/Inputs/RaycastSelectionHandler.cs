using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This class is used for select components with a raycast. This components should have the implementation of IRaySelectable.
/// </summary>
public class RaycastSelectionHandler : MonoBehaviour
{
    private Ray _ray;
    private RaycastHit _raycastHit;
    private IRaySelectable _selection;

    /// <summary>
    /// Use has a handler to select an IRaySelectable with a Raycast.
    /// </summary>
    /// <param name="currentCamera">The camera from wich the raycast is going to by casted.</param>
    /// <param name="touchPosition">The screenPosition from wich the ray is casted.</param>
    public Transform Select(Camera currentCamera, Vector2 touchPosition)
    {
        _ray = currentCamera.ScreenPointToRay(touchPosition);
        
        if (Physics.Raycast(_ray, out _raycastHit))
        {
            if (_selection != null)
                _selection.Unselect();

            _selection = _raycastHit.transform.GetComponent<IRaySelectable>();

            if (_selection != null)
            {
                if (!_selection.IsSelected)
                {
                    _raycastHit.transform.GetComponent<IRaySelectable>().Select();
                    return _raycastHit.transform;
                }
            }
        }
        else if (_selection != null)
            _selection.Unselect();

        return null;
    }
}
