using UnityEngine;
using UnityEngine.AI;

public class HitFenceState : EnemyState
{
    public GameObject toDestroyFence;
    [SerializeField] Fence _toDestroyFenceScript;
    [SerializeField] private float _shootTimer, _shootCounter;
    [SerializeField] EnemyState _continueWalkingStaste;

    public override void OnEnter(EnemyStateMachine InFSM, NavMeshAgent InAgent)
    {
        base.OnEnter(InFSM, InAgent);

        _agent.transform.rotation = Quaternion.Euler(toDestroyFence.transform.position - _agent.transform.position);

        _agent.isStopped = true;

        _toDestroyFenceScript = toDestroyFence.GetComponent<Fence>();
        _toDestroyFenceScript.OnDie.AddListener(ContinueWalking);
        //GameObject newWeapon = Instantiate(_weaponMaster, Vector3.one, Quaternion.identity);
        //newWeapon.name = InWeapon.Name;
        //Weapon weaponScript = newWeapon.GetComponent<Weapon>();
        //weaponScript.SetScriptableObject(InWeapon);
        //weaponScript.InstantiateBullets(_playerBulletsLayerMask);

        //_playerWeapon.AddWeapon(newWeapon, newWeapon.GetComponent<Weapon>());
    }

    void ContinueWalking()
    {
        _fsm.ChangeState(_continueWalkingStaste);
    }

    public override void OnExit()
    {
        _agent.isStopped = false;
        _toDestroyFenceScript.OnDie.RemoveListener(ContinueWalking);
    }

    public override void Tick()
    {
        //attack fence by time
        _fsm._myWeapon.Shoot();

    }
}
