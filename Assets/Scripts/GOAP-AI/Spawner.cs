using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject patientPrefab;
    [SerializeField] private int initialNumPatients;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < initialNumPatients; i++)
        {
            Instantiate(patientPrefab, this.transform.position, Quaternion.identity);
        }
        Invoke(nameof(SpawnPatient), 5);
    }

    void SpawnPatient()
    {
        Instantiate(patientPrefab, this.transform.position, Quaternion.identity);
        Invoke(nameof(SpawnPatient), Random.Range(2, 10));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
