using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviourTree {

    // The context is a shared object every node has access to.
    // Commonly used components and subsytems should be stored here
    // It will be somewhat specfic to your game exactly what to add here.
    // Feel free to extend this class 
    public class Context {
        public GameObject gameObject;
        public Transform transform;
        public Animator animator;
        public Rigidbody physics;
        public NavMeshAgent agent;
        public BoxCollider boxCollider;
        public SphereCollider attackCollider;
        public CharacterController characterController;
        public AI_BoundingBox boundingBox;
        public SensorStats sensorStats;
        public AwarenessSystem awarenessSystem;
        // Add other game specific systems here

        public static Context CreateFromGameObject(GameObject gameObject) {
            // Fetch all commonly used components
            Context context = new()
            {
                gameObject = gameObject,
                transform = gameObject.transform,
                animator = gameObject.GetComponentInChildren<Animator>(),
                physics = gameObject.GetComponent<Rigidbody>(),
                agent = gameObject.GetComponent<NavMeshAgent>(),
                boxCollider = gameObject.GetComponent<BoxCollider>(),
                attackCollider = gameObject.GetComponentInChildren<SphereCollider>(),
                characterController = gameObject.GetComponent<CharacterController>(),
                boundingBox = Object.FindObjectOfType<AI_BoundingBox>(),
                sensorStats = gameObject.GetComponent<SensorStats>(),
                awarenessSystem = gameObject.GetComponent<AwarenessSystem>()
            };

            // Add whatever else you need here...

            return context;
        }
    }
}