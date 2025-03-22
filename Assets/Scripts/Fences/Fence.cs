using System;
using UnityEngine;

public class Fence : Actor
{
    public Action OnChangeState;

    private void Start()
    {
        OnRespawn.AddListener(CallOnChangeState);
    }

    public override void Damage(float damage, GameObject vandal)
    {
        if(vandal && vandal.layer == 9) //PlayerBullet layer
        {
            if (vandal.CompareTag("FixBullet"))
            {
                Heal(damage);
                print("HEALED! " + vandal.name);
            }
        }
        else
        {
            base.Damage(damage, vandal);

        }
    }

    protected override void Die()
    {
        gameObject.SetActive(false);
        base.Die();
        CallOnChangeState();
    }

    void CallOnChangeState()
    {
        OnChangeState?.Invoke();
    }

    [ContextMenu("Kill")]
    void Kill()
    {
        Damage(999, null);
    }

    [ContextMenu("Respa")]
    void Respa()
    {
        gameObject.SetActive(true);
        Heal(999);
    }
}
