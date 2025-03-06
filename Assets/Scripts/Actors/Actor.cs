using UnityEngine;

public class Actor : MonoBehaviour
{
    public float HP = 100;
    public bool Alive = true;

    public void Damage(float damage)
    {
        HP -= damage;
        if (HP < 0)
        {
            Alive = false;
            gameObject.SetActive(false);
        }
    }
}
