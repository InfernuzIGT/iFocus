using DG.Tweening;
using UnityEngine;

public class Animation : MonoBehaviour, IAnimable
{
    [Header("Animation")]
    [SerializeField, Range(0f, 3f)] protected float _duration = 1;
    [SerializeField, Range(0f, 5f)] protected float _delay = .5f;
    [SerializeField] protected Ease _ease = Ease.InOutQuad;

    public virtual void Start()
    {
        SetInteraction(false);
    }

    public virtual void Show()
    {
        SetInteraction(true);
    }

    public virtual void Hide()
    {
        SetInteraction(false);
    }

    public virtual void SetInteraction(bool interactuable) { }
}