using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class DialogueVariables
{
    public Dictionary<string, Ink.Runtime.Object> Variables { get; private set; }

    private Story globalVariablesStory;
    private const string saveVariablesKey = "INK_VARIABLES";

    public DialogueVariables(TextAsset loadGlobalsJSON)
    {
        globalVariablesStory = new(loadGlobalsJSON.text);

        // TODO: REPLACE with another save/load method
        if (PlayerPrefs.HasKey(saveVariablesKey))
        {
            string jsonState = PlayerPrefs.GetString(saveVariablesKey);
            globalVariablesStory.state.LoadJson(jsonState);
        }

        Variables = new Dictionary<string, Ink.Runtime.Object>();
        foreach (string name in globalVariablesStory.variablesState)
        {
            Ink.Runtime.Object value = globalVariablesStory.variablesState.GetVariableWithName(name);
            Variables.Add(name, value);
            Debug.Log("Initialized global dialogue variable: " + name + " = " + value);
        }
    }

    public void StartListening(Story story)
    {
        VariablesToStory(story);
        story.variablesState.variableChangedEvent += VariableChanged;
    }

    public void StopListening(Story story)
    {
        story.variablesState.variableChangedEvent -= VariableChanged;
    }

    private void VariableChanged(string name, Ink.Runtime.Object value)
    {
        if (Variables.ContainsKey(name))
        {
            Variables.Remove(name);
            Variables.Add(name, value);
        }
    }

    private void VariablesToStory(Story story)
    {
        foreach(KeyValuePair<string, Ink.Runtime.Object> variable in Variables)
        {
            story.variablesState.SetGlobal(variable.Key, variable.Value);
        }
    }

    public void SaveVariables()
    {
        if (globalVariablesStory != null)
        {
            VariablesToStory(globalVariablesStory);
            // TODO: REPLACE with another save/load method
            PlayerPrefs.SetString(saveVariablesKey, globalVariablesStory.state.ToJson());
        }
    }
}
