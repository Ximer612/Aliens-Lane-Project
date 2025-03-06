using UnityEngine;
using System.Collections.Generic;
using System;

public class PlayerWeapon : MonoBehaviour
{
    public Action OnSwitchWeapon;
    public Action<int> OnUpdateAmmo;
    public Weapon CurrentWeapon { get { return _weapons[_currentWeaponIndex]; } }

    [SerializeField] private List<Weapon> _weapons;
    [SerializeField] private int _currentWeaponIndex;
    [SerializeField] private InputManager _inputManager;
    bool canSwitch = true;

    void Start()
    {
        _inputManager.GetInput.OnFireInput += TryShoot;
        _inputManager.GetInput.OnSwitchWeaponInput += SwitchWeapon;

        for (int i = 0; i < _weapons.Count; i++)
        {
            _weapons[i].Unlocked = true;
            _weapons[i].OnReload += OnUpdateAmmo;
        }

        OnSwitchWeapon?.Invoke();
        OnUpdateAmmo?.Invoke(CurrentWeapon.AmmoLeft);
    }

    void TryShoot()
    {
        CurrentWeapon.Shoot();
    }

    void SwitchWeapon(float weaponOffset)
    {
        if (!canSwitch) return;

        canSwitch = false;
        int sign = weaponOffset > 0 ? 1 : -1;

        do
        {
            _currentWeaponIndex = (_currentWeaponIndex + sign) % _weapons.Count;
            _currentWeaponIndex = _currentWeaponIndex < 0 ? _weapons.Count-1 : _currentWeaponIndex;
            print(_currentWeaponIndex);
        } while (!CurrentWeapon.Unlocked);

        OnSwitchWeapon?.Invoke();
        OnUpdateAmmo?.Invoke(CurrentWeapon.AmmoLeft);
        Invoke(nameof(CanSwitch), 1f);
    }

    void CanSwitch()
    {
        canSwitch = true;
    }
}
