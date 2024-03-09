using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("Move State")]
    public float movementVelocity = 7f;

    [Header("Jump State")]
    public float jumpVelocity = 5f;

    [Header("Crouch States")]
    public float crouchMovementVelocity = 3f;
    public float crouchColliderHeight = 0.15f;
    public float standColliderHeight = 0.35f;

    [Header("Check Variables")]
    public float groundCheckRadius = 0.2f;
    public LayerMask whatIsGround;

    [Header("Misc")]
    public int maxHealth = 100;
    public int startingLevel = 1;
    public int startingExperience = 0;
    public int startingGold = 5;
    //maybe this needs to scale
    public int experienceToNextLevel = 100;
}
