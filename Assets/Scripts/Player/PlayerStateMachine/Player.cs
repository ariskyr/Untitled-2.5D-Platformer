using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerStateMachine StateMachine { get; private set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }

    public Animator Animator { get; private set; }
    public Rigidbody RB { get; private set; }
    [SerializeField] private PlayerData playerData;

    public Vector3 CurrentVelocity { get; private set; }
    private bool facingRight = true;
    private Vector3 workspace;

    private void Awake()
    {
        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
    }

    private void Start()
    {
        Animator = GetComponent<Animator>();
        RB = GetComponent<Rigidbody>();
        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        CurrentVelocity = RB.velocity;
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.LogicUpdate();
    }

    public void setVelocityXZ(float velocityX, float velocityZ)
    {
        workspace.Set(velocityX, CurrentVelocity.y, velocityZ);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public void CheckIfShouldFlip(float xInput)
    {
        if (xInput > 0 && !facingRight)
        {
            Flip();
        }
        else if (xInput < 0 && facingRight)
        {
            Flip();
        }
    }
    private void Flip()
    {
        facingRight = !facingRight;

        if (facingRight)
        {
            Animator.SetFloat("direction", 1f);
        }
        else
        {
            Animator.SetFloat("direction", -1f);
        }


        //Flip the position of the attack point to be always in front of the player
        //m_AttackPoint.RotateAround(transform.position, Vector3.up, 180);
    }
}
