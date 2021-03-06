﻿using Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    [SerializeField] private SettingsSO _settings;

    [Header("Main")]
    [SerializeField] private TextMeshProUGUI _titleTxt = null;
    [Space]
    [SerializeField] private TextMeshProUGUI _glucoseTxt = null;
    [SerializeField] private TextMeshProUGUI _insulinTxt = null;

    [Header("Side Bar")]
    [SerializeField] private Button _menuBtn = null;

    [Header("Food Selection")]
    [SerializeField] private Button _startBtn = null;
    [Space]
    [SerializeField] private ButtonFood[] _foodBtn = null;
    [Space]
    [SerializeField] private TextMeshProUGUI _IGValueTxt = null;

    [Header("Other")]
    [SerializeField] private ButtonMaster _animationButtonMaster = null;
    [SerializeField] private AnimationSideScreen _animationSideScreen = null;

    private StateRunningEvent _stateRunningEvent;

    private int _IGValue;

    private void Start()
    {
        _stateRunningEvent = new StateRunningEvent();
        _stateRunningEvent.igValue = _IGValue;

        // Side Bar
        _menuBtn.onClick.AddListener(() => OpenSideMenu(true));

        // Food Selection
        _startBtn.onClick.AddListener(StartSimulation);

        for (int i = 0; i < _foodBtn.Length; i++)
        {
            int index = i;

            _foodBtn[i].AddListener(() => SetIG(_settings.foodIGValues[index]));
            // _foodBtn[i].AddListener(() => SetIG(_foodBtn[index].IGValue));
        }

        _IGValue = _settings.defaultIGValue;
        _IGValueTxt.text = _IGValue.ToString();
    }

    private void OpenSideMenu(bool open)
    {
        if (open)
        {
            _animationSideScreen.SetFirstButton();
            _animationSideScreen.Show();
        }
        else
        {
            _animationSideScreen.Hide();
        }
    }

    public void UpdateValues(float glucoseValue, float insulinValue)
    {
        _glucoseTxt.text = glucoseValue.ToString("F2");
        _insulinTxt.text = insulinValue.ToString("F2");
    }

    private void SetIG(int IGValue)
    {
        _IGValue = IGValue;
        _IGValueTxt.text = _IGValue.ToString();
    }

    private void StartSimulation()
    {
        _animationButtonMaster.Hide();
        EventController.TriggerEvent(_stateRunningEvent);
    }

}