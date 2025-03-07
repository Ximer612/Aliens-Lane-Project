using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Weapon : MonoBehaviour
{
    public event Action<int> OnReloaded;
    public UnityEvent OnShootBullet, OnBulletHit;
    [SerializeField] private WeaponScriptableObject _weaponScriptableObject;
    public WeaponScriptableObject ScriptableObject { get => _weaponScriptableObject; }
    [SerializeField] private Queue<Tuple<GameObject,Bullet>> _bulletsPool;
    public int AmmoLeft = 0;
    public int AmmoRecharges = 0;
    public bool Unlocked = false;
    [SerializeField] private bool _endlessAmmo = false;
    [SerializeField] private bool _reloading = false;

    public void SetScriptableObject(WeaponScriptableObject scriptableObject)
    {
        _weaponScriptableObject = scriptableObject;
        AmmoLeft = _weaponScriptableObject.MaxAmmo;
        _endlessAmmo = _weaponScriptableObject.EndlessAmmo;
    }


    private void Start()
    {
        AmmoLeft = _weaponScriptableObject.MaxAmmo;
        _endlessAmmo = _weaponScriptableObject.EndlessAmmo;

        _bulletsPool = new Queue< Tuple < GameObject,Bullet >> (_weaponScriptableObject.MaxAmmo);

        for (int i = 0; i < _weaponScriptableObject.MaxAmmo; i++)
        {
            GameObject bullet = Instantiate(_weaponScriptableObject.BulletPrefab,transform);
            //If it's a player then give it's a PlayerBullet else a EnemyBullet

            bullet.layer = gameObject.layer == 3 ? 9 : 10;

            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.SetBullet(_weaponScriptableObject.Damage, _weaponScriptableObject.RandomSpread, Physics2D.GetLayerCollisionMask(bullet.layer));
            Tuple<GameObject,Bullet> bulletTuple = new Tuple<GameObject,Bullet>(bullet,bulletScript);

            bulletScript.OnResetDelegate += () => { _bulletsPool.Enqueue(bulletTuple); };
            _bulletsPool.Enqueue(bulletTuple);
        }
    }

    public void Shoot()
    {
        if (_bulletsPool.Count < 1 || _reloading)
            return;

        AmmoLeft--;
        OnReloaded?.Invoke(AmmoLeft);

        Tuple<GameObject, Bullet> bullet = _bulletsPool.Dequeue();

        if(bullet.Item1 && bullet.Item2)
        {
            bullet.Item2.ShootBullet(Camera.main.transform);
        }

        if (AmmoLeft < 1)
        {
            if (!_reloading && (_endlessAmmo || AmmoRecharges > 0))
            {
                _reloading = true;
                Invoke(nameof(Reload), _weaponScriptableObject.ReloadTime);
            }

            return;
        }
    }

    public void Reload()
    {
        AmmoLeft = _weaponScriptableObject.MaxAmmo;
        _reloading = false;
        OnReloaded?.Invoke(AmmoLeft);
    }
}
