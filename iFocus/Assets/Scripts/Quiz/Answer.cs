using Events;
using UnityEngine;
using UnityEngine.UI;

public enum AnswerState
{
    None = 0,
    Correct = 1,
    Incorrect = 2
}

public class Answer : MonoBehaviour
{
    [SerializeField] private int id = 0;
    [Space]
    [SerializeField] private Color colorNone = Color.white;
    [SerializeField] private Color colorCorrect = Color.green;
    [SerializeField] private Color colorIncorrect = Color.red;

    private Image _image;
    private Button _button;
    private Text _text;

    private QuizAnswerEvent _quizAnswerEvent;
    private AnswerState _state;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _button = GetComponent<Button>();
        _text = GetComponentInChildren<Text>();
    }

    private void Start()
    {
        _quizAnswerEvent = new QuizAnswerEvent();
        _quizAnswerEvent.id = id;

        _button.onClick.AddListener(Select);
    }

    private void Select()
    {
        if (_state != AnswerState.None) return;
        
        EventController.TriggerEvent(_quizAnswerEvent);
    }

    public void SetText(string text)
    {
        _text.text = text;
    }

    public void SetState(AnswerState state)
    {
        _state = state;
        
        switch (state)
        {
            case AnswerState.None:
                _text.color = Color.black;
                _image.color = colorNone;
                break;

            case AnswerState.Correct:
                _text.color = Color.white;
                _image.color = colorCorrect;
                break;

            case AnswerState.Incorrect:
                _text.color = Color.white;
                _image.color = colorIncorrect;
                break;
        }
    }

}