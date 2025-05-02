using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class ObjectBManager : MonoBehaviour
{
    TrailRenderer trailRenderer;

    private void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();
    }

    public void ToggleTrailVisibility()
    {
        trailRenderer.enabled = !trailRenderer.enabled;
    }
}
