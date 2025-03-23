using UnityEditor;
using UnityEngine;

public class AreaBullet : Bullet
{
    [SerializeField] protected Vector3 _areaHalfExtension;

    public override void ShootBullet(Transform muzzle)
    {
        base.ShootBullet(muzzle);
        transform.rotation = muzzle.rotation;

        Collider[] hittedGameObjects = Physics.OverlapBox(transform.position, _areaHalfExtension, transform.rotation, _layerMask);

        for (int i = 0; i < hittedGameObjects.Length; i++)
        {
            Actor hittedActor = hittedGameObjects[i].GetComponent<Actor>();

            if (!hittedActor)
            {
                continue;
            }

            hittedActor.Damage(_damage, gameObject);
            OnHit();
        }

        ResetBullet();

        //Invoke(nameof(ResetBullet),3);
        //activate update since it's an area so maybe takes time damage
    }
}
