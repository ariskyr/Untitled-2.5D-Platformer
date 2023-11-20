using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class  Stat
{
    [SerializeField] private float value = 1;

    public float GetValue()
    {
        return value;
    }

}
