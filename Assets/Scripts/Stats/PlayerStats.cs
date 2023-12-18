using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : CharacterStats
{
    public override void Die()
    {
        base.Die();
        currentHealth = maxHealth;
        transform.position = new Vector3(0,0.5f,0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
