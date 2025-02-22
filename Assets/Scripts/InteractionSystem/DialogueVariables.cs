using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class DialogueVariables
{
    public Dictionary<string, Ink.Runtime.Object> Variables { get; private set; }

    private readonly Story globalVariablesStory;

    public DialogueVariables(TextAsset loadGlobalsJSON)
    {
        globalVariablesStory = new(loadGlobalsJSON.text);

        Variables = new Dictionary<string, Ink.Runtime.Object>();
        foreach (string name in globalVariablesStory.variablesState)
        {
            Ink.Runtime.Object value = globalVariablesStory.variablesState.GetVariableWithName(name);
            Variables.Add(name, value);
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

    public void VariableChanged(string name, Ink.Runtime.Object value)
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

    public void SetQuestVariable(string name, QuestState state)
    {
        if (Variables.ContainsKey(name))
        {
            Variables.Remove(name);

            string enumValueString = state.ToString();
            globalVariablesStory.variablesState[name] = enumValueString;
            Variables.Add(name, new Ink.Runtime.StringValue(enumValueString));
        }
        else
        {
            Debug.LogWarning("Variable not found: " + name);
        }
    }

}
