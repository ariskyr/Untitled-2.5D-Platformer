using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
public class CharacterCombat : MonoBehaviour
{
    private CharacterStats myStats;
    // Start is called before the first frame update
    void Start()
    {
        myStats = GetComponent<CharacterStats>();
    }

    void Update()
    {
        myStats.attackCooldown -= Time.deltaTime;
    }

    public void Attack(CharacterStats targetStats)
    {
        if (myStats.attackCooldown <= 0)
        {
            targetStats.TakeDamage(myStats.attackDamage.GetValue());
            myStats.attackCooldown = 1 / myStats.attackSpeed.GetValue();
        }
    }
}
