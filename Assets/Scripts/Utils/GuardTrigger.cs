using UnityEngine;
using System.Collections.Generic;

public class GuardTrigger : MonoBehaviour
{
    private Chest _parentChest;
    private HashSet<Collider> _currentNPCs = new HashSet<Collider>();

    private void Awake()
    {
        _parentChest = GetComponentInParent<Chest>();
        gameObject.tag = "Guardable";
    }

    private void Update()
    {
        // Check for destroyed NPCs every frame
        List<Collider> toRemove = new List<Collider>();

        foreach (Collider npcCollider in _currentNPCs)
        {
            if (npcCollider == null || !npcCollider.gameObject.activeInHierarchy)
            {
                toRemove.Add(npcCollider);
                _parentChest?.NPCExited();
            }
        }

        foreach (Collider removed in toRemove)
        {
            _currentNPCs.Remove(removed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && _parentChest != null)
        {
            if (_currentNPCs.Add(other))
            {
                _parentChest.NPCEntered();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") && _parentChest != null)
        {
            if (_currentNPCs.Remove(other))
            {
                _parentChest.NPCExited();
            }
        }
    }
}