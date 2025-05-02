using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ObjectManager : MonoBehaviour
{
    [SerializeField] Transform target; // Target to rotate towards

    private LissajousCurve lissajousCurve;
    private RotateTowardsTarget rotateTowardsTarget;
    private ManageColor manageColor;

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

        rotateTowardsTarget = transform.GetComponent<RotateTowardsTarget>();
        rotateTowardsTarget.SetTarget(target); // Set the target for rotation

        manageColor = transform.GetComponent<ManageColor>();
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
        //Now make this object a grabbable object
        Collider col = transform.AddComponent<SphereCollider>();
        Rigidbody rb = transform.AddComponent<Rigidbody>();
        rb.isKinematic = true; // Set to true to prevent physics interactions
        rb.useGravity = false; // Disable gravity

        yield return null;
        XRGrabInteractable grabbable = transform.AddComponent<XRGrabInteractable>();

        // Setting interaction layers for 3 layers
        grabbable.interactionLayers = InteractionLayerMask.GetMask("ObjectA"); // Set the interaction layer

        // Hook into grab events
        grabbable.selectEntered.AddListener(OnGrabbed);
        grabbable.selectExited.AddListener(OnReleased);

        yield return null;
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        lissajousCurve.enabled = false; // Disable the LissajousCurve script to stop movement
        rotateTowardsTarget.enabled = false; // Disable rotation
        manageColor.enabled = false; // Disable color management
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        lissajousCurve.enabled = true; 
        rotateTowardsTarget.enabled = true; 
        manageColor.enabled = true; 
    }
}
