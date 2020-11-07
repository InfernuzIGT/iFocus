using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LocalizationUtility : MonoBehaviour
{
    [SerializeField] private Locale _currentLocale;

    [Header("Example")]
    public TextMeshProUGUI titleTxt;
    public AudioSource audioSource;

    private AsyncOperationHandle m_InitializeOperation;
    private List<Locale> _listLocales;
    private int _index;

    private void Start()
    {
        m_InitializeOperation = LocalizationSettings.SelectedLocaleAsync;

        if (m_InitializeOperation.IsDone)
        {
            InitializeCompleted(m_InitializeOperation);
        }
        else
        {
            m_InitializeOperation.Completed += InitializeCompleted;
        }
    }

    private void InitializeCompleted(AsyncOperationHandle obj)
    {
        _listLocales = new List<Locale>();
        _listLocales.AddRange(LocalizationSettings.AvailableLocales.Locales);

        _index = 0;

        UpdateLocale(_listLocales[_index]);
    }

    private void UpdateLocale(Locale locale)
    {
        LocalizationSettings.SelectedLocale = locale;

        _currentLocale = locale;
        titleTxt.text = locale.name;

        Debug.Log($"Update Locale: {locale.name} [{locale.Identifier.Code}]");
    }

    public void Move(bool isIncrement)
    {
        if (isIncrement)
        {
            _index = _index < _listLocales.Count - 1 ? _index + 1 : 0;
        }
        else
        {
            _index = _index > 0 ? _index - 1 : _listLocales.Count - 1;
        }

        UpdateLocale(_listLocales[_index]);
    }
    
    public void Play()
    {
        audioSource.Play();
    }

}