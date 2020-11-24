using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonTimeline : MonoBehaviour
{
    [Header("Button Timeline")]
    [SerializeField] private bool _isSelected = false;
    [SerializeField] private bool _isLocked = true;

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

    public void AddListener(UnityAction action)
    {
        _button.onClick.AddListener(action);
    }

}