using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : MonoBehaviour
{
    [SerializeField] public Weapon _myWeapon;
    [SerializeField] EnemyState _currentState;
    [SerializeField] NavMeshAgent _agent;
    [SerializeField] public Vector3 _destinationTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(_currentState == null)
        {
            enabled = false;
            return;
        }

        _currentState.OnEnter(this, _agent);
    }

    // Update is called once per frame
    void Update()
    {
        _currentState.Tick();
    }

    public void ChangeState(EnemyState InNewState)
    {
        _currentState.OnExit();
        _currentState = InNewState;
        InNewState.OnEnter(this, _agent);
    }

    public void UpdateDestination()
    {
        _agent.SetDestination(_destinationTransform);
    }
}
