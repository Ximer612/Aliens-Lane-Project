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
    [SerializeField] private Transform _weaponsHolder;
    bool canSwitch = true;

    void Start()
    {
        _inputManager.GetInput.OnFireInput += TryShoot;
        _inputManager.GetInput.OnSwitchWeaponInput += SwitchWeapon;

        for (int i = 0; i < _weapons.Count; i++)
        {
            _weapons[i].Unlocked = true;
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

        CurrentWeapon.OnReloaded -= OnUpdateAmmo;

        do
        {
            _currentWeaponIndex = (_currentWeaponIndex + sign) % _weapons.Count;
            _currentWeaponIndex = _currentWeaponIndex < 0 ? _weapons.Count - 1 : _currentWeaponIndex;
        } while (!CurrentWeapon.Unlocked); //check on remaning ammo?


        OnSwitchWeapon?.Invoke();
        CurrentWeapon.OnReloaded += OnUpdateAmmo;
        OnUpdateAmmo?.Invoke(CurrentWeapon.AmmoLeft);
        Invoke(nameof(CanSwitch), 0.5f);
    }

    void SetWeapon(int weaponIndex)
    {
        CurrentWeapon.OnReloaded -= OnUpdateAmmo;

        _currentWeaponIndex = weaponIndex;

        OnSwitchWeapon?.Invoke();
        CurrentWeapon.OnReloaded += OnUpdateAmmo;
        OnUpdateAmmo?.Invoke(CurrentWeapon.AmmoLeft);
        Invoke(nameof(CanSwitch), 0.5f);
    }


    void CanSwitch()
    {
        canSwitch = true;
    }

    public void AddWeapon(GameObject inNewWeapon, Weapon inNewWeaponScript)
    {
        if(!inNewWeaponScript)
        {
            return;
        }

        for (int i = 0; i < _weapons.Count; i++)
        {
            if (_weapons[i].ScriptableObject == inNewWeaponScript.ScriptableObject)
            {
                Destroy(inNewWeapon);
                return;
            }
        }

        _weapons.Add(inNewWeaponScript);
        inNewWeapon.transform.SetParent(_weaponsHolder);
        inNewWeapon.transform.localPosition = Vector3.zero;
        inNewWeaponScript.Unlocked = true;

        SetWeapon(_weapons.Count - 1);
    }
}
