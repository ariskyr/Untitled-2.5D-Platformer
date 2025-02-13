using UnityEngine;
using System.Collections;
using Utils;
using Pixelplacement;
using System;

[System.Flags]
public enum FactionMask
{
    None = 0,
    Player = 1 << 0, // 0001
    Knight = 1 << 1, // 0010
    Enemy = 1 << 2, // 0100
    Neutral = 1 << 3, // 1000
    // add more factions here if you need
    // example: Boss = 1 << 4, // 10000
}
public class Damageable : MonoBehaviour
{
    [Header("Core Settings")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private bool invincible = false;
    [SerializeField] private float damageCooldown = 0.2f;

    [Header("Factions")]
    [EnumFlags] public FactionMask factions;
    [EnumFlags] public FactionMask hostileFactions;
    [SerializeField] private bool allowFriendlyFire = false;

    [Header("Effects")]
    [SerializeField] private ParticleSystem hitEffect;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private Material hitMaterial;
    [SerializeField] private float hitFlashDuration = 0.1f;

    private bool canTakeDamage = true;
    private Material originalMaterial;
    private SpriteRenderer spriteRenderer;

    public event Action<int> OnHealthChanged;
    public event Action<GameObject> OnDeath;

    public int CurrentHealth { get; private set; }

    private void Awake()
    {
        CurrentHealth = maxHealth;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer) originalMaterial = spriteRenderer.material;
    }

    public void Initialize(int health)
    {
        CurrentHealth = Mathf.Clamp(health, 0, maxHealth);
    }

    public void TakeDamage(int damage, FactionMask attackerFactions, GameObject attacker)
    {
        if (!canTakeDamage || !ShouldTakeDamage(attackerFactions)) return;

        StartCoroutine(DamageCooldownRoutine());
        ApplyDamage(damage, attacker);
        PlayHitEffects();
    }

    private bool ShouldTakeDamage(FactionMask attackerFactions)
    {
        bool isHostile = (attackerFactions & hostileFactions) != 0;
        bool sameFaction = (attackerFactions & factions) != 0;
        return isHostile || (allowFriendlyFire && sameFaction);
    }

    private void ApplyDamage(int damage, GameObject attacker)
    {
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
        OnHealthChanged?.Invoke(CurrentHealth);

        if (CurrentHealth <= 0) HandleDeath(attacker);
    }

    private void HandleDeath(GameObject killer)
    {
        OnDeath?.Invoke(killer);
        // Add death effects here
    }
    public void Heal(int amount)
    {
        CurrentHealth = Mathf.Min(CurrentHealth + amount, maxHealth);
        OnHealthChanged?.Invoke(CurrentHealth);
    }

    private void PlayHitEffects()
    {
        if (hitEffect) Instantiate(hitEffect, transform.position, Quaternion.identity);
        if (hitSound) AudioSource.PlayClipAtPoint(hitSound, transform.position);
        StartCoroutine(HitFlashRoutine());
    }

    private IEnumerator HitFlashRoutine()
    {
        if (spriteRenderer && hitMaterial)
        {
            spriteRenderer.material = hitMaterial;
            yield return new WaitForSeconds(hitFlashDuration);
            spriteRenderer.material = originalMaterial;
        }
    }

    private IEnumerator DamageCooldownRoutine()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canTakeDamage = true;
    }
}