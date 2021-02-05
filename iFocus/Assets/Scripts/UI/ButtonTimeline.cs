using Events;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTimeline : MonoBehaviour
{
    [Header("Button Timeline")]
    [SerializeField] private int id = 0;
    [SerializeField] private bool _isSelected = false;
    [SerializeField] private bool _isLocked = true;

    private HightlightDataEvent _hightlightDataEvent;
    private StatePauseHPEvent _statePauseHPEvent;

    private Button _button;
    private CanvasGroup _canvasGroup;

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

            _canvasGroup.interactable = _isSelected;
        }
    }

    public bool IsLocked { get { return _isLocked; } set { _isLocked = value; } }

    private void Awake()
    {
        _button = GetComponent<Button>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        _hightlightDataEvent = new HightlightDataEvent();
        _hightlightDataEvent.id = id;

        _statePauseHPEvent = new StatePauseHPEvent();
        
        _button.onClick.AddListener(Select);
    }

    private void Select()
    {
        IsSelected = true;

        EventController.TriggerEvent(_hightlightDataEvent);
    }

}