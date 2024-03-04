using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(SensorStats))]
public class VisionSensor : MonoBehaviour
{
    SensorStats _Stats;

    [SerializeField] LayerMask DetectionMask = ~0;

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

            var vectorToTarget = candidateTarget.transform.position - _Stats.EyeLocation;

            //if out of range - cannot see
            if (vectorToTarget.sqrMagnitude > (_Stats.VisionConeRange * _Stats.VisionConeRange))
                continue;

            vectorToTarget.Normalize();

            //if out of vision cone - cannot see
            if (Vector3.Dot(vectorToTarget, _Stats.EyeDirection) < _Stats.CosVisionConeAngle)
                continue;

            //raycast is done last because its an expensive operation
            RaycastHit hitResult;
            if (Physics.Raycast(_Stats.EyeLocation, vectorToTarget, out hitResult, 
                _Stats.VisionConeRange, DetectionMask, QueryTriggerInteraction.Collide))
            {
                if (hitResult.collider.GetComponentInParent<DetectableTarget>() == candidateTarget)
                    _Stats.ReportCanSee(candidateTarget);
            }
        }
    }
}


