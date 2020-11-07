using Pyros;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleUIPanel : MonoBehaviour
{
    [SerializeField] private Movement _openMovement;
    [SerializeField] private Movement _closeMovement;
    [SerializeField] private bool _isOpen;

    public void SwitchOnOff()
    {
        if (_isOpen)
            Close();
        else
            Open();
    }

    public void Open()
    {
        if (_isOpen)
            return;
        _openMovement?.StartMove();
        _isOpen = true;
    }

    public void Close()
    {
        if (!_isOpen)
            return;

        _closeMovement?.StartMove();
        _isOpen = false;
    }
}
