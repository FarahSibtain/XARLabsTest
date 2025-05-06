using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(MeshFilter)), RequireComponent(typeof(MeshRenderer))]
public class ProceduralMeshGenerator : MonoBehaviour
{
    [SerializeField] private Material ObjectAMat;

    public void GenerateMesh()
    {
        MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();

        // Assign the mesh to the mesh filter
        meshFilter.mesh = GenerateSphereWithCone();

        // Assign materials
        meshRenderer.material = ObjectAMat;
        transform.localScale = new Vector3(0.05f, 0.05f, 0.05f); 
    }

    Mesh GenerateSphereWithCone()
    {
        Mesh mesh = new Mesh
        {
            name = "SphereWithCone"
        };

        // Create sphere and cone meshes
        Mesh sphereMesh = GenerateSphereMesh(1f, 24, 12); 
        Mesh coneMesh = GenerateConeMesh(0.6f, 0.9f, 12);

        // Combine sphere and cone vertices
        Vector3[] combinedVertices = new Vector3[sphereMesh.vertexCount + coneMesh.vertexCount];
        Vector2[] combinedUVs = new Vector2[sphereMesh.vertexCount + coneMesh.vertexCount];
        Vector3[] combinedNormals = new Vector3[sphereMesh.vertexCount + coneMesh.vertexCount]; // Store normals
        int[] combinedTriangles = new int[sphereMesh.triangles.Length + coneMesh.triangles.Length];

        // Copy sphere vertices, UVs, and triangles
        sphereMesh.vertices.CopyTo(combinedVertices, 0);
        sphereMesh.uv.CopyTo(combinedUVs, 0);
        sphereMesh.normals.CopyTo(combinedNormals, 0); 
        sphereMesh.triangles.CopyTo(combinedTriangles, 0);

        // Copy cone vertices, adjust their position, and UVs
        for (int i = 0; i < coneMesh.vertexCount; i++)
        {
            Vector3 v = coneMesh.vertices[i];
            Vector3 rotated = new Vector3(v.x, -v.z, v.y); // Rotate -90° around X

            // Embed cone slightly into the sphere
            Vector3 displaced = rotated + new Vector3(0, 0, 0.95f);
            combinedVertices[sphereMesh.vertexCount + i] = displaced;

            // Calculate averaged normal for smooth lighting at base
            Vector3 sphereNormal = v.normalized; // Assume sphere center at origin
            Vector3 coneNormal = (coneMesh.normals[i].y * Vector3.forward + coneMesh.normals[i].x * Vector3.right - coneMesh.normals[i].z * Vector3.up).normalized;
            Vector3 smoothNormal = (sphereNormal + coneNormal).normalized;
            combinedNormals[sphereMesh.vertexCount + i] = smoothNormal; // Store calculated normals

            combinedUVs[sphereMesh.vertexCount + i] = coneMesh.uv[i]; // Copy UVs
        }

        // Copy cone triangles and adjust indices
        for (int i = 0; i < coneMesh.triangles.Length; i++)
        {
            combinedTriangles[sphereMesh.triangles.Length + i] = coneMesh.triangles[i] + sphereMesh.vertexCount;
        }

        // Assign combined data to the mesh
        mesh.vertices = combinedVertices;
        mesh.triangles = combinedTriangles;
        mesh.uv = combinedUVs;
        mesh.normals = combinedNormals; // Assign pre-calculated normals instead of recalculating

        // recalculate bounds
        mesh.RecalculateBounds();
        Debug.Log($"ProceduralMeshGenerator: Generated mesh with {mesh.vertexCount} vertices.");

        return mesh;
    }

