using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateWorld : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI debugText;

    // Update is called once per frame
    void Update()
    {
        Dictionary<string, int> worldStates = GWorld.Instance.GetWorld().GetStates();
        string text = "World States: \n";
        foreach(KeyValuePair<string, int> state in worldStates)
        {
            text += state.Key + ": " + state.Value + "\n";
        }
        debugText.text = text;
    }
}
