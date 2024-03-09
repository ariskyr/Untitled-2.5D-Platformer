using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventsManager : GenericSingleton<GameEventsManager>
{
    public PlayerEvents playerEvents;
    public MiscEvents miscEvents;

    protected override void Awake()
    {
        base.Awake();
        //init
        playerEvents = new PlayerEvents();
        miscEvents = new MiscEvents();
    }
}
