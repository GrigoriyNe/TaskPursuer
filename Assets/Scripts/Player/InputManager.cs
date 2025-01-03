using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput _input;
    private Vector2 _moveDirection;
    private Vector2 _lookDirection;
    private bool _isJump;

    public event Action Jumped;
    public event Action<Vector2> Moved;
    public event Action<Vector2> Looked;

    private void OnEnable()
    {
        _input.Enable();

        _input.Player.Jump.started += OnJump;
        _input.Player.Jump.performed += OnJump;
        _input.Player.Jump.canceled += OnJump;

        _input.Player.Look.started += OnLook;
        _input.Player.Look.performed += OnLook;
        _input.Player.Look.canceled += OnLook;

        _input.Player.Move.started += OnMoved;
        _input.Player.Move.performed += OnMoved;
        _input.Player.Move.canceled += OnMoved;
    }

    private void OnDisable()
    {
        _input.Disable();

        _input.Player.Jump.started -= OnJump;
        _input.Player.Jump.performed -= OnJump;
        _input.Player.Jump.canceled -= OnJump;

        _input.Player.Look.started -= OnLook;
        _input.Player.Look.performed -= OnLook;
        _input.Player.Look.canceled -= OnLook;

        _input.Player.Move.started -= OnMoved;
        _input.Player.Move.performed -= OnMoved;
        _input.Player.Move.canceled -= OnMoved;
    }

    private void Awake()
    {
        _input = new PlayerInput();
    }

    private void OnMoved(InputAction.CallbackContext context)
    {
        context.ReadValue<Vector2>();

        if (_moveDirection != context.ReadValue<Vector2>())
        {
            _moveDirection = context.ReadValue<Vector2>();

            if (context.canceled)
                _moveDirection = Vector2.zero;

            Moved?.Invoke(_moveDirection);
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            _isJump = false;
        }
        else if (context.performed && _isJump == false)
        {
            _isJump = true;
            Jumped?.Invoke();
            context = new InputAction.CallbackContext();
        }
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        if (context.canceled)
            _lookDirection = Vector2.zero;

        context.ReadValue<Vector2>();
        _lookDirection = context.ReadValue<Vector2>();

        if (_lookDirection != Vector2.zero)
            Looked?.Invoke(_lookDirection);
    }
}
