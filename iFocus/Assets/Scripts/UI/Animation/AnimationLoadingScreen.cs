using DG.Tweening;

public class AnimationLoadingScreen : Animation
{
    public override void Start()
    {
        Hide();
    }

    public override void Hide()
    {
        transform.DOLocalMoveY(720, _duration)
            .SetRelative()
            .SetDelay(_delay)
            .OnComplete(AutoDestroy);
    }

    private void AutoDestroy()
    {
        Destroy(this.gameObject);
    }
}