using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputSubscription : MonoBehaviour
{
    InputMap _Input = null;

    public Vector2 MovementInput { get; private set; } = Vector2.zero;
    public Vector2 CameraInput { get; private set; } = Vector2.zero;
    public bool MenuInput { get; private set; } = false;
    public bool JumpInput { get; private set; } = false;
    public bool FireInput { get; private set; } = false;
    public bool InteractInput { get; private set; } = false;
    public bool SprintInput { get; private set; } = false;
    public float SwitchWeaponInput { get; private set; } = 0f;

    public Action OnInteractInput;
    public Action OnFireInput;
    public Action<float> OnSwitchWeaponInput;

    private void OnEnable() //subscribe to inputs
    {
        _Input = new InputMap();

        _Input.PlayerInputMap.Enable();

        _Input.PlayerInputMap.MovementInput.performed += SetMovement;
        _Input.PlayerInputMap.MovementInput.canceled += SetMovement;

        _Input.PlayerInputMap.CameraInput.performed += SetCamera;
        _Input.PlayerInputMap.CameraInput.canceled += SetCamera;

        _Input.PlayerInputMap.JumpInput.performed += SetJump;
        _Input.PlayerInputMap.JumpInput.canceled += SetJump;

        _Input.PlayerInputMap.FireInput.performed += SetFire;
        _Input.PlayerInputMap.FireInput.performed += CallFire;
        _Input.PlayerInputMap.FireInput.canceled += SetFire;

        _Input.PlayerInputMap.SprintInput.performed += SetSprint;
        _Input.PlayerInputMap.SprintInput.canceled += SetSprint;

        _Input.PlayerInputMap.InteractInput.performed += SetInteract;
        _Input.PlayerInputMap.InteractInput.performed += CallInteract;
        _Input.PlayerInputMap.InteractInput.canceled += SetInteract;


        _Input.PlayerInputMap.SwitchWeaponInput.performed += SetSwitchWeaponInput;
        _Input.PlayerInputMap.SwitchWeaponInput.canceled += SetSwitchWeaponInput;

    }

    private void OnDisable() //unsubscribe to inputs
    {
        _Input.PlayerInputMap.Disable();

        _Input.PlayerInputMap.MovementInput.performed -= SetMovement;
        _Input.PlayerInputMap.MovementInput.canceled  -= SetMovement;

        _Input.PlayerInputMap.CameraInput.performed -= SetCamera;
        _Input.PlayerInputMap.CameraInput.canceled -= SetCamera;

        _Input.PlayerInputMap.JumpInput.performed -= SetJump;
        _Input.PlayerInputMap.JumpInput.canceled -= SetJump;

        _Input.PlayerInputMap.FireInput.performed -= SetFire;
        _Input.PlayerInputMap.FireInput.performed -= CallFire;
        _Input.PlayerInputMap.FireInput.canceled -= SetFire;

        _Input.PlayerInputMap.SprintInput.performed += SetSprint;
        _Input.PlayerInputMap.SprintInput.canceled += SetSprint;

        _Input.PlayerInputMap.InteractInput.performed -= SetInteract;
        _Input.PlayerInputMap.InteractInput.performed -= CallInteract;
        _Input.PlayerInputMap.InteractInput.canceled -= SetInteract;

        _Input.PlayerInputMap.SwitchWeaponInput.performed -= SetSwitchWeaponInput;
        _Input.PlayerInputMap.SwitchWeaponInput.canceled -= SetSwitchWeaponInput;

    }

    private void Update()
    {
        MenuInput = _Input.PlayerInputMap.MenuInput.WasPressedThisFrame();
    }

    private void SetMovement(InputAction.CallbackContext ctx)
    {
        MovementInput = ctx.ReadValue<Vector2>();

    }

    private void SetJump(InputAction.CallbackContext ctx)
    {
        JumpInput = ctx.performed;
    }

    private void SetSprint(InputAction.CallbackContext ctx)
    {
        SprintInput = ctx.performed;
    }

    private void SetFire(InputAction.CallbackContext ctx)
    {
        FireInput = ctx.performed;
    }

    private void SetInteract(InputAction.CallbackContext ctx)
    {
        FireInput = ctx.performed;
    }

    private void CallFire(InputAction.CallbackContext ctx)
    {
        OnFireInput?.Invoke();
    }

    private void SetCamera(InputAction.CallbackContext ctx)
    {
        CameraInput = ctx.ReadValue<Vector2>();
    }

    private void CallInteract(InputAction.CallbackContext ctx)
    {
        OnInteractInput?.Invoke();
    }

    private void SetSwitchWeaponInput(InputAction.CallbackContext ctx)
    {
        SwitchWeaponInput = ctx.ReadValue<Vector2>().y;
        OnSwitchWeaponInput?.DynamicInvoke(SwitchWeaponInput);
    }
}
