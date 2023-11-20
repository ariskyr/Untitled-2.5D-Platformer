using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeStats : CharacterStats
{
    public override void Die()
    {
        base.Die();
        GetComponent<BoxCollider>().enabled = false;
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
