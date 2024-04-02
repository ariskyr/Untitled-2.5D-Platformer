using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : Hittable
{
   
    //this variable should be in the player class as well
    public bool Invincible { get; set; }

    public event Action OnDestroyed;

    private void Update()
    {
        //Debug.Log("current health: " + CurrentHealth);
    }

    protected override void Awake()
    {
        base.Awake();
        Invincible = false;
    }

    public override void OnAttackHit(Vector2 position, Vector2 force, int damage)
    {
        if (Player.Instance.CurrentHealth <= 0 || Invincible)
            return;

        DealDamage(damage);

        base.OnAttackHit(position, force, damage);
    }

    public void DealDamage(int damage)
    {
        GameEventsManager.Instance.playerEvents.HealthLost(damage);
        //this should probably be in the player class?
        if (Player.Instance.CurrentHealth <= 0)
        {
            OnDestroyed?.Invoke();
        }
    }
}
