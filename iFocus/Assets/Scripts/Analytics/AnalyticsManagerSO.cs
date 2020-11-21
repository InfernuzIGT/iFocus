using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

[CreateAssetMenu(fileName = "Analytics Manager", menuName = "ScriptableObjects/Analytics", order = 3)]
public class AnalyticsManagerSO : ScriptableObject
{
    /// <summary>
    /// Call this function when you open the quiz menu.
    /// </summary>
    public void QuizStarted()
    {
        AnalyticsResult analyticsResult = Analytics.CustomEvent("Quiz Started");
        Debug.Log("Analytics Result: " + analyticsResult);
    }

    /// <summary>
    /// Use this to send a custom avent with a result percentage when the user finished the current quiz.
    /// </summary>
    /// <param name="result">Result values in 0 to 100 percentage.</param>
    public void QuizEnded(int result)
    {
        AnalyticsResult analyticsResult = Analytics.CustomEvent("Quiz Ended",
        new Dictionary<string, object> { {"Percentage: " , result} });
        Debug.Log("Analytics Result: " + analyticsResult);
    }

    /// <summary>
    /// This function should be call when ever a user finish the action of selection an answer from the quiz.
    /// </summary>
    /// <param name="id">The unic question ID.</param>
    /// <param name="answerResult">The result of the selection.</param>
    public void AnswerSelected(int id, bool answerResult)
    {
        AnalyticsResult analyticsResult = Analytics.CustomEvent("Answer" + id,
        new Dictionary<string, object> { { "Value: ", answerResult? "Correct:": "False"} });
        Debug.Log("Analytics Result: " + analyticsResult);
    }
}
