using Events;
using UnityEngine;

public class HighlightPoint : MonoBehaviour, IRaySelectable
{
    public HighlightCategory category; // TODO Mariano: REMOVE
    [Space]

    [SerializeField] private int id = 0;
    [SerializeField] private bool _isSelected = false;
    [SerializeField] private bool _isLocked;
    [Space]
    [SerializeField] private Transform cameraTransform = null; // TODO: Remover y reemplazar por variables en Data

    private GameObject _cameraTransform;
    private HightlightDataEvent _hightlightDataEvent;
    private Material _material;

    // Properties
    public bool IsSelected { get { return _isSelected; } set { _isSelected = value; } }
    public bool IsLocked { get { return _isLocked; } set { _isLocked = value; } }

    private Camera _camera;

    private void Awake()
    {
        // _material = GetComponentInChildren<SpriteRenderer>().material;
        // _material = GetComponent<MeshRenderer>().material;
        _camera = Camera.main;
    }

    private void Start()
    {
        _hightlightDataEvent = new HightlightDataEvent();
        _hightlightDataEvent.id = id;
        _hightlightDataEvent.action = Unselect;

        ChangeLock(_isLocked);

        _material.SetFloat("_Category", (int)category);
    }

    public Transform GetTransform()
    {
        // return _cameraTransform;
        return cameraTransform;
    }

    [ContextMenu("Select")]
    public void Select()
    {
        // Debug.Log ($"SELECT");
        if (!_isLocked)
        {
            // Debug.Log($"<color=green><b>[SELECTED]</b></color> Highlight Point - ID {id}");

            IsSelected = true;

            EventController.TriggerEvent(_hightlightDataEvent);
        }
    }

    public void Unselect()
    {
        // Debug.Log($"<color=green><b>[UNSELECTED]</b></color> Highlight Point - ID {id}");
        IsSelected = false;
        SimulationManager._control.RestarTimeLine();
    }

    [ContextMenu("Lock")]
    public void Lock()
    {
        ChangeLock(true);
    }

    [ContextMenu("Unlock")]
    public void Unlock()
    {
        ChangeLock(false);
    }

    public void ChangeLock(bool isLocked)
    {
        _isLocked = isLocked;

        // _material.SetFloat("_isLocked", _isLocked ? 1 : 0);
    }
}