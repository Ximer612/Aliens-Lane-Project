using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Action OnHitDelegate, OnResetDelegate;
    [SerializeField] protected LayerMask _layerMask;
    [SerializeField] protected int _damage;
    [SerializeField] protected float _spread;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void SetBullet(int inDamage, float inRandomSpread, LayerMask inLayerMask)
    {
        _damage = inDamage;
        _spread = inRandomSpread;
        _layerMask = inLayerMask;
    }

    public void OnHit()
    {
        OnHitDelegate?.Invoke();
    }

    public void ResetBullet()
    {
        OnResetDelegate?.Invoke();
        gameObject.SetActive(false);
    }

    public virtual void ShootBullet(Transform muzzle)
    {
        gameObject.SetActive(true);
    }

}
