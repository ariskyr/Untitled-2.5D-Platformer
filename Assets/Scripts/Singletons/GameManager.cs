using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//DERIVE FROM GENERIC SINGLETON
public class GameManager : GenericSingleton<GameManager>
{
    [SerializeField] private string[] scenesWithCrossfade;
}
