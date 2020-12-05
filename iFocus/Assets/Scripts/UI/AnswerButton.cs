using Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnswerButton : MonoBehaviour
{
    [Header("Answer")]
    [SerializeField] private QuizDataSO _data;
    [Space]
    [SerializeField] private int id = 0;

    private Image _image;
    private Button _button;
    private TextMeshProUGUI _text;

    private QuizAnswerEvent _quizAnswerEvent;
    private AnswerState _state;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _button = GetComponent<Button>();
        _text = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        _quizAnswerEvent = new QuizAnswerEvent();
        _quizAnswerEvent.id = id;

        _button.onClick.AddListener(Select);
    }

    private void Select()
    {
        if (_state != AnswerState.None)return;

        EventController.TriggerEvent(_quizAnswerEvent);
    }

    public void SetText(string text)
    {
        _text.text = text;
    }

    public void SetState(AnswerState state)
    {
        _state = state;

        _text.color = _data.GetTextColor(_state);
        _image.color = _data.GetButtonColor(_state);
    }

}