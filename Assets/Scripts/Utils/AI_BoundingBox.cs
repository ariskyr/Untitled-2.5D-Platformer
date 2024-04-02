using UnityEditor.Rendering;
using UnityEngine;

public class AI_BoundingBox : MonoBehaviour
{
    //This is a box collider that is used to determine the AI's bounding box.
    //Use the box collider edit to configure it on the area you want the AI to navigate.

    [SerializeField] private bool DebugMode = false;
    [SerializeField] private BBColor selectedcolor = BBColor.Red;
    [SerializeField] private BoxCollider boxCollider;
    public Vector3 BoundsMin { get; private set; }
    public Vector3 BoundsMax { get; private set; }

   private enum BBColor
    {
        Red,
        Green,
        Blue
    }

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        BoundsMin = boxCollider.bounds.min;
        BoundsMax = boxCollider.bounds.max;
    }

    private void OnDrawGizmosSelected()
    {
        if (!DebugMode) return;

        switch
            (selectedcolor)
        {
            case BBColor.Red:
                Gizmos.color = new Color(1, 0, 0, 0.25f);
                break;
            case BBColor.Green:
                Gizmos.color = new Color(0, 1, 0, 0.25f);
                break;
            case BBColor.Blue:
                Gizmos.color = new Color(0, 0, 1, 0.25f);
                break;
        }

        Gizmos.DrawCube(boxCollider.bounds.center, boxCollider.bounds.size);
    }
}
