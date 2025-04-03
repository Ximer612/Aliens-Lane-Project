using UnityEngine;

public class Buyable : MonoBehaviour
{
    public WeaponScriptableObject toBuyWeapon;

    public void OnBuy()
    {
        gameObject.SetActive(false);
    }
}
