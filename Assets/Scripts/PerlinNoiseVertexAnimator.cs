using UnityEngine;

public class PerlinNoiseVertexAnimator : MonoBehaviour
{
    [SerializeField] float noiseScale = 1.0f;
    [SerializeField] float displacementAmplitude = 0.2f;
    [SerializeField] float animationSpeed = 1.0f;

    private Mesh deformingMesh;
    private Vector3[] originalVertices;
    private Vector3[] displacedVertices;
    private Vector3[] normals;

    public void Init(Mesh _mesh)
    {
        deformingMesh = _mesh;

        // Save original vertex positions and normals
        originalVertices = deformingMesh.vertices;
        normals = deformingMesh.normals;
        displacedVertices = new Vector3[originalVertices.Length];
    }

    private void Update()
    {
        if (deformingMesh == null)
            return;

        float time = Time.time * animationSpeed;

        for (int i = 0; i < originalVertices.Length; i++)
        {
            Vector3 vertex = originalVertices[i];
            Vector3 normal = normals[i];

            // Create a unique noise input per vertex based on position and time
            float noise = Mathf.PerlinNoise(vertex.x * noiseScale + time, vertex.y * noiseScale + time);

            // Displace the vertex along its normal
            displacedVertices[i] = vertex + displacementAmplitude * noise * normal;
        }

        deformingMesh.vertices = displacedVertices;
        deformingMesh.RecalculateNormals();
        deformingMesh.RecalculateBounds();
    }
}