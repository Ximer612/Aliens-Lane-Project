using System.ComponentModel;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerCam;
    [SerializeField] private Transform orientation;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private InputManager _inputManager;


    [Header("Movement Handling")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _sprintSpeed;

    [Header("Jump Handling")]
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpCooldown;
    [SerializeField] private float _extraGravity;
    [SerializeField] private float _airMultiplier;
    [SerializeField] private bool _readyToJump = true;

    [Header("Ground Handling")]
    [SerializeField] private float _groundDrag;
    [SerializeField] private float _groundCheckOffset;
    [SerializeField] private float _playerHeight;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private bool _isGrounded;

    [Header("Slope Handling")]
    public bool _isOnSlope;
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    public float slopeHitOffset;

    [Header("Debug")]
    [SerializeField] private Vector2 _inputMovementDirection;
    [SerializeField] private Vector3 _moveDirection;
    [SerializeField] private bool _inputJumping;
    [SerializeField] private MovementState _movementState;

    private enum MovementState
    {
        Walking,Sprinting,Air,LAST
    }

    void Start()
    {
        _rigidbody.freezeRotation = true;

    }

    // Update is called once per frame
    void Update()
    {
        GroundCheck();
        ManageInput();
        StateHandler();
        SpeedControl();
        HandleDrag();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void GroundCheck()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, _playerHeight * 0.5f + _groundCheckOffset, _groundLayer);
    }

    private void HandleDrag()
    {
        if( _isGrounded ) {
            _rigidbody.linearDamping = _groundDrag;
        }
        else
        {
            _rigidbody.linearDamping = 0;
        }
    }


    private void ManageInput()
    {
        _inputMovementDirection = new Vector2(_inputManager.GetInput.MovementInput.x, _inputManager.GetInput.MovementInput.y);
        _inputJumping = _inputManager.GetInput.JumpInput;

        if(_inputJumping && _readyToJump && _isGrounded)
        {
            Jump();
            Invoke(nameof(ResetJump), _jumpCooldown);
        }
    }

    private void Movement()
    {
        //walk forward
        _moveDirection = orientation.forward * _inputMovementDirection.y + orientation.right * _inputMovementDirection.x;

        _isOnSlope = OnSlope();

        if (_isOnSlope)
        {
            _rigidbody.AddForce(GetSlopeMoveDirection() * _moveSpeed * 20f, ForceMode.Force);

            if(_rigidbody.linearVelocity.y > 0)
            {
                _rigidbody.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }

        if(_isGrounded)
        {
            _rigidbody.AddForce(_moveDirection.normalized * _moveSpeed * 10f, ForceMode.Force);
        }
        else
        {
            _rigidbody.AddForce(_moveDirection.normalized * _moveSpeed * 10f * _airMultiplier, ForceMode.Force);

            float tempExtraGravity = _extraGravity;

            if (_rigidbody.linearVelocity.y < 0)
            {
                tempExtraGravity *= 35;
            }

            _rigidbody.AddForce(Vector3.down * Time.deltaTime * tempExtraGravity);
        }

        _rigidbody.useGravity = !_isOnSlope;
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(_rigidbody.linearVelocity.x, 0f, _rigidbody.linearVelocity.z);

        if (flatVel.sqrMagnitude > _moveSpeed * _moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * _moveSpeed;
            _rigidbody.linearVelocity = new Vector3(limitedVel.x, _rigidbody.linearVelocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        _readyToJump = false;

        _rigidbody.linearVelocity = new Vector3(_rigidbody.linearVelocity.x, 0f, _rigidbody.linearVelocity.z);
        _rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        _readyToJump = true;
    }

    private void StateHandler()
    {
        if (_isGrounded && _inputManager.GetInput.SprintInput)
        {
            _movementState = MovementState.Sprinting;
            _moveSpeed = _sprintSpeed;
        }
        else if (_isGrounded)
        {
            _movementState = MovementState.Walking;
            _moveSpeed = _walkSpeed;
        }
        else
        {
            _movementState = MovementState.Air;
        }
    }

    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position,Vector3.down,out slopeHit,_playerHeight * 0.5f + slopeHitOffset))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(_moveDirection, slopeHit.normal).normalized;
    }

        //private void CounterMovement(Vector2 direction, Vector2 mag)
        //{
        //    if (!grounded || _inputJumping) return;

        //    //Slow down sliding
        //    //if (crouching)
        //    //{
        //    //    _rigidbody.AddForce(moveSpeed * Time.deltaTime * -_rigidbody.linearVelocity.normalized * slideCounterMovement);
        //    //    return;
        //    //}

        //    //Counter movement
        //    if (Mathf.Abs(mag.x) > threshold && Mathf.Abs(direction.x) < 0.05f || (mag.x < -threshold && direction.x > 0) || (mag.x > threshold && direction.x < 0))
        //    {
        //        _rigidbody.AddForce(moveSpeed * orientation.transform.right * Time.deltaTime * -mag.x * counterMovement);
        //    }
        //    if (Mathf.Abs(mag.y) > threshold && Mathf.Abs(direction.y) < 0.05f || (mag.y < -threshold && direction.y > 0) || (mag.y > threshold && direction.y < 0))
        //    {
        //        _rigidbody.AddForce(moveSpeed * orientation.transform.forward * Time.deltaTime * -mag.y * counterMovement);
        //    }

        //    //Limit diagonal running. This will also cause a full stop if sliding fast and un-crouching, so not optimal.
        //    if (Mathf.Sqrt((Mathf.Pow(_rigidbody.linearVelocity.x, 2) + Mathf.Pow(_rigidbody.linearVelocity.z, 2))) > maxSpeed)
        //    {
        //        float fallspeed = _rigidbody.linearVelocity.y;
        //        Vector3 n = _rigidbody.linearVelocity.normalized * maxSpeed;
        //        _rigidbody.linearVelocity = new Vector3(n.x, fallspeed, n.z);
        //    }
        //}

        //private bool IsFloor(Vector3 v)
        //{
        //    float angle = Vector3.Angle(Vector3.up, v);
        //    return angle < maxSlopeAngle;
        //}

        //private bool cancellingGrounded;

        ///// <summary>
        ///// Handle ground detection
        ///// </summary>
        //private void OnCollisionStay(Collision other)
        //{
        //    //Make sure we are only checking for walkable layers
        //    int layer = other.gameObject.layer;
        //    if (whatIsGround != (whatIsGround | (1 << layer))) return;

        //    //Iterate through every collision in a physics update
        //    for (int i = 0; i < other.contactCount; i++)
        //    {
        //        Vector3 normal = other.contacts[i].normal;
        //        //FLOOR
        //        if (IsFloor(normal))
        //        {
        //            grounded = true;
        //            cancellingGrounded = false;
        //            normalVector = normal;
        //            CancelInvoke(nameof(StopGrounded));
        //        }
        //    }

        //    //Invoke ground/wall cancel, since we can't check normals with CollisionExit
        //    float delay = 3f;
        //    if (!cancellingGrounded)
        //    {
        //        cancellingGrounded = true;
        //        Invoke(nameof(StopGrounded), Time.deltaTime * delay);
        //    }
        //}
}
