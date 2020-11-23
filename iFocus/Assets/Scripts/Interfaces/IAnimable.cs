/// <summary>
/// This Interface should be implemented by all the clases that has elements wich may be animated with DOTween.
/// </summary>
public interface IAnimable
{
    void Show();

    void Hide();

    void SetInteraction(bool interactuable);

}