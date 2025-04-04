using UnityEngine;
using UnityEngine.Events;

public class Actor : MonoBehaviour
{
    public float MaxHP = 200;
    [SerializeField] private float _hp = 100;
    public float HP { get => _hp; set => _hp = Mathf.Min(value, MaxHP); }
    public bool Alive = true;
    public UnityEvent OnDie, OnHeal, OnRespawn;
    //public PlayerWeapon CurrentWeapon;

    public virtual void Heal(float value)
    {
        if(HP < 1)
        {
            HP = value;
            Respawn();
            return;
        }

        HP += value;
        OnHeal.Invoke();
    }
    public virtual void Respawn()
    {
       OnRespawn.Invoke();
    }

    public virtual void Damage(float damage, GameObject vandal)
    {
        HP -= damage;
        if (HP < 1)
        {
            Alive = false;
            Die();
        }
    }

    protected virtual void Die()
    {
        OnDie.Invoke();
    }
}
