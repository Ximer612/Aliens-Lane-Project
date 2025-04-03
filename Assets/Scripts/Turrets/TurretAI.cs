using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAI : MonoBehaviour
{
    [SerializeField] List<Transform> enemies;
    [SerializeField] Transform body,head,shootPoint;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] float bodyYawSpeed, headPitchSpeed, shootRange,minXHead,maxXHead;
    [SerializeField] int currentEnemy;
    [SerializeField] bool foundedEnemy;
    [SerializeField] Weapon weapon;

    private void Start()
    {
        weapon.SetScriptableObject(weapon.ScriptableObject);
        weapon.InstantiateBullets(WeaponManager.PlayerBulletsLayerMask);
    }

    void Update()
    {
        FindNearestEnemy();

        if (foundedEnemy && enemies[currentEnemy])
        {
            RotateBody();
            RotateHead();

            lineRenderer.SetPosition(0, shootPoint.position);
            lineRenderer.SetPosition(1, shootPoint.position + shootPoint.forward * shootRange);

            Shoot();
        }
    }

    private void FindNearestEnemy()
    {
        float lastEnemyDistance=shootRange*shootRange;
        foundedEnemy = false;

        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies.Count <= i) break;

            Transform indexEnemy = enemies[i];
            RaycastHit hitInfo;

            bool hitted = Physics.Raycast(shootPoint.transform.position, (indexEnemy.position - shootPoint.position).normalized, out hitInfo);

            if(!hitted)
            {
                continue;
            }

            if(!hitInfo.collider.CompareTag("Enemy"))
            {
                continue;
            }

            float distance = (indexEnemy.position - body.position).sqrMagnitude;
            if (distance < lastEnemyDistance)
            {
                Vector3 targetDir = (enemies[currentEnemy].transform.position - body.position).normalized;
                Quaternion lookDir = Quaternion.LookRotation(targetDir);

                Vector3 lookDirVector = lookDir.eulerAngles;

                if (lookDirVector.x < maxXHead && lookDirVector.x > minXHead)
                {
                    lastEnemyDistance = distance;
                    currentEnemy = i;
                    foundedEnemy = true;

                }
            }
        }
    }

    [ContextMenu("Rotate Head")]
    private void RotateHead()
    {
        Vector3 targetDir = (enemies[currentEnemy].transform.position - head.position).normalized;
        Quaternion lookDir = Quaternion.LookRotation(targetDir);

        Vector3 lookDirVector = lookDir.eulerAngles;

        Vector3 newRotation = new Vector3(lookDirVector.x, head.rotation.eulerAngles.y, head.rotation.eulerAngles.z);

        head.rotation = Quaternion.Slerp(head.rotation, Quaternion.Euler(newRotation), Time.deltaTime * headPitchSpeed);
    }

    void RotateBody()
    {
        Vector2 enemyPos = new Vector2(enemies[currentEnemy].position.x, enemies[currentEnemy].position.z);
        Vector2 bodyPos = new Vector2(body.position.x, body.position.z);

        Vector2 direction = (enemyPos - bodyPos).normalized;

        float angleDirection = ((float)Mathf.Atan2(direction.y, direction.x));

        Vector3 lookAtEnemy = new Vector3(Mathf.Cos(angleDirection),0, Mathf.Sin(angleDirection));

        if ((body.transform.forward - lookAtEnemy).sqrMagnitude > 0.03f)
        {

            body.transform.forward = Vector3.Slerp(body.transform.forward, lookAtEnemy, Time.deltaTime * bodyYawSpeed);
        }
        else body.transform.forward = lookAtEnemy;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            enemies.Add(other.transform);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemies.Remove(other.transform);
        }
    }

    void Shoot()
    {
        weapon.Shoot(shootPoint);
    }
}
