using System;
using UnityEngine;
using UnityEngine.Serialization;
using static UnityEngine.GraphicsBuffer;

public class ManageColor : MonoBehaviour
{
    private Transform target; 
    private Renderer targetRenderer;

    private void Update()
    {
        if (target == null || targetRenderer == null)
            return;

        // Direction from Object A to Object B
        Vector3 directionToB = (target.position - transform.position).normalized;

        // Forward direction of Object A
        Vector3 forward = transform.forward;

        // Dot product gives a value from -1 (behind) to +1 (in front)
        float dot = Vector3.Dot(forward, directionToB);

        // Normalize to 0..1 (0 = behind, 1 = front)
        float t = (dot + 1f) / 2f;

        // Lerp from blue (behind) to red (front)
        Color color = Color.Lerp(Color.blue, Color.red, t);

        // Apply the color to the material
        targetRenderer.material.color = color;
    }

    public void Init(Transform newTarget)
    {
        target = newTarget;
        targetRenderer = GetComponent<MeshRenderer>();
    }
}