using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player2D : GenericSingleton<Player2D>, IPlayerDirectionProvider
{
    public PlayerStateMachine2D StateMachine { get; private set; }

    
    public PlayerIdleState2D IdleState { get; private set; }
    public PlayerMoveState2D MoveState { get; private set; }
    public PlayerJumpState2D JumpState { get; private set; }
    public PlayerInAirState2D InAirState { get; private set; }
    public PlayerLandState2D LandState { get; private set; }
    public PlayerCrouchIdleState2D CrouchIdleState { get; private set; }
    public PlayerCrouchMoveState2D CrouchMoveState { get; private set; }

    public Animator Animator { get; private set; }
    public Rigidbody2D RB { get; private set; }

    public bool facingRight { get; private set; } = true;
    private Vector2 workspace;
    public PlayerInput playerInput;
    private float dir = 1f;
    public Vector2 CurrentVelocity { get; private set; }

    [SerializeField] private Transform groundCheck;

    [Header("Data")]
    [SerializeField] private PlayerData playerData;

    protected override void Awake()
    {
        base.Awake();
        playerInput = GetComponent<PlayerInput>();
        StateMachine = new PlayerStateMachine2D();
        IdleState = new PlayerIdleState2D(this, StateMachine, playerData, "idle");
        MoveState = new PlayerMoveState2D(this, StateMachine, playerData, "move");
        JumpState = new PlayerJumpState2D(this, StateMachine, playerData, "inAir");
        InAirState = new PlayerInAirState2D(this, StateMachine, playerData, "inAir");
        LandState = new PlayerLandState2D(this, StateMachine, playerData, "land");
        CrouchIdleState = new PlayerCrouchIdleState2D(this, StateMachine, playerData, "crouchIdle");
        CrouchMoveState = new PlayerCrouchMoveState2D(this, StateMachine, playerData, "crouchMove");

    }

    private void Start()
    {
        Animator = GetComponent<Animator>();
        RB = GetComponent<Rigidbody2D>();
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

    public void SetVelocityX(float movementVelocity)
    {
        workspace.Set(movementVelocity, CurrentVelocity.y);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public void SetVelocityY(float velocityY)
    {
        workspace.Set(CurrentVelocity.x, velocityY);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public void TeleportPlayer(Vector3 position)
    {
        transform.position = position;
    }

    public bool CheckIfGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, playerData.groundCheckRadius, playerData.whatIsGround);
    }

    public void CheckIfShouldFlip(float xInput)
    {
        if (xInput * Animator.GetFloat("direction") < 0)
        {
            dir = Animator.GetFloat("direction");
            Animator.SetFloat("direction", dir * -1f);
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, playerData.groundCheckRadius);
    }
}
