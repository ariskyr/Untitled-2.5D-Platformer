using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public enum PlayerStates 
    {
        IDLE,
        WALK,
        ATTACK,
        JUMP,
        CROUCH
    }
    PlayerStates CurrentState
    {
        set 
        {
            if (stateLock == false)
            {
                currentState = value;

                switch (currentState)
                {
                    case PlayerStates.IDLE:
                        animator.Play("Player_BT_Idle");
                        break;
                    case PlayerStates.WALK:
                        animator.Play("Player_BT_Walk");
                        break;
                    case PlayerStates.ATTACK:
                        animator.Play("Player_BT_Attack");
                        stateLock = true;
                        break;
                    case PlayerStates.JUMP:
                        animator.Play("Player_BT_Walk");
                        break;
                    case PlayerStates.CROUCH:
                        animator.Play("Player_BT_Walk");
                        break;
                }
            }
            
        }
    }

    public PlayerController controller;
    public float moveSpeed = 40f;
    private Vector2 movementInput;
    private float HorizontalMove = 0f;
    private float VerticalMove = 0f;
    private bool jump = false;
    private bool crouch = false;
    private bool stateLock = false;         // if true, animation state shouldn't change
    private Animator animator;
    private PlayerStates currentState;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

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

        if(movementInput != Vector2.zero) 
        {
            //switch to walking if speed is not zero
            CurrentState = PlayerStates.WALK;
            animator.SetFloat("xMove", movementInput.x);
            animator.SetFloat("zMove", movementInput.y);
        } else
        {
            CurrentState = PlayerStates.IDLE;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && !context.performed)
        {
            CurrentState = PlayerStates.JUMP;
            jump = true;
        }
        // TODO add a OnJumpFinished function event on the end of a jump animation to change state
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.started && !context.performed)
        {
            CurrentState = PlayerStates.CROUCH;
            crouch = true;
        } else if (context.canceled && !context.performed)
        {
            CurrentState = PlayerStates.IDLE;
            crouch = false;
        }
    }

    public void OnFire()
    {
        CurrentState = PlayerStates.ATTACK;
    }

    public void OnFireFinished()
    {
        stateLock = false;
        CurrentState = PlayerStates.IDLE;
    }
}
