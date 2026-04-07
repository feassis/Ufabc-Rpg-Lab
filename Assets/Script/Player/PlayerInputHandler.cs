using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInputs inputActions;

    public static Action<Vector2> OnMoveInput;
    public static Action<bool> OnSprintInput;
    public static Action OnDashInput;
    public static Action OnAttackInput;

    private void OnEnable()
    {
        if(inputActions == null)
        {
            inputActions = new PlayerInputs();
        }
        inputActions.Enable();
        inputActions.Combat.Move.performed += Move_Performed;
        inputActions.Combat.Move.canceled += Move_Canceled;
        inputActions.Combat.Sprint.performed += Sprint_performed;
        inputActions.Combat.Sprint.canceled += Sprint_canceled;
        inputActions.Combat.Dash.performed += Dash_performed;
        inputActions.Combat.Attack.performed += Attack_performed;
    }

    private void Attack_performed(InputAction.CallbackContext obj)
    {
        OnAttackInput?.Invoke();
    }

    private void Dash_performed(InputAction.CallbackContext obj)
    {
        OnDashInput?.Invoke();
    }

    private void Sprint_canceled(InputAction.CallbackContext obj)
    {
        OnSprintInput?.Invoke(false);
    }

    private void Sprint_performed(InputAction.CallbackContext obj)
    {
        OnSprintInput?.Invoke(true);
    }

    private void Move_Canceled(InputAction.CallbackContext context)
    {
        OnMoveInput?.Invoke(Vector2.zero);
    }

    private void Move_Performed(UnityEngine.InputSystem.InputAction.CallbackContext input)
    {
        OnMoveInput?.Invoke(input.ReadValue<Vector2>());
    }

    public static Vector3 GetMousePosInWorld()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();

        mousePos.z = Mathf.Abs(Camera.main.transform.position.z);

        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        mousePos.z = 0f; // 2D → z = 0

        return mousePos;
    }

    private void OnDisable()
    {
        if(inputActions == null)
        {
            return;
        }

        inputActions.Disable();

        inputActions.Combat.Move.performed -= Move_Performed;
        inputActions.Combat.Move.canceled -= Move_Canceled;
        inputActions.Combat.Sprint.performed -= Sprint_performed;
        inputActions.Combat.Sprint.canceled -= Sprint_canceled;
    }
}




