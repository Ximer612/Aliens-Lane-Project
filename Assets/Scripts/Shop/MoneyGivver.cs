using System.Diagnostics;
using System.Globalization;
using TMPro;
using UnityEngine;

public class MoneyGivver : MonoBehaviour
{

    public static MoneyGivver Instance;

    [SerializeField] TMP_Text _moneyText;
    [SerializeField] float _currentMoney = 1f;

    private void Awake()
    {
        Instance = this;
        //_moneyText.text = "{1.00}€";
        AddMoney(0);
    }

    public void AddMoney(float moneyAmount)
    {
        _currentMoney += moneyAmount;
        _moneyText.text = _currentMoney.ToString("C", new CultureInfo("it-IT")); ;
    }

    public bool RemoveMoney(float moneyAmount)
    {
        if (_currentMoney < moneyAmount)
           return false;

        _currentMoney -= moneyAmount;
        return true;
    }

}
