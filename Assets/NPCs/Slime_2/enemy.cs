using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    public Animator animator;
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        animator.SetTrigger("hurt");

        if(currentHealth < 0 )
        {
            Die();
        }
    }
    
    void Die()
    {
        Debug.Log("Died!");
        animator.SetBool("IsDead", true);
        
       GetComponent<BoxCollider>().enabled = false;
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
