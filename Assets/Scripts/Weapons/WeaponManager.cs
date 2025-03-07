using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] PlayerWeapon _playerWeapon;
    [SerializeField] GameObject _weaponMaster;

    public void GiveWeaponToPlayer(WeaponScriptableObject InWeapon)
    {
        GameObject newWeapon = Instantiate(_weaponMaster, Vector3.one, Quaternion.identity);
        newWeapon.name = InWeapon.Name;
        Weapon weaponScript = newWeapon.GetComponent<Weapon>();
        weaponScript.SetScriptableObject(InWeapon);

        _playerWeapon.AddWeapon(newWeapon, newWeapon.GetComponent<Weapon>());
    }

}
