using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LissajousCurve : MonoBehaviour
{
    [Range(0, 20)] public float A = 4f;
    [Range(0, 20)] public float B = 4f;
    [Range(0, 20)] public float aa = 5f;
    [Range(0, 20)] public float bb = 4f;
    [Range(0, 20)] public float delta = 2.3f;
    [Range(0, 2)] public float speed = 1; // Speed of the animation

    float t = 0;

    //void Start()
    //{
    //    // Checks if there is a Trail Renderer component attached to this gameobject.
    //    // If not, it will attach one, and set values for the relevant fields
    //    CheckTrailRenderer();
    //}

    void Update()
    {
        t += Time.deltaTime * speed;

        // Compute XY motion in local space
        float x = A * Mathf.Sin(aa * t + delta);
        float y = B * Mathf.Sin(bb * t);

        transform.localPosition = new Vector3(x, y, 0);
    }
    
    void CheckTrailRenderer()
    {
        TrailRenderer tr = GetComponent<TrailRenderer>();
        if (tr == null)
        {
            tr = gameObject.AddComponent<TrailRenderer>();
            tr.time = 2;
            tr.materials[0] = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            tr.startWidth = 0.04f;
            tr.endWidth = 0.04f;
        }
    }
}