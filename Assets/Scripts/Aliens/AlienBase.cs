using UnityEngine;

public class AlienBase : Actor
{
    public float OnDieMoneyAmount = 0.5f;

    protected override void Die()
    {
        MoneyGivver.Instance.AddMoney(OnDieMoneyAmount);
        base.Die();
    }

    public void DeleteMe()
    {
        Destroy(gameObject);
    }
}
