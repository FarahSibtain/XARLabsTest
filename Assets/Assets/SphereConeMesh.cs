using UnityEngine;

public class SphereConeMesh : MonoBehaviour
{
    public float sphereRadius = 1.0f;
    public float coneHeight = 1.5f;
    public float coneRadius = 0.5f;
    public int segments = 24;

    void Start()
    {
        CreateMesh();
    }

    void CreateMesh()
    {
        // Create GameObject
        GameObject objectA = new GameObject("Object A");
        
        // Add mesh components
        MeshFilter meshFilter = objectA.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = objectA.AddComponent<MeshRenderer>();
        
        // Create mesh
        Mesh mesh = new Mesh();
        mesh.name = "SphereConeMesh";
        
        // Generate vertices for sphere
        int sphereVertCount = (segments + 1) * segments; // Latitude x Longitude
        int coneVertCount = segments + 1; // Base circle + tip
        int totalVerts = sphereVertCount + coneVertCount;
        
        Vector3[] vertices = new Vector3[totalVerts];
        Vector3[] normals = new Vector3[totalVerts];
        Vector2[] uvs = new Vector2[totalVerts];
        
        // Generate sphere vertices
        int v = 0;
        for (int lat = 0; lat < segments; lat++)
        {
            float theta = lat * Mathf.PI / segments;
            float sinTheta = Mathf.Sin(theta);
            float cosTheta = Mathf.Cos(theta);
            
            for (int lon = 0; lon <= segments; lon++)
            {
                float phi = lon * 2 * Mathf.PI / segments;
                float sinPhi = Mathf.Sin(phi);
                float cosPhi = Mathf.Cos(phi);
                
                float x = cosPhi * sinTheta;
                float y = cosTheta; 
                float z = sinPhi * sinTheta;
                
                vertices[v] = new Vector3(x, y, z) * sphereRadius;
                normals[v] = new Vector3(x, y, z);
                uvs[v] = new Vector2((float)lon / segments, (float)lat / segments);
                v++;
            }
        }
        
        // Generate cone vertices (reusing front sphere vertices for connection)
        Vector3 coneTip = new Vector3(0, 0, sphereRadius + coneHeight);
        vertices[v] = coneTip;
        normals[v] = Vector3.forward;
        uvs[v] = new Vector2(0.5f, 0.5f);
        int tipIndex = v++;
        
        // Add cone base vertices (overlapping with sphere)
        for (int i = 0; i <= segments; i++)
        {
            float angle = i * 2 * Mathf.PI / segments;
            float x = coneRadius * Mathf.Cos(angle);
            float y = coneRadius * Mathf.Sin(angle);
            
            vertices[v] = new Vector3(x, y, sphereRadius);
            
            // Calculate normal (pointing outward from cone)
            Vector3 basePoint = vertices[v];
            Vector3 toTip = coneTip - basePoint;
            Vector3 tangent = new Vector3(-y, x, 0).normalized;
            normals[v] = Vector3.Cross(tangent, toTip).normalized;
            
            uvs[v] = new Vector2((float)i / segments, 0);
            v++;
        }
        
        // Calculate triangles
        int sphereTriCount = (segments - 1) * segments * 2;
        int coneTriCount = segments;
        int totalTriCount = sphereTriCount + coneTriCount;
        int[] triangles = new int[totalTriCount * 3];
        
        int t = 0;
        
        // Sphere triangles
        for (int lat = 0; lat < segments - 1; lat++)
        {
            int rowStart = lat * (segments + 1);
            int nextRowStart = (lat + 1) * (segments + 1);
            
            for (int lon = 0; lon < segments; lon++)
            {
                int current = rowStart + lon;
                int next = current + 1;
                int nextRow = nextRowStart + lon;
                int nextRowNext = nextRow + 1;
                
                // First triangle
                triangles[t++] = current;
                triangles[t++] = nextRowNext;
                triangles[t++] = nextRow;
                
                // Second triangle
                triangles[t++] = current;
                triangles[t++] = next;
                triangles[t++] = nextRowNext;
            }
        }
        
        // Cone triangles
        int coneBaseStart = sphereVertCount + 1;
        for (int i = 0; i < segments; i++)
        {
            triangles[t++] = tipIndex;
            triangles[t++] = coneBaseStart + i;
            triangles[t++] = coneBaseStart + ((i + 1) % (segments + 1));
        }
        
        // Assign to mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;
        mesh.uv = uvs;
        
        mesh.RecalculateBounds();
        
        // Assign to GameObject
        meshFilter.mesh = mesh;
        meshRenderer.material = new Material(Shader.Find("Standard"));
    }
}