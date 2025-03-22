using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof(NavMeshAgent))]
public class NormalAlien : Actor
{
    [SerializeField] Weapon _weapon;
    [SerializeField] EnemyStateMachine FSM;

    private void Awake()
    {
        _weapon.SetScriptableObject(_weapon.ScriptableObject);
        _weapon.InstantiateBullets(WeaponManager.EnemyBulletsLayerMask);
        FSM._myWeapon = _weapon;
    }

    public virtual void RefreshDestination()
    {
        FSM.UpdateDestination();
    }

}
