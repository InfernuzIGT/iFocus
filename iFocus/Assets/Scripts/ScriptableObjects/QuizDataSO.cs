using System;
using UnityEngine;

[Serializable]
public struct QuizData
{
    public int id;
    public string question;
    public string answerTrue;
    public string answerFalse01;
    public string answerFalse02;
    public string answerFalse03;
    // public string answerFalse04;
}

public enum AnswerState
{
    None = 0,
    Correct = 1,
    Incorrect = 2
}

[Serializable]
public class ColorState
{
    public Color text;
    public Color button;
}

[CreateAssetMenu(fileName = "New Quiz Data", menuName = "ScriptableObjects/Quiz Data", order = 1)]
public class QuizDataSO : ScriptableObject
{
    [Header("General")]
    [SerializeField] private int _maxQuestions = 5;
    [SerializeField] private float _timeToWait = 3;
    [Space]
    [SerializeField] private ColorState stateNone = null;
    [SerializeField] private ColorState stateCorrect = null;
    [SerializeField] private ColorState stateIncorrect = null;

    [Header("Settings")]
    [SerializeField] private string fileNameQuiz = "QuizData";
    [SerializeField] private QuizData[] quizData;

    private JSONConverter jsonConverter = new JSONConverter();

    // Properties
    public int MaxQuestions { get { return _maxQuestions; } }
    public float TimeToWait { get { return _timeToWait; } }

    [ContextMenu("Load Data")]
    public void LoadData()
    {
        quizData = jsonConverter.GetData<QuizData>(fileNameQuiz, SuccessQuiz, FailQuiz);
    }

    public QuizData GetQuizDataById(int id)
    {
        return quizData[id];
    }

    public QuizData[] GetQuizData()
    {
        return quizData;
    }

    private void SuccessQuiz()
    {
#if UNITY_EDITOR
        Debug.Log($"<color=green><b>[QUIZ] </b></color> Success");
#endif
    }

    private void FailQuiz()
    {
#if UNITY_EDITOR
        Debug.Log($"<color=red><b>[QUIZ] </b></color> Fail");
#endif
    }

    public Color GetTextColor(AnswerState state)
    {
        switch (state)
        {
            case AnswerState.None:
                return stateNone.text;

            case AnswerState.Correct:
                return stateCorrect.text;

            case AnswerState.Incorrect:
                return stateIncorrect.text;
        }

        return Color.black;
    }
    
    public Color GetButtonColor(AnswerState state)
    {
        switch (state)
        {
            case AnswerState.None:
                return stateNone.button;

            case AnswerState.Correct:
                return stateCorrect.button;

            case AnswerState.Incorrect:
                return stateIncorrect.button;
        }

        return Color.white;
    }

}