using UnityEngine;

public class AlienMovement : MonoBehaviour
{
    [SerializeField] Rigidbody _rb;
    [SerializeField] float _walkSpeed = 5;
    [SerializeField] Transform _destination;
    [SerializeField] Vector3 _walkDirection;
    [SerializeField] float _customGravity = -9.81f;
    private void Start()
    {
        if (!_destination) _destination = House.Instance.transform;
    }

    private void Update()
    {
        _walkDirection = (_destination.position - transform.position).normalized;
    }

    private void FixedUpdate()
    {
        Vector3 test = _walkDirection * _walkSpeed;
        _rb.linearVelocity = new Vector3(test.x, _customGravity, test.z);
    }
}
