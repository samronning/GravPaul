using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraShake : cameraController {

    //Camera Shake
    public float power = 0.7f;
    public float duration = 1.0f;
    public new Transform camera;
    public static bool shouldShake = false;
    public float slowDownAmount = 1.0f;

    float initialDuration;

    void Start()
    {
        camera = Camera.main.transform;
        initialDuration = duration;
    }

    void FixedUpdate()
    {
        if (shouldShake)
        {
            if (duration > 0)
            {
                camera.localPosition += Random.insideUnitSphere * power;
                duration -= Time.deltaTime * slowDownAmount;
            }
            else
            {
                shouldShake = false;
                duration = initialDuration;
            }
        }
    }
}
