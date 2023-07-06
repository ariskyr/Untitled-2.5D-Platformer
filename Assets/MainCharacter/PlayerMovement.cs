using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public PlayerController controller;
    public float moveSpeed = 40f;
    private Vector2 movementInput;
    private float HorizontalMove = 0f;
    private float VerticalMove = 0f;
    private bool jump = false;
    private bool crouch = false;

    // Update is called once per frame
    private void FixedUpdate()
    {
        HorizontalMove = movementInput.x * moveSpeed;
        VerticalMove = movementInput.y * moveSpeed;
        controller.Move(HorizontalMove * Time.fixedDeltaTime, VerticalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {

        if (context.started && !context.performed)
            jump = true;
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        bool isPressed = (!context.started || context.performed) ^ context.canceled;

        if (context.started && !context.performed)
            crouch = true;
        else if (context.canceled && !context.performed)
            crouch = false;
    }

}
