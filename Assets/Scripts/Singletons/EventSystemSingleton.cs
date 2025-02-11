using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemSingleton : GenericSingleton<EventSystemSingleton>
{
    protected override void Awake()
    {
        base.Awake();

        if (GetComponent<EventSystem>() == null)
        {
            gameObject.AddComponent<EventSystem>();
        }
        if (GetComponent<StandaloneInputModule>() == null)
        {
            gameObject.AddComponent<StandaloneInputModule>();
        }
    }
}