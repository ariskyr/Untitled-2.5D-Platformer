using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float lookRadius = 3f;
    private Animator animator;
    private Transform target;
    private NavMeshAgent agent;
    private bool targetInSight;
    private bool facingRight = false; //Similarly to the logic of the PlayerController, determine which way the enemy is facing
    private float distance;
    [SerializeField] private LayerMask PlayerLayer;
    private CharacterCombat enemyCombat;


    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyCombat = GetComponent<CharacterCombat>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        targetInSight = Physics.CheckSphere(transform.position, lookRadius, PlayerLayer);

        ApproachTarget();
    }

    private void ApproachTarget()
    {
        if (targetInSight)
        {
            agent.SetDestination(target.position);
            if (agent.velocity != Vector3.zero) {
                animator.SetFloat("walk", 1f);
            }

            distance = Vector3.Distance(transform.position, target.position);
            if (distance <= agent.stoppingDistance)
            {
                animator.SetFloat("walk", 0f);
                //Attack
                enemyCombat.Attack(target.GetComponent<CharacterStats>());
            }
        }
        else
        {
            agent.SetDestination(transform.position);
            if (agent.velocity == Vector3.zero)
            {
                animator.SetFloat("walk", 0f);
            }
        }
        if (facingRight && agent.velocity.x < 0)
        {
            Flip();
        }
        else if (!facingRight && agent.velocity.x > 0)
        {
            Flip();
        }
    }
    
    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;
        
        //Flip the object
        transform.Rotate(0,180,0);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
