using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputSys : Singleton<PlayerInputSys>
{
    internal PlayerInput playerInput;
    internal Vector2 inputVector;

    internal PlayerManager playerManager_Instance;
    internal GameManager gameManager_Instance;

    private void Start()
    {
        playerManager_Instance = PlayerManager.Instance;
        gameManager_Instance = GameManager.Instance;
    }

    protected override void Awake()
    {
        base.Awake();
        playerInput = new();
        playerInput.Player.Enable();
        playerInput.Player.Jump.performed += Jump;
        playerInput.Player.SwitchCharacter.performed += SwitchCharacter;
        playerInput.Player.Movement.performed += RotationPerformed;
        playerInput.Player.Movement.canceled += RotationCanceled;

        playerInput.Player.HoldShift.performed += HoldShift_performed;
        playerInput.Player.HoldShift.canceled += HoldShift_canceled; ;

    }

    private void HoldShift_performed(InputAction.CallbackContext obj)
    {
        playerManager_Instance.HoldShiftPerformed();
    }

    private void HoldShift_canceled(InputAction.CallbackContext obj)
    {
        playerManager_Instance.HoldShiftCanceled();
    }

    private void RotationPerformed(InputAction.CallbackContext obj)
    {
        playerManager_Instance.RotationPerformed(obj.ReadValue<Vector2>());
    }

    private void RotationCanceled(InputAction.CallbackContext obj)
    {
        playerManager_Instance.RotationCanceled();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        playerManager_Instance.Jump();
    }
    private void SwitchCharacter(InputAction.CallbackContext obj)
    {
        playerManager_Instance.SwitchCharacter(int.Parse(obj.control.displayName) - 1);
    }

    public Vector2 GetInputVector() => playerInput.Player.Movement.ReadValue<Vector2>().normalized;

    public void SetMove(bool enabled)
    {
        if (enabled) playerInput.Player.Movement.Enable();
        else playerInput.Player.Movement.Disable();
    }

    public void SetJump(bool enabled)
    {
        if (enabled) playerInput.Player.Jump.Enable();
        else playerInput.Player.Jump.Disable();
    }

    public void SetPlayerControls(bool enabled)
    {
        if (enabled) playerInput.Player.Enable();
        else playerInput.Player.Disable();
    }

}
