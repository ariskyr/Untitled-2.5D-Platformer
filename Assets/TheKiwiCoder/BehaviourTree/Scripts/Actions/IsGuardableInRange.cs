using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class IsGuardableInRange : Conditional
{
    public float range = 5f;
    private GameObject nearestGameObjectWithTag;
    private float nearestDistance = Mathf.Infinity;

    protected override State OnUpdate()
    {
        // Find the guardable object
        //! this is not efficient since it needs to be evaluated every frame for each npc
        //! find a better way to make it
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(blackboard.tag);
        foreach (GameObject go in gameObjects)
        {
            float dist = Vector3.Distance(context.transform.position, go.transform.position);
            if (dist < nearestDistance)
            {
                nearestGameObjectWithTag = go;
                nearestDistance = dist;
            }
        }
        if (nearestGameObjectWithTag)
        {
            // Check if the guardable object is within range
            var distance = Vector3.Distance(context.transform.position, nearestGameObjectWithTag.transform.position);
            if (distance < range)
            {
                blackboard.pointOfInterest = nearestGameObjectWithTag;
                _isTriggered = true;
            }
            else
            {
                blackboard.pointOfInterest = null;
                _isTriggered = false;
            }
        }
        else
        {
            _isTriggered = false;
        }
        return base.OnUpdate();
    }
}
