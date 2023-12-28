using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(GWorldDebug))]
[CanEditMultipleObjects]
public class GWorldDebugEditor : Editor
{
    private GWorldDebug worldDebug;

    void OnEnable()
    {
        EditorApplication.update += UpdateEditor;
    }

    void OnDisable()
    {
        EditorApplication.update -= UpdateEditor;
    }

    void UpdateEditor()
    {
        if (worldDebug != null)
        {
            Repaint();
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        worldDebug = (GWorldDebug)target;

        Dictionary<string, int> worldStates = GWorld.Instance.GetWorld().GetStates();

        GUILayout.Label("World States:");

        foreach (KeyValuePair<string, int> state in worldStates)
        {
            GUILayout.Label($"{state.Key}: {state.Value}");
        }

        serializedObject.ApplyModifiedProperties();
        EditorApplication.QueuePlayerLoopUpdate();
    }
}