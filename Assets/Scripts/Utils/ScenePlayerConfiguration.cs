using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScenePlayerConfiguration", menuName = "Game Configuration/Scene Player Configuration", order = 1)]
public class ScenePlayerConfiguration : ScriptableObject
{
    [Header("Scene Configuration")]
    [Tooltip("Add the exact names of scenes that should use the Player2D controller.")]
    public List<string> scenesUsingPlayer2D;

}
