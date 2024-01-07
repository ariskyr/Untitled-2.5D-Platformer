using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GNode
{
    public GNode parent;
    public float cost;
    public Dictionary<string, int> state;
    public GAction action;

    public GNode(GNode parent, float cost, Dictionary<string, int> allstates, GAction action)
    {
        this.parent = parent;
        this.cost = cost;
        this.state = new Dictionary<string, int>(allstates);
        this.action = action;
    }
    public GNode(GNode parent, float cost, Dictionary<string, int> allstates, Dictionary<string, int> beliefstates, GAction action)
    {
        this.parent = parent;
        this.cost = cost;
        this.state = new Dictionary<string, int>(allstates);
        foreach (KeyValuePair<string, int> belief in beliefstates)
        {
            if (!this.state.ContainsKey(belief.Key))
            {
                this.state.Add(belief.Key, belief.Value);
            }
        }
        this.action = action;
    }
}

public class GPlanner
{
    public Queue<GAction> Plan(List<GAction> actions, Dictionary<string, int> goal, WorldStates beliefStates)
    {
        List<GAction> usableActions = new List<GAction>();
        foreach (GAction action in actions)
        {
            if (action.isAchievable())
            {
                usableActions.Add(action);
            }
        }

        List<GNode> leaves = new List<GNode>();
        GNode start = new GNode(null, 0, GWorld.Instance.GetWorld().GetStates(), beliefStates.GetStates(), null);

        bool success = BuildGraph(start, leaves, usableActions, goal);

        if (!success)
        {
            Debug.LogError("NO PLAN");
            return null;
        }

        GNode cheapest = null;
        foreach (GNode leaf in leaves)
        {
            if (cheapest == null)
            {
                cheapest = leaf;
            }
            else
            {
                if (leaf.cost < cheapest.cost)
                {
                    cheapest = leaf;
                }
            }
        }

        List<GAction> result = new List<GAction>();
        GNode n = cheapest;
        while (n != null)
        {
            if(n.action != null)
            {
                result.Insert(0, n.action);
            }
            n = n.parent;
        }

        Queue<GAction> queue = new Queue<GAction>();
        foreach (GAction action in result)
        {
            queue.Enqueue(action);
        }
        return queue;
    }

    private bool BuildGraph(GNode parent, List<GNode> leaves, List<GAction> usableActions, Dictionary<string, int> goal)
    {
        bool foundPath = false;
        foreach (GAction action in usableActions)
        {
            if (action.isAchievableGiven(parent.state))
            {
                Dictionary<string, int> currentState = new Dictionary<string, int>(parent.state);
                foreach (KeyValuePair<string, int> effect in action.aftereffects)
                {
                    if (!currentState.ContainsKey(effect.Key))
                    {
                        currentState.Add(effect.Key, effect.Value);
                    }
                }

                GNode node = new GNode(parent, parent.cost + action.cost, currentState, action);

                if (GoalAchieved(goal, currentState))
                {
                    leaves.Add(node);
                    foundPath = true;
                }
                else
                {
                    List<GAction> subset = ActionSubset(usableActions, action);
                    bool found = BuildGraph(node, leaves, subset, goal);
                    if (found)
                    {
                        foundPath = true;
                    }
                }
            }
        }
        return foundPath;
    }

    private bool GoalAchieved(Dictionary<string, int> goal, Dictionary<string, int> state)
    {
        foreach (KeyValuePair <string, int> g in goal)
        {
            if (!state.ContainsKey(g.Key))
            {
                return false;
            }
        }
        return true;
    }

    private List<GAction> ActionSubset(List<GAction> actions, GAction removeMe)
    {
        List<GAction> subset = new List<GAction>();
        foreach (GAction action in actions)
        {
            if (!action.Equals(removeMe))
            {
                subset.Add(action);
            }
        }
        return subset;
    }
}
