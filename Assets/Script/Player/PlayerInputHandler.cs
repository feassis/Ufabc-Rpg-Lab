using System;
using UnityEngine;
using UnityEngine.InputSystem;

//classe de gerenciamento da interface com o usuário
public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInputs inputActions;

    public static Action<Vector2> OnMoveInput;
    public static Action<bool> OnSprintInput;
    public static Action OnDashInput;
    public static Action OnAttackInput;
    public static Action OnSpecialInput;

    //inicialisa o InputAction e registra os methodos aos eventos de input
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
        inputActions.Combat.Special.performed += Special_performed;
    }

    //chama o evento de special
    private void Special_performed(InputAction.CallbackContext obj)
    {
        OnSpecialInput?.Invoke();
    }

    //chama o evento de attack
    private void Attack_performed(InputAction.CallbackContext obj)
    {
        OnAttackInput?.Invoke();
    }

    //chama o evento de dash performado
    private void Dash_performed(InputAction.CallbackContext obj)
    {
        OnDashInput?.Invoke();
    }

    //chama o evento de sprint cancelado
    private void Sprint_canceled(InputAction.CallbackContext obj)
    {
        OnSprintInput?.Invoke(false);
    }

    //chama o evento de sprint performado
    private void Sprint_performed(InputAction.CallbackContext obj)
    {
        OnSprintInput?.Invoke(true);
    }

    //chama o evento de movimento cancelado
    private void Move_Canceled(InputAction.CallbackContext context)
    {
        OnMoveInput?.Invoke(Vector2.zero);
    }

    //chama o evento de movimento performado
    private void Move_Performed(UnityEngine.InputSystem.InputAction.CallbackContext input)
    {
        OnMoveInput?.Invoke(input.ReadValue<Vector2>());
    }


    //retorna a posição do mouse no mundo
    public static Vector3 GetMousePosInWorld()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();

        mousePos.z = Mathf.Abs(Camera.main.transform.position.z);

        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        mousePos.z = 0f; // 2D → z = 0

        return mousePos;
    }

    //desliga o InputAction e desregistra os methodos aos eventos de input
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

        inputActions.Combat.Dash.performed -= Dash_performed;
        inputActions.Combat.Attack.performed -= Attack_performed;
        inputActions.Combat.Special.performed -= Special_performed;
    }
}




