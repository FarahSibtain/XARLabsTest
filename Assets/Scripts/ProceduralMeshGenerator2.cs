
using System.Collections;
using UnityEngine;

public class ProceduralMeshGenerator : MonoBehaviour
{
    [Header("Sphere Parameters")]
    [SerializeField] float sphereRadius = 1.0f;
    [SerializeField] int sphereSegments = 24;
    [SerializeField] int sphereRings = 12;

    [Header("Cone Parameters")]
    [SerializeField] float coneHeight = 1.5f;
    [SerializeField] float coneRadius = 0.5f;
    [SerializeField] int coneSegments = 24;

    Mesh mesh;
    public void GenerateMesh()
    {
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();

        mesh = new Mesh();
        mesh.name = "SphereWithCone";
        meshFilter.mesh = mesh;

        GenerateSphereWithCone(mesh);

        meshRenderer.material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
    }

    private void GenerateSphereWithCone(Mesh mesh)
    {
        int sphereVertexCount = (sphereRings + 1) * (sphereSegments + 1);
        int totalVertexCount = sphereVertexCount + 1; // +1 for cone tip
        int sphereTriangleCount = sphereRings * sphereSegments * 2;
        int coneTriangleCount = coneSegments;
        int totalTriangleCount = sphereTriangleCount + coneTriangleCount;

        Vector3[] vertices = new Vector3[totalVertexCount];
        Vector3[] normals = new Vector3[totalVertexCount];
        Vector2[] uvs = new Vector2[totalVertexCount];
        int[] triangles = new int[totalTriangleCount * 3];

        int vertexIndex = 0;
        int triangleIndex = 0;
        int[] coneBaseRingIndices = new int[coneSegments + 1];

        GenerateSphere(vertices, normals, uvs, triangles, ref vertexIndex, ref triangleIndex, coneBaseRingIndices);
        GenerateCone(vertices, normals, uvs, triangles, ref vertexIndex, ref triangleIndex, coneBaseRingIndices);

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;
        mesh.uv = uvs;
        mesh.RecalculateBounds();
    }

    private void GenerateSphere(Vector3[] vertices, Vector3[] normals, Vector2[] uvs, int[] triangles, ref int vertexIndex, ref int triangleIndex, int[] coneBaseRingIndices)
    {
        int startIndex = vertexIndex;
        int frontRingIndex = sphereRings / 2; // Approximate "front" ring

        for (int ring = 0; ring <= sphereRings; ring++)
        {
            float phi = Mathf.PI * (float)ring / sphereRings;
            for (int segment = 0; segment <= sphereSegments; segment++)
            {
                float theta = 2.0f * Mathf.PI * (float)segment / sphereSegments;
                float x = Mathf.Sin(phi) * Mathf.Cos(theta);
                float y = Mathf.Cos(phi);
                float z = Mathf.Sin(phi) * Mathf.Sin(theta);
                Vector3 normal = new Vector3(x, y, z).normalized;
                Vector3 position = normal * sphereRadius;

                vertices[vertexIndex] = position;
                normals[vertexIndex] = normal;
                uvs[vertexIndex] = new Vector2((float)segment / sphereSegments, (float)ring / sphereRings);

                if (ring == frontRingIndex && segment <= coneSegments)
                {
                    coneBaseRingIndices[segment] = vertexIndex;
                }

                if (ring < sphereRings && segment < sphereSegments)
                {
                    int current = vertexIndex;
                    int next = current + 1;
                    int nextRing = current + sphereSegments + 1;
                    int nextRingNext = nextRing + 1;

                    triangles[triangleIndex++] = current;
                    triangles[triangleIndex++] = nextRingNext;
                    triangles[triangleIndex++] = nextRing;

                    triangles[triangleIndex++] = current;
                    triangles[triangleIndex++] = next;
                    triangles[triangleIndex++] = nextRingNext;
                }

                vertexIndex++;
            }
        }
    }

    private void GenerateCone(Vector3[] vertices, Vector3[] normals, Vector2[] uvs, int[] triangles, ref int vertexIndex, ref int triangleIndex, int[] coneBaseRingIndices)
    {
        Vector3 coneTip = vertices[coneBaseRingIndices[0]] + new Vector3(0, 0, coneHeight);
        vertices[vertexIndex] = coneTip;
        normals[vertexIndex] = new Vector3(0, 0, 1);
        uvs[vertexIndex] = new Vector2(0.5f, 0.5f);
        int coneTipIndex = vertexIndex;
        vertexIndex++;

        for (int i = 0; i < coneSegments; i++)
        {
            int base0 = coneBaseRingIndices[i];
            int base1 = coneBaseRingIndices[i + 1];

            triangles[triangleIndex++] = coneTipIndex;
            triangles[triangleIndex++] = base0;
            triangles[triangleIndex++] = base1;
        }
    }
}
