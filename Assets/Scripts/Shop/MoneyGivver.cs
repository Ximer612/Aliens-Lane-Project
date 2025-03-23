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
        _moneyText.text = string.Format("{0:#.00}€", _currentMoney);
    }
    

}
