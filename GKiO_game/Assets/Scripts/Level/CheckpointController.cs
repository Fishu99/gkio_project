using UnityEngine;

/// <summary>
/// A script for checkpoints.
/// It only displayes blue gismos in scene view.
/// </summary>
public class CheckpointController : MonoBehaviour
{
    [SerializeField] private Color gizmoColor = new Color(0, 0, 1, 0.3f);

    private void OnDrawGizmos()
    {
        var collider = GetComponent<Collider>();
        Gizmos.color = gizmoColor;
        Gizmos.DrawCube(collider.bounds.center, collider.bounds.size);
    }
}
