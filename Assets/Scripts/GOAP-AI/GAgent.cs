using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;


public class SubGoal
{
    public Dictionary<string, int> sgoals;
    public bool remove;

    public SubGoal(string s, int i, bool r)
    {
        //initialization
        sgoals = new Dictionary<string, int>
        {
            { s, i }
        };
        remove = r;
    }
}

public class GAgent : MonoBehaviour
{
    [Header("Actions")]
    public List<GAction> actions = new List<GAction>();
    public Dictionary<SubGoal, int> goals = new Dictionary<SubGoal, int>();
    public GInventory inventory = new GInventory();
    public WorldStates beliefs = new WorldStates();
    public GAction currentAction;

    GPlanner planner;
    Queue<GAction> actionQueue;
    SubGoal currentGoal;
    bool invoked = false;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        GAction[] acts = this.GetComponents<GAction>();
        foreach (GAction action in acts)
        {
            actions.Add(action);
        }
    }

    //debug purposes
    public Queue<GAction> GetActionQueue()
    {
        return actionQueue;
    }

    void CompleteAction()
    {
        currentAction.running = false;
        currentAction.PostPerform();
        invoked = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (currentAction != null && currentAction.running)
        {

            // continuously update the destination while the action is running
            if (currentAction.target != null)
            {
                currentAction.agent.SetDestination(currentAction.target.transform.position);
            }

            //navmesh agent
            float distanceToTarget = Vector3.Distance(currentAction.target.transform.position, this.transform.position);
            //if this distance is <2, the action is considered complete
            if (currentAction.agent.hasPath && distanceToTarget < 2f) 
            {
                if (!invoked)
                {
                    Invoke(nameof(CompleteAction), currentAction.duration);
                    invoked = true;
                }
            }
            return;
        }

        if (planner == null || actionQueue == null)
        {
            planner = new GPlanner();

            //order our goals by important
            var sortedGoals = from entry in goals orderby entry.Value descending select entry;

            foreach(KeyValuePair<SubGoal, int> sg in sortedGoals)
            {
                actionQueue = planner.Plan(actions, sg.Key.sgoals, beliefs);
                if (actionQueue != null)
                {
                    currentGoal = sg.Key;
                    break;
                }
            }
        }

        if (actionQueue != null && actionQueue.Count == 0) 
        { 
            if (currentGoal.remove)
            {
                goals.Remove(currentGoal);
            }
            planner = null;
        }

        if (actionQueue != null && actionQueue.Count > 0)
        {
            currentAction = actionQueue.Dequeue();
            if (currentAction.PrePerform())
            {
                if (currentAction.target == null && currentAction.targetTag != "")
                {
                    currentAction.target = GameObject.FindWithTag(currentAction.targetTag);
                }

                if (currentAction.target != null)
                {
                    currentAction.running = true;
                    //original setting of destination
                    currentAction.agent.SetDestination(currentAction.target.transform.position);
                }
            }
            else
            {
                actionQueue = null;
            }
        }
    }
}
