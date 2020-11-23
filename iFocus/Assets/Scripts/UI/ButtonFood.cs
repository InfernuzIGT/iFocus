using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonFood : MonoBehaviour
{
    [SerializeField] private int _IGValue = 0;
    [Space]
    [SerializeField] private Button _button = null;

    public int IGValue { get { return _IGValue; } }

    public void AddListener(UnityAction action)
    {
        _button.onClick.AddListener(action);
    }

}