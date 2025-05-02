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

    [Header("Sliders")]
    [SerializeField] private Slider sliderA;
    [SerializeField] private Slider sliderAa;
    [SerializeField] private Slider sliderDelta;
    [SerializeField] private Slider sliderB;
    [SerializeField] private Slider sliderBb;
    [SerializeField] private Slider sliderSpeed;

    float t = 0;
    Vector3 initialPos;

    void Start()
    {
        // Checks if there is a Trail Renderer component attached to this gameobject.
        // If not, it will attach one, and set values for the relevant fields
        CheckTrailRenderer();
        initialPos = transform.position;

        // Set the sliders to the current values of A, B, aa, bb, delta, and speed
        sliderA.value = A;
        sliderAa.value = aa;
        sliderDelta.value = delta;
        sliderB.value = B;
        sliderBb.value = bb;
        sliderSpeed.value = speed;
    }

    void Update()
    {
        t += Time.deltaTime * speed;

        // Calculate positions for Object A using Lissajous formula (XY plane only)
        float x = A * Mathf.Sin(aa * t + delta);
        float y = B * Mathf.Sin(bb * t);
        
        transform.position = new Vector3(initialPos.x + x, initialPos.y + y, initialPos.z);
    }

    public void ResetObjectInitialPosition()
    {
        initialPos = transform.position;
        enabled = true;
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
    public void SetA(float value)
    {
        A = value;
    }

    public void SetB(float value)
    {
        B = value;
    }

    public void SetAa(float value)
    {
        aa = value;
    }

    public void SetBb(float value)
    {
        bb = value;
    }

    public void SetDelta(float value)
    {
        delta = value;
    }

    public void SetSpeed(float value)
    {
        speed = value;
    }
}