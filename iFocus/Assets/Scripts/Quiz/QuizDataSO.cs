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

[CreateAssetMenu(fileName = "New Quiz Data", menuName = "ScriptableObjects/Quiz Data", order = 1)]
public class QuizDataSO : ScriptableObject
{
    public string fileNameQuiz = "QuizData";
    [SerializeField] private QuizData[] quizData;

    private JSONConverter jsonConverter = new JSONConverter();

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
        Debug.Log($"<color=green><b>[QUIZ] </b></color> Success");
    }

    private void FailQuiz()
    {
        Debug.Log($"<color=red><b>[QUIZ] </b></color> Fail");
    }

}