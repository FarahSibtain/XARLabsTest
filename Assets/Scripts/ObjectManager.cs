using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    [SerializeField] Transform target; // Target to rotate towards

    private void Start()
    {
        ProceduralMeshGenerator1 proceduralMeshGenerator = transform.GetComponent<ProceduralMeshGenerator1>();
        LissajousCurve lissajousCurve = transform.GetComponent<LissajousCurve>();

        lissajousCurve.enabled = false; // Disable the LissajousCurve script to prevent movement
        proceduralMeshGenerator.GenerateMesh();
        lissajousCurve.enabled = true; // Re-enable the LissajousCurve script

        RotateTowardsTarget rotateTowardsTarget = transform.GetComponent<RotateTowardsTarget>();
        rotateTowardsTarget.SetTarget(target); // Set the target for rotation

        ManageColor manageColor = transform.GetComponent<ManageColor>();
        manageColor.Init(target);  

        PerlinNoiseVertexAnimator perlinNoise = transform.GetComponent<PerlinNoiseVertexAnimator>();
        if (perlinNoise != null)
        {
            Mesh mesh = GetComponent<MeshFilter>().mesh;
            perlinNoise.Init(mesh); // Initialize Perlin noise with the mesh
        }
    }
}
