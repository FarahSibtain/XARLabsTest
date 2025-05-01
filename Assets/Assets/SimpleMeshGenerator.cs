using UnityEngine;

public class SimpleMeshGenerator : MonoBehaviour
{
    void Start()
    {
        GameObject objectA = new GameObject("Object A");
        GenerateMesh(objectA);
    }
    
    void GenerateMesh(GameObject targetObject)
    {
        MeshFilter meshFilter = targetObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = targetObject.AddComponent<MeshRenderer>();
        
        Mesh mesh = new Mesh();
        
        // Create a simple mesh combining sphere and cone
        CreateSphereWithCone(mesh);
        
        meshFilter.mesh = mesh;
        meshRenderer.material = new Material(Shader.Find("Standard"));
    }
    
    void CreateSphereWithCone(Mesh mesh)
    {
        // Simple implementation combining sphere and cone
        mesh.vertices = new Vector3[]
        {
            // Sphere part (simplified)
            new Vector3(0, 0, 0),       // Center
            new Vector3(1, 0, 0),       // Right
            new Vector3(-1, 0, 0),      // Left
            new Vector3(0, 1, 0),       // Top
            new Vector3(0, -1, 0),      // Bottom
            new Vector3(0, 0, 1),       // Front (connection point)
            
            // Cone tip
            new Vector3(0, 0, 2.5f)     // Tip
        };
        
        mesh.triangles = new int[]
        {
            // Sphere triangles
            0, 1, 3,  // Right-top face
            0, 3, 2,  // Left-top face
            0, 2, 4,  // Left-bottom face
            0, 4, 1,  // Right-bottom face
            1, 5, 3,  // Right-front face
            3, 5, 2,  // Top-front face
            2, 5, 4,  // Left-front face
            4, 5, 1,  // Bottom-front face
            
            // Cone triangles
            5, 1, 6,  // Front-right-tip
            5, 6, 3,  // Front-tip-top
            5, 2, 6,  // Front-left-tip
            5, 6, 4   // Front-tip-bottom
        };
        
        mesh.RecalculateNormals();
    }
}