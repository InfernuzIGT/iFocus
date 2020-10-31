using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Events;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class is responsable for all the backend logic of the Quiz module.
/// </summary>
public class QuizController : MonoBehaviour
{
    [Header("Data")]
    public QuizDataSO quizData;

    [Header("Gameplay")]
    public int maxQuestions = 5;
    public int questionIndex;
    [Space]
    public int correctAnswerId;
    public int counterCorrect;
    public int counterIncorrect;

    [Header("Questions")]
    public float timeToChoose = 3;
    public float timeToWait = 3;

    [Header("UI")]
    public GameObject contentAnswers;
    public GameObject contentResults;
    [Space]
    public Text progressTxt;
    public Text questionTxt;
    public List<Answer> answers;
    [Space]
    public Text questionTotalTxt;
    public Text questionCorrectTxt;
    public Text questionIncorrectTxt;

    private QuizData[] _quizData;
    private List<QuizData> _tempQuizData;
    private List<Answer> _answersTemp;
    private List<string> _answersText;

    private WaitForSeconds _waitForSeconds;

    private void Start()
    {
        _waitForSeconds = new WaitForSeconds(timeToWait);

        _answersTemp = new List<Answer>();
        _answersText = new List<string>();
        _tempQuizData = new List<QuizData>();

        _quizData = quizData.GetQuizData();
    }

    private void OnEnable()
    {
        EventController.AddListener<QuizAnswerEvent>(CheckQuestion);
    }

    private void OnDisable()
    {
        EventController.RemoveListener<QuizAnswerEvent>(CheckQuestion);
    }

    public void LaunchQuiz()
    {
        questionIndex = 0;
        counterCorrect = 0;
        counterIncorrect = 0;

        contentAnswers.SetActive(true);
        contentResults.SetActive(false);

        _tempQuizData.AddRange(_quizData.ToList());

        if (maxQuestions >= _quizData.Length)maxQuestions = _quizData.Length;

        UpdateQuestions();
    }

    private void UpdateQuestions()
    {
        // Reset Color
        for (int i = 0; i < answers.Count; i++)
            answers[i].SetState(AnswerState.None);

        // Finish game
        if (questionIndex == maxQuestions)
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
        correctAnswerId = r;
        _answersTemp[r].SetText(_tempQuizData[randomData].answerTrue);
        _answersTemp.Remove(_answersTemp[r]);

        // Random to false answers
        for (int i = 0; i < _answersTemp.Count; i++)
        {
            r = UnityEngine.Random.Range(0, _answersText.Count);
            _answersTemp[i].SetText(_answersText[r]);
            _answersText.Remove(_answersText[r]);
        }

        questionIndex++;
        _tempQuizData.Remove(_tempQuizData[randomData]);

        progressTxt.text = string.Format("{0}/{1}", questionIndex, maxQuestions);

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
            answers[i].SetState(i == correctAnswerId ? AnswerState.Correct : AnswerState.Incorrect);

        // Add Points
        if (correctAnswerId == evt.id)
        {
            counterCorrect++;
        }
        else
        {
            counterIncorrect++;
        }

        StartCoroutine(NextQuestion());
    }

    private void FinishGame()
    {
        float porcentage = (counterCorrect * 100) / maxQuestions;

        questionTxt.text = string.Format("{0}%", porcentage);
        progressTxt.text = "-";

        questionTotalTxt.text = maxQuestions.ToString();
        questionCorrectTxt.text = counterCorrect.ToString();;
        questionIncorrectTxt.text = counterIncorrect.ToString();

        contentAnswers.SetActive(false);
        contentResults.SetActive(true);
    }

}