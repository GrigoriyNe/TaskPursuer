using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private InputManager _input;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _lookSpeed;
    [SerializeField] private Camera _camera;
    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _strayfeSpeed = 3f;

    [Header("Look")]
    [SerializeField] private float _horizontalTurnSesitivity = 3;
    [SerializeField] private float _verticalTurnSesitivity = 2;
    [SerializeField] private float _verticalMinAngle = -89;
    [SerializeField] private float _verticalMaxAngle = 89;
    
    [SerializeField] private float _jumpSpeed = 7;

    private Vector3 _verticalVelocity;
    private Vector2 _moveDirection;
    private Vector2 _lookDirection;
    private float _cameraAngle;
    private Transform _transform;
    private CharacterController _characterController;
    private float _gravityFactor = 2f;
    private bool _isJumpPress = false;

    private void OnEnable()
    {
        _input.Moved += SetMoveDirection;
        _input.Looked += Look;
        _input.Jumped += TryJump;
    }

    private void OnDisable()
    {
        _input.Moved -= SetMoveDirection;
        _input.Looked -= Look;
        _input.Jumped -= TryJump;
    }

    private void Awake()
    {
        _transform = transform;
        _characterController = GetComponent<CharacterController>();
        _cameraAngle = _camera.transform.localEulerAngles.x;
        _verticalVelocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void TryJump()
    {
        _isJumpPress = true;
    }

    private void SetMoveDirection(Vector2 moveDirection)
    {
        _moveDirection = moveDirection;
    }

    private void Look(Vector2 lookDirection)
    {
        if (_lookDirection != lookDirection)
            _lookDirection = lookDirection;

        if (_lookDirection != Vector2.zero)
        {
            _cameraAngle -= _lookDirection.y * _verticalTurnSesitivity;
            _cameraAngle = Mathf.Clamp(_cameraAngle, _verticalMinAngle, _verticalMaxAngle);
            _camera.transform.localEulerAngles = Vector3.right * _cameraAngle;

            _transform.Rotate(Vector3.up * _horizontalTurnSesitivity * _lookDirection.x);
        }
    }

    private void Movement()
    {
        Vector3 forward = Vector3.ProjectOnPlane(_camera.transform.forward, Vector3.up).normalized;
        Vector3 right = Vector3.ProjectOnPlane(_camera.transform.right, Vector3.up).normalized;
        Vector3 playerDirection;

        if (_characterController != null)
        {
            playerDirection = forward * _moveDirection.y * _speed + right * _moveDirection.x * _strayfeSpeed;

            if (_characterController.isGrounded)
            {
                if (_isJumpPress)
                {
                    _verticalVelocity = Vector3.up * _jumpSpeed;
                    _isJumpPress = false;
                }

                _characterController.Move((playerDirection + _verticalVelocity) * Time.fixedDeltaTime);
            }
            else
            {
                Vector3 horizontalVelocity = _characterController.velocity;
                horizontalVelocity.y = 0;
                _verticalVelocity += Physics.gravity * Time.fixedDeltaTime * _gravityFactor;
                _characterController.Move((horizontalVelocity + _verticalVelocity) * Time.fixedDeltaTime);
            }
        }
    }
}
