using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static LayerMask PlayerBulletsLayerMask, EnemyBulletsLayerMask;

    [SerializeField] PlayerWeapon _playerWeapon;
    [SerializeField] MoneyGivver _moneyGivver;
    [SerializeField] GameObject _weaponMaster;
    [SerializeField] Turret _turretTickets;
    [SerializeField] LayerMask _playerBulletsLayerMask, _enemyBulletsLayerMask;

    private void Awake()
    {
        PlayerBulletsLayerMask = _playerBulletsLayerMask;
        EnemyBulletsLayerMask = _enemyBulletsLayerMask;
    }

    public void GivePlayerATurretTicket(Buyable InWeapon)
    {
        //not enough money
        if (!_moneyGivver.RemoveMoney(InWeapon.toBuyWeapon.ShopPrice))
        {
            NoMoney();
            return;
        }

        InWeapon.OnBuy();
        _turretTickets.AddTicket();
    }

    public void GiveWeaponToPlayer(Buyable InWeapon)
    {
        //not enough money
        if (!_moneyGivver.RemoveMoney(InWeapon.toBuyWeapon.ShopPrice))
        {
            NoMoney();
            return;
        }

        InWeapon.OnBuy();

        GameObject newWeapon = Instantiate(_weaponMaster, Vector3.one, Quaternion.identity);
        newWeapon.name = InWeapon.toBuyWeapon.Name;
        Weapon weaponScript = newWeapon.GetComponent<Weapon>();
        weaponScript.SetScriptableObject(InWeapon.toBuyWeapon);
        weaponScript.InstantiateBullets(_playerBulletsLayerMask);

        _playerWeapon.AddWeapon(newWeapon, newWeapon.GetComponent<Weapon>());
    }

    //public void GiveWeaponToActor(Actor InActor, WeaponScriptableObject InWeapon)
    //{
    //    GameObject newWeapon = Instantiate(_weaponMaster, Vector3.one, Quaternion.identity);
    //    newWeapon.name = InWeapon.Name;
    //    Weapon weaponScript = newWeapon.GetComponent<Weapon>();
    //    weaponScript.SetScriptableObject(InWeapon);
    //    weaponScript.InstantiateBullets(_playerBulletsLayerMask);

    //    _playerWeapon.AddWeapon(newWeapon, newWeapon.GetComponent<Weapon>());
    //}

    void NoMoney()
    {
        print("not enough money!");
    }

}
