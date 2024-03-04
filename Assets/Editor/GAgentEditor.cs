using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

[CustomEditor(typeof(GAgentDebug))]
[CanEditMultipleObjects]
public class GAgentDebugEditor : Editor 
{


    void OnEnable()
    {

    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        serializedObject.Update();
        GAgentDebug agent = (GAgentDebug) target;
        GUILayout.Label("Name: " + agent.name);
        Vector3 velocity = agent.gameObject.GetComponent<NavMeshAgent>().velocity;
        string direction = Mathf.Abs(velocity.x) > Mathf.Abs(velocity.z) ?
                    (velocity.x > 0 ? "Moving RIGHT" : "Moving LEFT") :
                    (velocity.z > 0 ? "Moving UP" : "Moving DOWN");
        GUILayout.Label("Direction: " + direction);
        GUILayout.Label("Actions: ");
        foreach (GAction a in agent.gameObject.GetComponent<GAgent>().actions)
        {
            string pre = "";
            string eff = "";

            foreach (KeyValuePair<string, int> p in a.preconditions)
                pre += p.Key + ", ";
            foreach (KeyValuePair<string, int> e in a.aftereffects)
                eff += e.Key + ", ";

            GUILayout.Label("====  " + a.actionName + " \nPRE: (" + pre + ") \nAFTER: (" + eff + ")");
        }
        GUILayout.Label("Goals: ");
        foreach (KeyValuePair<SubGoal, int> g in agent.gameObject.GetComponent<GAgent>().goals)
        {
            GUILayout.Label("---: ");
            foreach (KeyValuePair<string, int> sg in g.Key.sgoals)
                GUILayout.Label("--->  " + sg.Key + ". Importance: " + g.Value);
        }
        GUILayout.Label("Current Action: " + agent.gameObject.GetComponent<GAgent>().currentAction);
        if (agent.gameObject.GetComponent<GAgent>().GetActionQueue() != null)
        {
            GUILayout.Label("Plan: "); 
            foreach (GAction a in agent.gameObject.GetComponent<GAgent>().GetActionQueue())
            {
                GUILayout.Label("==== " + a.actionName);
            }

        }
        
        serializedObject.ApplyModifiedProperties();
    }
}