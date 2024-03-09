using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int goldGained = 1;

    private SphereCollider sphereCollider;
    private SpriteRenderer visual;

    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
        visual = GetComponent<SpriteRenderer>();
    }

    private void CollectCoin()
    {
        sphereCollider.enabled = false;
        visual.gameObject.SetActive(false);
        GameEventsManager.Instance.playerEvents.GoldGained(goldGained);
        GameEventsManager.Instance.miscEvents.CoinCollected();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            CollectCoin();
    }
}
