using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static LayerMask PlayerBulletsLayerMask, EnemyBulletsLayerMask;

    [SerializeField] PlayerWeapon _playerWeapon;
    [SerializeField] GameObject _weaponMaster;
    [SerializeField] LayerMask _playerBulletsLayerMask, _enemyBulletsLayerMask;

    private void Awake()
    {
        PlayerBulletsLayerMask = _playerBulletsLayerMask;
        EnemyBulletsLayerMask = _enemyBulletsLayerMask;
    }

    public void GiveWeaponToPlayer(WeaponScriptableObject InWeapon)
    {
        GameObject newWeapon = Instantiate(_weaponMaster, Vector3.one, Quaternion.identity);
        newWeapon.name = InWeapon.Name;
        Weapon weaponScript = newWeapon.GetComponent<Weapon>();
        weaponScript.SetScriptableObject(InWeapon);
        weaponScript.InstantiateBullets(_playerBulletsLayerMask);

        _playerWeapon.AddWeapon(newWeapon, newWeapon.GetComponent<Weapon>());
    }

}
