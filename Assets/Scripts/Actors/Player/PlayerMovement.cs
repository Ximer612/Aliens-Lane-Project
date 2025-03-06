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
        //Debug.DrawRay(transform.position, Vector3.down * (_playerHeight * 0.5f + _groundCheckOffset),Color.magenta);
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

        if (_readyToJump  && flatVel.sqrMagnitude > _moveSpeed * _moveSpeed)
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
        if (_readyToJump && Physics.Raycast(transform.position,Vector3.down,out slopeHit,_playerHeight * 0.5f + slopeHitOffset))
        {
            Debug.DrawRay(transform.position, Vector3.down * (_playerHeight * 0.5f + slopeHitOffset), Color.green);
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        else
        {
           Debug.DrawRay(transform.position, Vector3.down * (_playerHeight * 0.5f + slopeHitOffset), Color.gray);
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(_moveDirection, slopeHit.normal).normalized;
    }
}
