using UnityEngine;

public class AttackPoint : MonoBehaviour
{
    [Header("Combat")]
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private LayerMask targetLayers;

    [Header("Effects")]
    [SerializeField] private ParticleSystem attackEffect;
    [SerializeField] private AudioClip attackSound;

    private Damageable ownerDamageable;

    private void Awake()
    {
        ownerDamageable = GetComponentInParent<Damageable>();
    }

    public void Attack()
    {
        PlayAttackEffects();

        Collider[] hits = Physics.OverlapSphere(transform.position, attackRange, targetLayers);
        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent<Damageable>(out var target))
            {
                Vector3 direction = (hit.transform.position - transform.position).normalized;
                target.TakeDamage(damage, ownerDamageable.factions, ownerDamageable.gameObject);
            }
        }
    }

    private void PlayAttackEffects()
    {
        if (attackEffect) Instantiate(attackEffect, transform.position, Quaternion.identity);
        if (attackSound) AudioSource.PlayClipAtPoint(attackSound, transform.position);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}