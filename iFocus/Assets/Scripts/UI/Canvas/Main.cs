using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
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
    
    [Header("Animation")]
    [SerializeField] private Animation _animationFoodSelection = null;
    

    private int _IGValue;

    private void Start()
    {
        // Side Bar
        _menuBtn.onClick.AddListener(() => OpenSideMenu(true));

        // Food Selection
        _startBtn.onClick.AddListener(StartSimulation);

        for (int i = 0; i < _foodBtn.Length; i++)
        {
            _foodBtn[i].AddListener(() => SetIG(_foodBtn[i].IGValue));
        }
    }

    private void OpenSideMenu(bool open)
    {
        Debug.Log($"<b> OPEN SIDE MENU: {open} </b>");
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
        Debug.Log($"<b> START! </b>");
    }
    
   

}