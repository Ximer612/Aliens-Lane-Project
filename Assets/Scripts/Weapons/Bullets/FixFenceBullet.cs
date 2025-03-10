using UnityEditor;
using UnityEngine;

public class FixFenceBullet : AreaBullet
{
    public override void ShootBullet(Transform muzzle)
    {
        gameObject.SetActive(true);

        transform.rotation = muzzle.rotation;

        Collider[] hittedGameObjects = Physics.OverlapBox(transform.position, _areaHalfExtension, transform.rotation, _layerMask);

        for (int i = 0; i < hittedGameObjects.Length; i++)
        {
            Actor hittedActor = hittedGameObjects[i].GetComponent<Actor>();

            if (!hittedActor)
            {
                continue;
            }

            hittedActor.Damage(hittedActor.gameObject.layer == 11 ? 1 : _damage, gameObject);
            OnHit();
        }

        ResetBullet();

        //Invoke(nameof(ResetBullet),3);
        //activate update since it's an area so maybe takes time damage
    }

}
