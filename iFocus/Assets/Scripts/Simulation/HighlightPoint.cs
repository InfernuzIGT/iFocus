using Events;
using UnityEngine;

public class HighlightPoint : MonoBehaviour, IRaySelectable
{
    [Header("Highlight Point")]
    [SerializeField] private int id = 0;
    [SerializeField] private bool _isSelected = false;
    [SerializeField] private bool _isLocked = true;

    private GameObject _cameraTransform;
    private HightlightDataEvent _hightlightDataEvent;
    private StatePauseHPEvent _statePauseHPEvent;
    private Material _material;

    private int hash_IsHighlited = Shader.PropertyToID("_isHighlited");
    private int hash_IsSelected = Shader.PropertyToID("_isSelected");

    // Properties
    public bool IsSelected { get { return _isSelected; } set { _isSelected = value; } }
    public bool IsLocked { get { return _isLocked; } set { _isLocked = value; } }

    private Camera _camera;

    private void OnEnable()
    {
        EventController.AddListener<HightlightDataEvent>(OnHighlightDataEvent);
    }

    private void OnDisable()
    {
        EventController.RemoveListener<HightlightDataEvent>(OnHighlightDataEvent);
    }

    private void Awake()
    {
        _material = GetComponent<MeshRenderer>().material;
        _camera = Camera.main;
    }

    private void Start()
    {
        _hightlightDataEvent = new HightlightDataEvent();
        _hightlightDataEvent.id = id;
        _hightlightDataEvent.action = Unselect;

        _statePauseHPEvent = new StatePauseHPEvent();

        ChangeLock(_isLocked);
    }

    public void Highlight(bool enable)
    {
        _isLocked = !enable;

        if (_isLocked)
        {
            _material.SetFloat(hash_IsHighlited, 0);
        }
        else
        {
            _material.SetFloat(hash_IsHighlited, 1);

            EventController.TriggerEvent(_statePauseHPEvent);
        }

    }

    private void OnMouseDown()
    {
        if (_isSelected || _isLocked) return;

        Select();
    }

    [ContextMenu("Select")]
    public void Select()
    {
        /*
        IsSelected = true;
        _material.SetFloat(hash_IsSelected, 1);
        */
        //TODO: cambiar estado de boton Master

        EventController.TriggerEvent(_hightlightDataEvent);

        //if (!_isLocked)
        //{
        //    IsSelected = true;

        //    EventController.TriggerEvent(_hightlightDataEvent);
        //}
    }

    public void Unselect()
    {
        // Debug.Log($"<color=green><b>[UNSELECTED]</b></color> Highlight Point - ID {id}");
        _material.SetFloat(hash_IsSelected, 0);
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

        //_material.SetFloat("_isLocked", _isLocked ? 1 : 0);
    }
    private void OnHighlightDataEvent(HightlightDataEvent eventHandler)
    {
        if (eventHandler.id == id)
        {
            IsSelected = true;
            _material.SetFloat(hash_IsSelected, 1);
        }
    }
}