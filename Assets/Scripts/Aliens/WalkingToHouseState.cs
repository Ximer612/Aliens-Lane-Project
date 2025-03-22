using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class WalkingToHouseState : EnemyState
{
    [SerializeField] float _walkSpeed;
    [SerializeField] HitFenceState _hitFencesState;
    [SerializeField] Vector3 _halfBoxFenceCheckExtent;
    [SerializeField] LayerMask _layerMask;

    public override void OnEnter(EnemyStateMachine InFSM, NavMeshAgent InAgent)
    {
        base.OnEnter(InFSM, InAgent);

        InFSM._destinationTransform = House.Instance.transform.position;

        _agent.SetDestination(InFSM._destinationTransform);
        _agent.speed = _walkSpeed;
        _agent.acceleration = (_agent.speed * 3);
        _agent.autoRepath = true;
    }

    public override void OnExit()
    {

    }

    public override void Tick()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, _halfBoxFenceCheckExtent,transform.rotation, _layerMask);

        for (int i = 0; i < colliders.Length; i++)
        {
            _hitFencesState.toDestroyFence = colliders[i].gameObject;
            _fsm.ChangeState(_hitFencesState);
            return;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, _halfBoxFenceCheckExtent * 2f);
    }

}
