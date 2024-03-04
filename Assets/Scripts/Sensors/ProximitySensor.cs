using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SensorStats))]
public class ProximitySensor : MonoBehaviour
{
    SensorStats _Stats;
    // Start is called before the first frame update
    void Start()
    {
        _Stats = GetComponent<SensorStats>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int index = 0; index < DetectableTargetManager.Instance.AllTargets.Count; ++index)
        {
            var candidateTarget = DetectableTargetManager.Instance.AllTargets[index];

            //skip if the target is ourselves
            if (candidateTarget.gameObject == gameObject)
                continue;

            if (Vector3.Distance(transform.position, candidateTarget.transform.position) <= _Stats.ProximityDetectionRange)
                _Stats.ReportInProximity(candidateTarget);
        }   
    }
}
