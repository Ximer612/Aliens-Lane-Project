using UnityEngine;

public class AlienBase : Actor
{
    public float OnDieMoneyAmount = 0.5f;

    protected override void Die()
    {
        base.Die();
        MoneyGivver.Instance.AddMoney(OnDieMoneyAmount);
    }
}
