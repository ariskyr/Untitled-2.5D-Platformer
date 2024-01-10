using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerStateMachine StateMachine { get; private set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerLandState LandState { get; private set; }

    public Animator Animator { get; private set; }
    public Rigidbody RB { get; private set; }
    [SerializeField] private PlayerData playerData;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform facing;

    public Vector3 CurrentVelocity { get; private set; }
    private bool facingRight = true;
    private Vector3 workspace;
    private Quaternion toRotation;

    private void Awake()
    {
        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
        JumpState = new PlayerJumpState(this, StateMachine, playerData, "inAir");
        InAirState = new PlayerInAirState(this, StateMachine, playerData, "inAir");
        LandState = new PlayerLandState(this, StateMachine, playerData, "land");
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
        StateMachine.CurrentState.PhysicsUpdate();
    }

    public void SetVelocityXZ(Vector2 movementVelocity)
    {
        workspace.Set(movementVelocity.x, CurrentVelocity.y, movementVelocity.y); //movementVelocity.y is in z axis
        RB.velocity = workspace;
        CurrentVelocity = workspace;

        //Also rotate the facing while moving
        toRotation = Quaternion.LookRotation(CurrentVelocity) * Quaternion.AngleAxis(-90, Vector3.up);
        if (!movementVelocity.Equals(Vector2.zero))
        {
            facing.rotation = toRotation;
        }
    }

    public void SetVelocityY(float velocityY)
    {
        workspace.Set(CurrentVelocity.x, velocityY, CurrentVelocity.z);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public bool CheckIfGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, playerData.groundCheckRadius, playerData.whatIsGround);
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

    private void AnimationTrigger()
    {
        StateMachine.CurrentState.AnimationTrigger();
    }

    private void AnimationFinishedTrigger()
    {
        StateMachine.CurrentState.AnimationFinishedTrigger();
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
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(groundCheck.position, playerData.groundCheckRadius);
    }
}
