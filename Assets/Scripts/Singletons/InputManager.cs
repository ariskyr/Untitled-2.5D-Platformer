using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : GenericSingleton<InputManager>
{
    private Vector2 move = Vector2.zero;
    private bool attack = false;
    private bool jump = false;
    private bool crouch = false;
    private bool interact = false;
    private bool test = false;

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public Vector2 GetMovePressed()
    {
        return move;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            attack = true;
        }
        else if (context.canceled)
        {
            attack = false;
        }
    }

    public bool GetAttackPressed()
    {
        bool result = attack;
        attack = false;
        return result;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jump = true;
        }
        else if (context.canceled)
        {
            jump = false;
        }
    }
    public bool GetJumpPressed()
    {
        bool result = jump;
        jump = false;
        return result;
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            crouch = true;
        }
        else if (context.canceled)
        {
            crouch = false;
        }
    }

    public bool GetCrouchPressed()
    {
        bool result = crouch;
        return result;
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            interact = true;
        }
        else if (context.canceled)
        {
            interact = false;
        }
    }
    public bool GetInteractPressed()
    {
        bool result = interact;
        interact = false;
        return result;
    }

    public void OnTest(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            test = true;
        }
        else if (context.canceled)
        {
            test = false;
        }
    }
    public bool GetTestPressed()
    {
        bool result = test;
        test = false;
        return result;
    }
}
