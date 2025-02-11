using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : GenericSingleton<Player>, IDataPersistence
{
    public PlayerStateMachine StateMachine { get; private set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerLandState LandState { get; private set; }
    public PlayerCrouchIdleState CrouchIdleState { get; private set; }
    public PlayerCrouchMoveState CrouchMoveState { get; private set; }
    public PlayerAttackState AttackState { get; private set; }

    public PlayerDeadState DeadState { get; private set; }

    public Animator Animator { get; private set; }
    public Rigidbody RB { get; private set; }
    [SerializeField] private PlayerData playerData;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform facing;

    public Vector3 CurrentVelocity { get; private set; }
    public bool facingRight { get; private set; } = true;
    private Vector3 workspace;
    private Quaternion toRotation;
    private BoxCollider topCollider;
    private float dir = 1f;

    public int CurrentHealth { get; private set; }
    public int CurrentLevel { get; private set; }
    public int CurrentExperience { get; private set; }
    public int CurrentGold { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
        JumpState = new PlayerJumpState(this, StateMachine, playerData, "inAir");
        InAirState = new PlayerInAirState(this, StateMachine, playerData, "inAir");
        LandState = new PlayerLandState(this, StateMachine, playerData, "land");
        CrouchIdleState = new PlayerCrouchIdleState(this, StateMachine, playerData, "crouchIdle");
        CrouchMoveState = new PlayerCrouchMoveState(this, StateMachine, playerData, "crouchMove");
        AttackState = new PlayerAttackState(this, StateMachine, playerData, "attack");
        DeadState = new PlayerDeadState(this, StateMachine, playerData, "dead");

        CurrentHealth = playerData.maxHealth;
        CurrentLevel = playerData.startingLevel;
        CurrentExperience = playerData.startingExperience;
        CurrentGold = playerData.startingGold;
    }

    private void OnEnable()
    {
        GameEventsManager.Instance.playerEvents.onHealthGained += HealthGained;
        GameEventsManager.Instance.playerEvents.onHealthLost += HealthLost;
        GameEventsManager.Instance.playerEvents.onExperienceGained += ExperienceGained;
        GameEventsManager.Instance.playerEvents.onGoldGained += GoldGained;
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.playerEvents.onHealthGained -= HealthGained;
        GameEventsManager.Instance.playerEvents.onHealthLost -= HealthLost;
        GameEventsManager.Instance.playerEvents.onExperienceGained -= ExperienceGained;
        GameEventsManager.Instance.playerEvents.onGoldGained -= GoldGained;
    }

    private void Start()
    {
        Animator = GetComponent<Animator>();
        RB = GetComponent<Rigidbody>();
        topCollider = GetComponent<BoxCollider>();
        StateMachine.Initialize(IdleState);

        GameEventsManager.Instance.playerEvents.PlayerHealthChange(CurrentHealth);
        GameEventsManager.Instance.playerEvents.PlayerLevelChange(CurrentLevel);
        GameEventsManager.Instance.playerEvents.PlayerExperienceChange(CurrentExperience);
        GameEventsManager.Instance.playerEvents.GoldChange(CurrentGold);

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

    private void HealthGained(int health)
    {
        CurrentHealth += health;
        if (CurrentHealth > playerData.maxHealth)
        {
            CurrentHealth = playerData.maxHealth;
        }
        GameEventsManager.Instance.playerEvents.PlayerHealthChange(CurrentHealth);
    }

    private void HealthLost(int health)
    {
        CurrentHealth -= health;
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            Die();
        }
        GameEventsManager.Instance.playerEvents.PlayerHealthChange(CurrentHealth);
    }

    private void Die()
    {
        StateMachine.ChangeState(DeadState);
        GameEventsManager.Instance.playerEvents.PlayerDeath();
    }

    private void GoldGained(int gold)
    {
        CurrentGold += gold;
        GameEventsManager.Instance.playerEvents.GoldChange(CurrentGold);
    }

    private void ExperienceGained(int experience)
    {
        CurrentExperience += experience;
        //check if player can level up
        while (CurrentExperience >= playerData.experienceToNextLevel)
        {
            CurrentExperience -= playerData.experienceToNextLevel;
            CurrentLevel++;
            GameEventsManager.Instance.playerEvents.PlayerLevelChange(CurrentLevel);
        }
        GameEventsManager.Instance.playerEvents.PlayerExperienceChange(CurrentExperience);
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

    public void SetTopColliderHeight(float height)
    {
        Vector3 center = topCollider.center;
        workspace.Set(topCollider.size.x, height, topCollider.size.z);
        center.y -= (topCollider.size.y - height) / 2;

        topCollider.size = workspace;
        topCollider.center = center;
    }

    public bool CheckIfGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, playerData.groundCheckRadius, playerData.whatIsGround);
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

    public void TeleportPlayer(Vector3 position)
    {
        transform.position = position;
    }

    public void LoadData(GameData data)
    {
        transform.position = data.playerPosition;
        CurrentExperience = data.playerExperience;
        CurrentLevel = data.playerLevel;
        CurrentHealth = data.playerHealth;
        CurrentGold = data.playerGold;

        CurrentHealth = data.playerHealth > 0 ? data.playerHealth : playerData.maxHealth;
        StateMachine.ChangeState(IdleState);
        GameEventsManager.Instance.playerEvents.PlayerHealthChange(CurrentHealth);
        RB.isKinematic = false;
    }

    public void SaveData(GameData data)
    {
        data.playerPosition = transform.position;
        data.playerExperience = CurrentExperience;
        data.playerLevel = CurrentLevel;
        data.playerHealth = CurrentHealth;
        data.playerGold = CurrentGold;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(groundCheck.position, playerData.groundCheckRadius);
    }
}
