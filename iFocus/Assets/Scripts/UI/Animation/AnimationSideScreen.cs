using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AnimationSideScreen : Animation
{
    [Header("Side Screen")]
    [SerializeField] private RectTransform _slideScreen = null;
    [Space]
    [SerializeField] private Button[] _simulationBtn = null;
    [Space]
    [SerializeField] private Button _quizBtn = null;
    [SerializeField] private Button _configurationBtn = null;
    [SerializeField] private Button _creditsBtn = null;
    [SerializeField] private Button _backBtn = null;

    [Header("Other")]
    [SerializeField] private AnimationQuiz _animationQuiz = null;

    private CanvasGroup _canvasGroup;

    public override void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();

        _quizBtn.onClick.AddListener(OpenQuiz);
        _configurationBtn.onClick.AddListener(OpenConfiguration);
        _creditsBtn.onClick.AddListener(OpenCredits);
        _backBtn.onClick.AddListener(Hide);

        base.Start();
    }

    [ContextMenu("Show")]
    public override void Show()
    {
        base.Show();

        _canvasGroup.DOFade(1, _duration);
        _slideScreen.DOMoveX(0, _duration);
    }

    [ContextMenu("Hide")]
    public override void Hide()
    {
        base.Hide();

        _canvasGroup.DOFade(0, _duration);
        _slideScreen.DOMoveX(-376, _duration);
    }

    public override void SetInteraction(bool interactuable)
    {
        _canvasGroup.interactable = interactuable;
        _canvasGroup.blocksRaycasts = interactuable;
    }

    public void SetFirstButton()
    {
        _simulationBtn[0].Select();
    }

    private void OpenQuiz()
    {
        Hide();
        _animationQuiz.LaunchQuiz(true);
        _animationQuiz.Show();
    }

    private void OpenConfiguration()
    {
        Debug.Log($"Open: Configuration");
    }

    private void OpenCredits()
    {
        Debug.Log($"Open: Credits");
    }

}