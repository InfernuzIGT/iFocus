using UnityEngine;
using UnityEngine.EventSystems;

public class SideScreenButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private CanvasGroup _canvasGroup;

    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        _canvasGroup.interactable = true;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        _canvasGroup.interactable = false;
    }
}