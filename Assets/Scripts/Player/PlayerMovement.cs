using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour, IDataPersistence
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
                        canMove = true;
                        break;
                    case PlayerStates.WALK:
                        animator.Play("Player_BT_Walk");
                        canMove = true;
                        break;
                    case PlayerStates.ATTACK:
                        animator.Play("Player_BT_Attack");
                        canMove = false;
                        break;
                    case PlayerStates.JUMP:
                        animator.Play("Player_BT_Jump");
                        break;
                    case PlayerStates.CROUCH:
                        animator.Play("Player_BT_Crouch");
                        break;
                }
            }
            
        }
    }

    //the player position
    public Vector3 playerPosition;

    public PlayerController controller;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public float moveSpeed = 40f;
    public int attackDamage = 40;
    private Vector2 movementInput;
    private float HorizontalMove = 0f;
    private float VerticalMove = 0f;
    private bool jump = false;
    private bool interact = false;
    private bool stateLock = false;         // if true, animation state shouldn't change
    private bool canMove = true;            // if true, character can move
    private Animator animator;
    private PlayerStates currentState;
    private CharacterCombat playerCombat;
    
    public void LoadData(GameData data)
    {
        transform.position = data.playerPosition;
    }

    public void SaveData(GameData data)
    {
        data.playerPosition = transform.position;
    }

    //maybe we need to change public property of this method
    public void TeleportPlayer(Vector3 position)
    {
        transform.position = position;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerCombat = GetComponent<CharacterCombat>();

    }

    private void Update()
    {
        playerPosition = transform.position;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //movementInput = InputManager.Instance.GetMovePressed();

        // flag to lock movement if attacking, I don't like something about how it feels
        // revisit later
        if (!canMove)
        {
            movementInput = Vector2.zero;
        }
            HorizontalMove = movementInput.x * moveSpeed * Time.fixedDeltaTime;
            VerticalMove = movementInput.y * moveSpeed * Time.fixedDeltaTime;
            controller.Move(HorizontalMove, VerticalMove, InputManager.Instance.GetCrouchPressed(), jump);
            jump = false;
        
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();

        if (canMove && movementInput != Vector2.zero) 
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
            // maybe change this so that the player can attack in the air
            stateLock = true;
        }
    }

    public void OnLanding()
    {
        stateLock = false;
        CurrentState = PlayerStates.IDLE;
    }

    // communication with the controller, if the player can crouch or not, regardless of the crouch button
    public void OnCrouching(bool isCrouching)
    {
        if (isCrouching)
        {
            CurrentState = PlayerStates.CROUCH;
            stateLock = true;   // lock to prevent animation change, might need to change if we want to have a crouch attack
        }
        else
        {
            stateLock = false;
            CurrentState = PlayerStates.IDLE;
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {

        if (context.started && !context.performed)
        {
            CurrentState = PlayerStates.ATTACK;
            stateLock = true;
            Collider[] hitcolliders = Physics.OverlapSphere(attackPoint.position, attackRange);
            foreach(Collider enemy in hitcolliders)
            {
                if(enemy.gameObject.tag == "Enemy" && playerCombat != null && enemy.GetComponent<CharacterStats>() != null)
                {
                    //playerCombat item contains the stats of the player (attack damage etc) and reduces the stats of the enemy (currentHealth)
                    playerCombat.Attack(enemy.GetComponent<CharacterStats>());
                }
            }
        }
    }

    public void OnAttackFinished()
    {
        stateLock = false;
        canMove = true;
        CurrentState = PlayerStates.IDLE;
    }

    private void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
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
