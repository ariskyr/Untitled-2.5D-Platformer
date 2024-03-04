using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] public int maxHealth = 100;
    [SerializeField] public float currentHealth;
    public float attackCooldown = 0f;
    private Animator animator;


    public Stat attackDamage;
    public Stat attackSpeed;
    public Stat attackDelay;


    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("hurt");
        Debug.Log(transform.name + " takes " + damage + " damage.");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        //Every Character might die in a different way so this will be overwritten
        animator.SetBool("IsDead", true);
        Debug.Log(transform.name + " died!");
    }
}
