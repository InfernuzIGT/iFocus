using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SideScreenButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [Header("Side Screen Button")]
    [SerializeField] private Image _buttonImg = null;
    [SerializeField] private TextMeshProUGUI _buttonTxt = null;
    [SerializeField] private Color _colorSelect = new Color(1, 1, 1, 1);
    [SerializeField] private Color _colorDeselect = new Color(1, 1, 1, 1);

    public void OnSelect(BaseEventData eventData)
    {
        _buttonTxt.color = _colorSelect;
        _buttonImg.color = _colorSelect;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        _buttonTxt.color = _colorDeselect;
        _buttonImg.color = _colorDeselect;
    }
}