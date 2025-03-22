using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyState : MonoBehaviour
{
    protected EnemyStateMachine _fsm;
    protected NavMeshAgent _agent;

    public virtual void OnEnter(EnemyStateMachine InFSM, NavMeshAgent InAgent)
    {
        _fsm = InFSM;
        _agent = InAgent;
    }
    public abstract void OnExit();
    public abstract void Tick();
}
