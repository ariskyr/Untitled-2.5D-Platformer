using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(AwarenessSystem))]
public class SensorStats : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI FeedbackDisplay;

    public bool DebugMode = false;

    [SerializeField] private float _VisionConeAngle = 45f;
    [SerializeField] private float _VisionConeRange = 10f;
    [SerializeField] private Color _VisionConeColour = new(1f, 0f, 0f, 0.25f);

    [SerializeField] private float _ProximityDetectionRange = 3f;
    [SerializeField] private Color _ProximityDetectionColour = new(1f, 1f, 1f, 0.25f);

    public Vector3 EyeLocation => transform.position;
    [HideInInspector] public Vector3 EyeDirection = Vector3.right;

    public float VisionConeAngle => _VisionConeAngle;
    public float VisionConeRange => _VisionConeRange;
    public Color VisionConeColour => _VisionConeColour;

    public float ProximityDetectionRange => _ProximityDetectionRange;
    public Color ProximityDetectionColour => _ProximityDetectionColour;

    public float CosVisionConeAngle { get; private set; } = 0f;
    AwarenessSystem awarenessSystem;
    private void Awake()
    {
        //cache the cos to avoid the operation in update
        CosVisionConeAngle = Mathf.Cos(_VisionConeAngle * Mathf.Deg2Rad);
        awarenessSystem = GetComponent<AwarenessSystem>();
    }

    public void ReportCanSee(DetectableTarget seen)
    {
        awarenessSystem.ReportCanSee(seen);
    }

    public void ReportInProximity(DetectableTarget target)
    {
        awarenessSystem.ReportInProximity(target);
    }

    public void OnSuspicious()
    {
        if (!DebugMode)
            return;
        FeedbackDisplay.text = "I am suspicious";
    }

    public void OnDetected(GameObject target)
    {
        if (!DebugMode)
            return;
        FeedbackDisplay.text = "I see you " + target.name;
    }

    public void OnFullyDetected(GameObject target)
    {
        if (!DebugMode)
            return;
        FeedbackDisplay.text = "I am coming for " + target.name;
    }

    public void OnLostDetect(GameObject target)
    {
        if (!DebugMode)
            return;
        FeedbackDisplay.text = "I lost sight of " + target.name;
    }

    public void OnLostSuspicion()
    {
        if (!DebugMode)
            return;
        FeedbackDisplay.text = "I lost suspicion";
    }

    public void OnFullyLost()
    {
        if (!DebugMode)
            return;
        FeedbackDisplay.text = "Must be nothing";
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(SensorStats))]
public class SensorStatsEditorr : Editor
{
    public void OnSceneGUI()
    {
        var sensor = target as SensorStats;

        if (!sensor.DebugMode)
            return;

        //Draw the detection range
        Handles.color = sensor.ProximityDetectionColour;
        Handles.DrawSolidDisc(sensor.transform.position, Vector3.up, sensor.ProximityDetectionRange);

        Quaternion coneRotation = Quaternion.LookRotation(sensor.EyeDirection);
        Quaternion correctedRotation = Quaternion.AngleAxis(-(sensor.VisionConeAngle/2), Vector3.up);
        Vector3 startPoint = coneRotation * correctedRotation * sensor.transform.forward;

        //draw the vision cone
        Handles.color = sensor.VisionConeColour;
        Handles.DrawSolidArc(sensor.transform.position, Vector3.up, startPoint, sensor.VisionConeAngle, sensor.VisionConeRange);
    }
}
#endif