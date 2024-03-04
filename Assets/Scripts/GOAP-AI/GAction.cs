using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//this class is not actually used by anyone, but every action derives from it
public abstract class GAction : MonoBehaviour
{
    public string actionName = typeof(GAction).Name;
    //very important, since the Planner will make a plan based on the lowest cost,
    //if that plan is possible (all preconditions are met)
    public float cost = 1.0f;
    //target location that the navmesh agent will go
    public GameObject target;
    //sometimes tag is used instead of an explicit target
    public string targetTag;
    //duration of the action, example: how much should we stay at a waypoint
    public float duration = 0;
    //what are the conditions for this action to be taken
    public WorldState[] preConditions;
    //what are the effects of finishing this action
    public WorldState[] afterEffects;

    public NavMeshAgent agent;

    //internal, easier to use in code, WorldState above are used/shown in editor
    public Dictionary<string, int> preconditions;
    public Dictionary<string, int> aftereffects;

    //what the agent knows to be true
    public WorldStates agentBeliefs;

    //the agent's invetory
    public GInventory inventory;

    //if this action is actively being performed atm
    public bool running = false;

    public GAction()
    {
        preconditions = new Dictionary<string, int>();
        aftereffects = new Dictionary<string, int>();
    }

    public void Awake()
    {
        agent = this.gameObject.GetComponent<NavMeshAgent>();

        if (preConditions != null )
        {
            foreach(WorldState w in preConditions)
            {
                preconditions.Add(w.key, w.value);
            }
        }

        if (afterEffects != null )
        { 
            foreach(WorldState w in afterEffects)
            {
                aftereffects.Add(w.key, w.value);
            }
        }

        inventory = this.GetComponent<GAgent>().inventory;
        agentBeliefs = this.GetComponent<GAgent>().beliefs;
    }

    public bool isAchievable()
    {
        return true;
    }

    public bool isAchievableGiven(Dictionary<string, int> conditions)
    {
        foreach (KeyValuePair<string, int> p in preconditions)
        {
            //if given some conditions, those do not match with the internal
            //preconditions of an action, the action cannot be taken
            if (!conditions.ContainsKey(p.Key))
                return false;
        }
        //the action can be taken, if all conditions are met
        return true;
    }

    //these allow for custom code to be executed, before and after an action
    public abstract bool PrePerform();
    public abstract bool PostPerform();
}
