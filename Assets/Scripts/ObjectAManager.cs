using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ObjectAManager : MonoBehaviour
{
    [SerializeField] Transform target; // Target to rotate towards

    private LissajousCurve lissajousCurve;

    private void Start()
    {
        StartCoroutine(InitObject());
    }

    IEnumerator InitObject()
    {
        ProceduralMeshGenerator proceduralMeshGenerator = transform.GetComponent<ProceduralMeshGenerator>();
        lissajousCurve = transform.GetComponent<LissajousCurve>();

        lissajousCurve.enabled = false; // Disable the LissajousCurve script to prevent movement
        proceduralMeshGenerator.GenerateMesh();
        lissajousCurve.enabled = true; // Re-enable the LissajousCurve script

        yield return null;

        RotateTowardsTarget rotateTowardsTarget = transform.GetComponent<RotateTowardsTarget>();
        rotateTowardsTarget.SetTarget(target); // Set the target for rotation

        ManageColor manageColor = transform.GetComponent<ManageColor>();
        manageColor.Init(target);

        yield return StartCoroutine(MakeGrabbable());

        PerlinNoiseVertexAnimator perlinNoise = transform.GetComponent<PerlinNoiseVertexAnimator>();
        if (perlinNoise != null)
        {
            Mesh mesh = GetComponent<MeshFilter>().mesh;
            perlinNoise.Init(mesh); // Initialize Perlin noise with the mesh
        }
    }

    IEnumerator MakeGrabbable()
    {
        Collider col = transform.AddComponent<SphereCollider>();
        Rigidbody rb = transform.AddComponent<Rigidbody>();
        rb.isKinematic = true; // Set to true to prevent physics interactions
        rb.useGravity = false; // Disable gravity

        yield return null;
        XRGrabInteractable grabbable = transform.AddComponent<XRGrabInteractable>();

        // Setting interaction layer
        grabbable.interactionLayers = InteractionLayerMask.GetMask("ObjectA");
        grabbable.throwOnDetach = false; 
        // Hook into grab events
        grabbable.selectEntered.AddListener(OnGrabbed);
        grabbable.selectExited.AddListener(OnReleased);

        yield return null;
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        lissajousCurve.enabled = false; 
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        lissajousCurve.enabled = true;
    }

    public void ToggleTrailVisibility()
    {
        TrailRenderer trailRenderer = GetComponent<TrailRenderer>();
        if (trailRenderer == null)
        {
            Debug.LogWarning("TrailRenderer component not found.");
            return;
        }
        trailRenderer.enabled = !trailRenderer.enabled;
    }
}
