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
    bool canSwitch, canShoot;

    [SerializeField] private float _shootTimer, _shootCounter;
    [SerializeField] private float _switchTimer, _switchCounter;


    void Start()
    {
        _inputManager.GetInput.OnFireInput += TryShoot;
        _inputManager.GetInput.OnSwitchWeaponInput += SwitchWeapon;

        for (int i = 0; i < _weapons.Count; i++)
        {
            _weapons[i].Unlocked = true;
            _weapons[i].SetScriptableObject(_weapons[i].ScriptableObject);
            _weapons[i].InstantiateBullets(WeaponManager.PlayerBulletsLayerMask);
        }

        OnSwitchWeapon?.Invoke();
        OnUpdateAmmo?.Invoke(CurrentWeapon.AmmoLeft);
        CanSwitch();
        enabled = false;
    }

    private void Update()
    {
        _shootCounter -= Time.deltaTime;

        if (_inputManager.GetInput.FireInput && canShoot && !CurrentWeapon.ScriptableObject.SingleSpread && _shootCounter < 0)
        {
            CurrentWeapon.Shoot();
            _shootCounter = CurrentWeapon.ScriptableObject.FireRate;
        }
    }

    void TryShoot()
    {
        if(CurrentWeapon.ScriptableObject.SingleSpread)
            CurrentWeapon.Shoot();
    }

    void SwitchWeapon(float weaponOffset)
    {
        if (!canSwitch) return;

        enabled = false;
        canSwitch = false;
        canShoot = false;
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
        _shootCounter = 0;
        canSwitch = true;
        canShoot = true;
        enabled = true;
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
