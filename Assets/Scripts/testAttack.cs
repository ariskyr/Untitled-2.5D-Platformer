using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testAttack : MonoBehaviour
{
    Destructable[] targets;

    private void Awake()
    {

        targets = FindObjectsOfType<Destructable>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //test attack

        if (InputManager.Instance.GetTestPressed())
        {
            foreach (Destructable t in targets)
            {
                t.OnAttackHit(t.transform.position, new(4.0f, 4.0f), 5);
            }
        }
        //test experience
        /*        
        if (InputManager.Instance.GetTestPressed())
        {
            GameEventsManager.Instance.playerEvents.ExperienceGained(10);
        }*/
        //test gold
        /*
        if (InputManager.Instance.GetTestPressed())
        {
            GameEventsManager.Instance.playerEvents.GoldGained(10);
        }
        */
    }
}