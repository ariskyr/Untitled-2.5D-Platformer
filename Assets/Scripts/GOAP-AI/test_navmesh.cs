using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class test_navmesh : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = target.transform.position;

        Vector3 velocity = agent.velocity;

        // Determine the orientation based on the velocity
        if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.z))
        {
            // Horizontal movement (left or right)
            if (velocity.x > 0)
            {
                Debug.Log("Moving right");
            }
            else
            {
                Debug.Log("Moving left");
            }
        }
        else
        {
            // Vertical movement (up or down)
            if (velocity.z > 0)
            {
                Debug.Log("Moving up");
            }
            else
            {
                Debug.Log("Moving down");
            }
        }
    }
}
