using System;
using UnityEngine;
using UnityEngine.Video;

public enum HighlightCategory
{
    None = 0,
    Initiator = 1,
    EffectorNormal = 2,
    EffectorDM = 3,
    Pathology = 4
}

[Serializable]
public class HighlightData
{
    public int id;
    public string title;
    public string descriptionShort;
    public string description;
    public Texture videoPreview;
    public VideoClip video;
}

[CreateAssetMenu(fileName = "New Highlight Data", menuName = "ScriptableObjects/Highlight Data", order = 1)]
public class HighlightDataSO : ScriptableObject
{
    public string fileNameHighlight = "HighlightData";
    [SerializeField] private HighlightData[] highlightData;

    private JSONConverter jsonConverter = new JSONConverter();

    [ContextMenu("Load Data")]
    public void LoadData()
    {
        highlightData = jsonConverter.GetData<HighlightData>(fileNameHighlight, SuccessHighlight, FailHighlight);
    }

    public HighlightData GetHighlightDataById(int id)
    {
        return highlightData[id];
    }

    private void SuccessHighlight()
    {
        Debug.Log($"<color=green><b>[HIGHLIGHT] </b></color> Success");
    }

    private void FailHighlight()
    {
        Debug.Log($"<color=red><b>[HIGHLIGHT] </b></color> Fail");
    }

}