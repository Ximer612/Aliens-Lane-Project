using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WeaponUISetter : MonoBehaviour
{
    [SerializeField] TMP_Text _weaponText;
    [SerializeField] TMP_Text _ammoText;
    [SerializeField] Image _weaponSprite;
    [SerializeField] PlayerWeapon _playerWeapon;

    private void Awake()
    {
        _playerWeapon.OnSwitchWeapon += SetWeapon;
        _playerWeapon.OnUpdateAmmo += SetAmmo;
    }

    void SetWeapon()
    {
        _weaponText.SetText(_playerWeapon.CurrentWeapon.ScriptableObject.Name);
        _weaponSprite.sprite = _playerWeapon.CurrentWeapon.ScriptableObject.HoldingWeaponSprite; // to do an animation
        _weaponSprite.rectTransform.localScale = Vector3.one;
        _weaponSprite.SetNativeSize();
        _weaponSprite.rectTransform.localScale *= 2;
    }

    void SetAmmo(int newAmmo)
    {
        _ammoText.SetText(newAmmo.ToString() + " - " + _playerWeapon.CurrentWeapon.ScriptableObject.MaxAmmo.ToString());
    }


}
