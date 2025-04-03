using UnityEngine;

public class BigBoyAlien : AlienBase
{
    int Lives = 3;
    bool immortal = false;
    float counter = 0;
    readonly float timer = 3;
    Vector3 lastScale;

    private void Awake()
    {
        enabled = false;
    }

    protected override void Die()
    {
        if(immortal)
        {
            return;
        }

        Lives--;

        if(Lives < 0 )
        {

           base.Die();
        }
        else
        {
            Heal(1000f);
            enabled = true;
            immortal = true;
            lastScale = transform.localScale;
            counter = 0;
        }

    }

    private void Update()
    {
        counter += Time.deltaTime;

        transform.localScale = Vector3.Lerp(lastScale, lastScale*2, counter);

        if (counter > timer)
        {
            immortal = false;
            enabled = false;
        }
    }
}
