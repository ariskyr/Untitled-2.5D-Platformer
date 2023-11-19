using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : GenericSingleton<InputManager>
{
    private bool jump = false;
    private bool crouch = false;
    private bool interact = false;

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
        crouch = false;
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
}
