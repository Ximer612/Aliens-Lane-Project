using UnityEngine;

public class RaycastBullet : Bullet
{
    [SerializeField] private float _maxDistance = 200f;
    public override void ShootBullet(Transform muzzle)
    {
        base.ShootBullet(muzzle);
        transform.SetPositionAndRotation(muzzle.position, muzzle.rotation);

        RaycastHit hittedGameObject;
        if(Physics.Raycast(transform.position, transform.forward, out hittedGameObject, _maxDistance, _layerMask))
        {
            OnHit();  // pass transform/vector3 and rotation to create a decal, use normal hitted?
            Actor hittedActor = hittedGameObject.collider.GetComponent<Actor>();

            if (hittedActor)
            {
                hittedActor.Damage(_damage, gameObject);
            }

        }

        //multiple trace removed

        //RaycastHit[] hittedGameObjects = Physics.RaycastAll(transform.position, transform.forward, _maxDistance, _layerMask);

        //int maxThrough = 3;

        //for (int i = 0; i < hittedGameObjects.Length; i++)
        //{
        //    maxThrough--;
        //    if(maxThrough <= 0)
        //    {
        //        break;
        //    }

        //    OnHit(); // pass transform/vector3 and rotation


        //    Actor hittedActor = hittedGameObjects[i].collider.GetComponent<Actor>();

        //    if (!hittedActor)
        //    {
        //        continue;
        //    }

        //    hittedActor.Damage(_damage);
        //}

        ResetBullet();
    }

}