    Mesh GenerateSphereMesh(float radius, int segments, int rings)
    {
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[(segments + 1) * (rings + 1)];
        Vector2[] uvs = new Vector2[vertices.Length];
        Vector3[] normals = new Vector3[vertices.Length];
        int[] triangles = new int[segments * rings * 6];

        // Generate vertices, normals and UVs
        for (int lat = 0; lat <= rings; lat++)
        {
            float theta = lat * Mathf.PI / rings;
            float v = (float)lat / rings;

            for (int lon = 0; lon <= segments; lon++)
            {
                float phi = lon * 2 * Mathf.PI / segments;
                float u = (float)lon / segments;
                int index = lat * (segments + 1) + lon;

                // Calculate position
                float x = radius * Mathf.Sin(theta) * Mathf.Cos(phi);
                float y = radius * Mathf.Cos(theta);
                float z = radius * Mathf.Sin(theta) * Mathf.Sin(phi);

                vertices[index] = new Vector3(x, y, z);

                // Normal is just the normalized position for a sphere
                normals[index] = new Vector3(x, y, z).normalized;

                // Improved UV mapping to reduce distortion at poles
                uvs[index] = new Vector2(u, v);
            }
        }

        // Generate triangles
        int triIndex = 0;
        for (int lat = 0; lat < rings; lat++)
        {
            for (int lon = 0; lon < segments; lon++)
            {
                int current = lat * (segments + 1) + lon;
                int next = current + segments + 1;

                // Standard quad triangulation
                triangles[triIndex++] = current;
                triangles[triIndex++] = current + 1;
                triangles[triIndex++] = next;

                triangles[triIndex++] = next;
                triangles[triIndex++] = current + 1;
                triangles[triIndex++] = next + 1;
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.normals = normals; // Assign pre-calculated normals
        return mesh;
    }

    Mesh GenerateConeMesh(float radius, float height, int segments)
    {
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[segments + 2];
        Vector2[] uvs = new Vector2[vertices.Length];
        Vector3[] normals = new Vector3[vertices.Length];
        int[] triangles = new int[segments * 3 + segments * 3];

        // Generate vertices, normals and UVs
        vertices[0] = Vector3.zero; // Base center
        normals[0] = Vector3.down; // Base normal points down
        uvs[0] = new Vector2(0.5f, 0.5f); // Center UV

        for (int i = 0; i < segments; i++)
        {
            float angle = i * Mathf.PI * 2 / segments;
            float x = radius * Mathf.Cos(angle);
            float z = radius * Mathf.Sin(angle);

            vertices[i + 1] = new Vector3(x, 0, z);
            normals[i + 1] = Vector3.down; // Base edge normals point down
            uvs[i + 1] = new Vector2((Mathf.Cos(angle) + 1) * 0.5f, (Mathf.Sin(angle) + 1) * 0.5f);
        }

        vertices[segments + 1] = new Vector3(0, height, 0); // Cone tip
        normals[segments + 1] = Vector3.up; // Tip normal points up initially (will be averaged with side normals)
        uvs[segments + 1] = new Vector2(0.5f, 1); // Tip UV

        // Generate triangles for base
        int triIndex = 0;
        for (int i = 0; i < segments; i++)
        {
            triangles[triIndex++] = 0;
            triangles[triIndex++] = i + 1;
            triangles[triIndex++] = (i + 1) % segments + 1;
        }

        // Generate triangles for sides
        for (int i = 0; i < segments; i++)
        {
            triangles[triIndex++] = i + 1;
            triangles[triIndex++] = (i + 1) % segments + 1;
            triangles[triIndex++] = segments + 1;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        // Calculate side normals
        Vector3 tipNormal = Vector3.zero;
        for (int i = 0; i < segments; i++)
        {
            int nextIdx = (i + 1) % segments + 1;
            Vector3 sideNormal = Vector3.Cross(
                vertices[nextIdx] - vertices[i + 1],
                vertices[segments + 1] - vertices[i + 1]
            ).normalized;

            // Update normals for side edges
            normals[i + 1] = Vector3.Lerp(normals[i + 1], sideNormal, 0.5f).normalized;

            // Accumulate for tip normal
            tipNormal += sideNormal;
        }

        // Average and normalize the tip normal
        normals[segments + 1] = tipNormal.normalized;

        mesh.normals = normals; // Assign pre-calculated normals
        return mesh;
    }
}