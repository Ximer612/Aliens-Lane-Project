using UnityEngine;

public class RaycastBullet : Bullet
{
    [SerializeField] private float _maxDistance = 200f;
    public override void ShootBullet(Transform muzzle)
    {
        base.ShootBullet(muzzle);
        transform.SetPositionAndRotation(muzzle.position, muzzle.rotation);

        RaycastHit[] hittedGameObjects = Physics.RaycastAll(transform.position, transform.forward, _maxDistance, _layerMask);

        for (int i = 0; i < hittedGameObjects.Length; i++)
        {
            Actor hittedActor = hittedGameObjects[i].collider.GetComponent<Actor>();

            if (!hittedActor)
            {
                continue;
            }

            hittedActor.Damage(_damage);
            OnHit();
        }

        print("RAYCASTED BUULT");
        ResetBullet();
    }

}
