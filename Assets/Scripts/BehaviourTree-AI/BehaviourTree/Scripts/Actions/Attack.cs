using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class Attack : ActionNode
{
    private Vector3 direction;
    [SerializeField] private int damage = 10;
    [SerializeField] private LayerMask layerMask;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {

        //set eye direction
        if (context.sensorStats)
        {
            direction = context.agent.velocity.normalized;
            context.sensorStats.EyeDirection = direction;
        }

        //set animation
        context.animator.SetFloat("DirectionX", direction.x);
        context.animator.SetFloat("DirectionY", direction.z);


        Collider[] hittableColliders = Physics.OverlapSphere(context.transform.position, context.attackCollider.radius, layerMask);

        Debug.Log(hittableColliders);

        foreach (var collider in hittableColliders)
        {
            if (collider.CompareTag("Player"))
            {
                hittableColliders[0].GetComponent<Destructable>().OnAttackHit(hittableColliders[0].transform.position, new(4.0f, 4.0f), damage);
            }
        }
        return State.Success;
    }
}
