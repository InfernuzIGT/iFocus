using DG.Tweening;
using Events;
using UnityEngine;

public class AnimationTimeline : Animation
{
    [Header("Timeline")]
    [SerializeField] private ButtonTimeline[] _buttonsTimeline;
    [Space]
    [SerializeField] private CanvasGroup _canvasSimulation = null;
    [SerializeField] private CanvasGroup _canvasPhatologies = null;

    private bool _simulationVisible = true;

    public override void Start()
    {
        base.Start();
    }

    public void ToggleAll(bool visible)
    {
        for (int i = 0; i < _buttonsTimeline.Length; i++)
        {
            _buttonsTimeline[i].IsSelected = visible;
        }
    }

    private void OnEnable()
    {
        EventController.AddListener<SwitchEvent>(Switch);
    }

    private void OnDisable()
    {
        EventController.RemoveListener<SwitchEvent>(Switch);
    }

    public override void Show()
    {
        base.Show();

        _canvasSimulation.transform.DOLocalMoveY(-150, _duration);
        _canvasPhatologies.transform.DOLocalMoveY(0, _duration);
        
        _canvasSimulation.DOFade(0, _duration);
        _canvasPhatologies.DOFade(1, _duration);
    }

    public override void Hide()
    {
        base.Hide();

        _canvasSimulation.transform.DOLocalMoveY(0, _duration);
        _canvasPhatologies.transform.DOLocalMoveY(-150, _duration);
        
        _canvasSimulation.DOFade(1, _duration);
        _canvasPhatologies.DOFade(0, _duration);
    }

    public override void SetInteraction(bool interactuable)
    {
        _canvasSimulation.interactable = !interactuable;
        _canvasPhatologies.interactable = interactuable;
    }

    public void Switch(SwitchEvent evt)
    {
        _simulationVisible = !_simulationVisible;

        if (_simulationVisible)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

}