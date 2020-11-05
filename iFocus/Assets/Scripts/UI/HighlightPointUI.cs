using UnityEngine;

public class HighlightPointUI : MonoBehaviour
{
    [SerializeField] private GameObject[] _objectsToActivate;
    [SerializeField] private bool _isSelected = false;
    [SerializeField] private bool _isLocked = true;

    // Properties
    public bool IsSelected 
    { 
        get 
        {
            return _isSelected; 
        } 
        set 
        {
            _isSelected = value;

            _objectsToActivate[0].SetActive(_isSelected);
            _objectsToActivate[1].SetActive(!_isSelected);

        }
    }
    public bool IsLocked { get { return _isLocked; } set { _isLocked = value; } }
}