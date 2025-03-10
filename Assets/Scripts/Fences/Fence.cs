using UnityEngine;

public class Fence : Actor
{
    public override void Damage(float damage, GameObject vandal)
    {
        if(vandal.layer == 9) //PlayerBullet layer
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
}
