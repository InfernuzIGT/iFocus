using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Events;
using TMPro;
using UnityEngine;

public class AnimationQuiz : Animation
{

    [Header("Quiz")]
    [SerializeField] private QuizDataSO _data = null;
    [Space]
    [SerializeField] private CanvasGroup[] _canvasQuiz = null;
    [SerializeField] private CanvasGroup[] _canvasResult = null;
    [Space]
    [SerializeField] private TextMeshProUGUI questionTotalTxt = null;
    [SerializeField] private TextMeshProUGUI questionTxt = null;
    [SerializeField] private TextMeshProUGUI infoTxt = null;
    [SerializeField] private List<AnswerButton> answers = null;
    [Space]
    [SerializeField] private TextMeshProUGUI progressTxt = null;
    [SerializeField] private TextMeshProUGUI questionCorrectTxt = null;
    [SerializeField] private TextMeshProUGUI questionIncorrectTxt = null;

    private QuizData[] _quizData;
    private List<QuizData> _tempQuizData;
    private List<AnswerButton> _answersTemp;
    private List<string> _answersText;

    private int _correctAnswerId;
    private int _counterCorrect;
    private int _counterIncorrect;
    private int _questionIndex;
    private int _maxQuestions;
    private bool _quizVisible = true;

    private CanvasGroup _canvasGroup;
    private WaitForSeconds _waitForSeconds;

    public override void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();

        base.Start();

        _waitForSeconds = new WaitForSeconds(_data.TimeToWait);

        _answersTemp = new List<AnswerButton>();
        _answersText = new List<string>();
        _tempQuizData = new List<QuizData>();

        _quizData = _data.GetQuizData();
    }

    private void OnEnable()
    {
        EventController.AddListener<QuizAnswerEvent>(CheckQuestion);
    }

    private void OnDisable()
    {
        EventController.RemoveListener<QuizAnswerEvent>(CheckQuestion);
    }

    public override void Show()
    {
        base.Show();

        _canvasGroup.DOFade(0, _duration);

    }

    public override void Hide()
    {
        base.Hide();

        _canvasGroup.DOFade(1, _duration);
    }

    public override void SetInteraction(bool interactuable)
    {
        _canvasGroup.interactable = !interactuable;
    }

    private void Switch(bool quizVisible)
    {
        if (quizVisible)
        {
            for (int i = 0; i < _canvasQuiz.Length; i++)
            {
                _canvasQuiz[i].DOFade(1, _duration);
                _canvasResult[i].DOFade(0, _duration);
            }
        }
        else
        {
            for (int i = 0; i < _canvasQuiz.Length; i++)
            {
                _canvasQuiz[i].DOFade(0, _duration);
                _canvasResult[i].DOFade(1, _duration);
            }
        }
    }

    public void LaunchQuiz(bool isMenu)
    {
        _questionIndex = 0;
        _counterCorrect = 0;
        _counterIncorrect = 0;

        if (isMenu)Switch(true);

        _tempQuizData.AddRange(_quizData.ToList());

        _maxQuestions = _data.MaxQuestions;

        if (_maxQuestions >= _quizData.Length)_maxQuestions = _quizData.Length;

        UpdateQuestions();
    }

    private void UpdateQuestions()
    {
        // Reset Color
        for (int i = 0; i < answers.Count; i++)
            answers[i].SetState(AnswerState.None);

        // Finish game
        if (_questionIndex == _maxQuestions)
        {
            FinishGame();
            return;
        }

        int randomData = UnityEngine.Random.Range(0, _tempQuizData.Count);

        // Set title
        questionTxt.text = _tempQuizData[randomData].question;

        // Add content to temporal lists
        _answersTemp.AddRange(answers);
        _answersText.Add(_tempQuizData[randomData].answerFalse01);
        _answersText.Add(_tempQuizData[randomData].answerFalse02);
        _answersText.Add(_tempQuizData[randomData].answerFalse03);
        // _answersText.Add(_tempQuizData[randomData].answerFalse04);

        // Random to pick a correct answer
        int r = UnityEngine.Random.Range(0, _answersText.Count);

        // Set the correct answer
        _correctAnswerId = r;
        _answersTemp[r].SetText(_tempQuizData[randomData].answerTrue);
        _answersTemp.Remove(_answersTemp[r]);

        // Random to false answers
        for (int i = 0; i < _answersTemp.Count; i++)
        {
            r = UnityEngine.Random.Range(0, _answersText.Count);
            _answersTemp[i].SetText(_answersText[r]);
            _answersText.Remove(_answersText[r]);
        }

        _questionIndex++;
        _tempQuizData.Remove(_tempQuizData[randomData]);

        progressTxt.text = string.Format("{0}/{1}", _questionIndex, _maxQuestions);

        // Se limpian todas las listas
        _answersTemp.Clear();
        _answersText.Clear();
    }

    private IEnumerator NextQuestion()
    {
        yield return _waitForSeconds;

        UpdateQuestions();
    }

    public void CheckQuestion(QuizAnswerEvent evt)
    {
        // Set colors to True/False
        for (int i = 0; i < answers.Count; i++)
            answers[i].SetState(i == _correctAnswerId ? AnswerState.Correct : AnswerState.Incorrect);

        // Add Points
        if (_correctAnswerId == evt.id)
        {
            _counterCorrect++;
        }
        else
        {
            _counterIncorrect++;
        }

        StartCoroutine(NextQuestion());
    }

    private void FinishGame()
    {
        float porcentage = (_counterCorrect * 100) / _maxQuestions;

        questionTxt.text = string.Format("{0}%", porcentage);
        progressTxt.text = "-";

        questionTotalTxt.text = _maxQuestions.ToString();
        questionCorrectTxt.text = _counterCorrect.ToString();;
        questionIncorrectTxt.text = _counterIncorrect.ToString();

        Switch(false);
    }
}