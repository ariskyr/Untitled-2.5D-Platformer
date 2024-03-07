using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventsManager : GenericSingleton<GameEventsManager>
{
    public PlayerEvents playerEvents;

    protected override void Awake()
    {
        base.Awake();
        //init
        playerEvents = new PlayerEvents();
    }
}
