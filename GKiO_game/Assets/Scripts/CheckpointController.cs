using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    [SerializeField] private Color gizmoColor = new Color(0, 0, 1, 0.3f);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        var collider = GetComponent<Collider>();
        Gizmos.color = gizmoColor;
        Gizmos.DrawCube(collider.bounds.center, collider.bounds.size);
    }
}
