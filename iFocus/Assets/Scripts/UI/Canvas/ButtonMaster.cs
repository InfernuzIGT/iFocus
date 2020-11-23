using UnityEngine;
using UnityEngine.UI;

public class ButtonMaster : MonoBehaviour
{
    [Header ("Button Master")]
    [SerializeField] private ButtonMasterSO _data = null;
    [SerializeField] private BUTTONMASTER_STATE _currentState = BUTTONMASTER_STATE.Food;
    [Space]
    [SerializeField] private Button _skipBtn = null;
    [SerializeField] private Button _resetBtn = null;
    [Space]
    [SerializeField] private Image _image = null;

    [Header("Animation")]
    [SerializeField] private Animation _animationButtonMaster = null;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void Start()
    {
        _image.sprite = _data.GetIcon(_currentState);

        _button.onClick.AddListener(Execute);
    }

    public void ChangeState(BUTTONMASTER_STATE newState)
    {
        _currentState = newState;
        _image.sprite = _data.GetIcon(_currentState);
    }

    private void Execute()
    {
        Debug.Log($"Button Master message. Current State: {_currentState}");
    }

    [ContextMenu("Show")]
    public void Show()
    {
        _animationButtonMaster.Show();
    }

    [ContextMenu("Hide")]
    public void Hide()
    {
        _animationButtonMaster.Hide();
    }

}