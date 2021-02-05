using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonFood : MonoBehaviour
{
    [SerializeField] private Button _button = null;

    public void AddListener(UnityAction action)
    {
        _button.onClick.AddListener(action);
    }

}