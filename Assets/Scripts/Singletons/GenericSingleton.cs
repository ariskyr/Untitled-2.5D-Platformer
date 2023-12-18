using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericSingleton<T>: MonoBehaviour where T : Component
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                //find generic instance
                instance = FindObjectOfType<T>();

                //if doesnt exist, create and attach
                if (instance == null)
                {
                    GameObject obj = new()
                    {
                        name = typeof(T).Name
                    };
                    instance = obj.AddComponent<T>();
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
            Debug.Log("Initialized SINGLETON object: " + gameObject.name);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
