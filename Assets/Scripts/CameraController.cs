using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour {

    public float normalFOV, zoomFOV;
    public float lerpVelocity = 0.05f;

    private Camera camera;
    private float axisLerpValue = 0;
    private bool isUpdatingFOV = false;

    void Start()
    {
        camera = GetComponent<Camera>();
        UpdateFOV();
    }

    void Update()
    {
        if(!isUpdatingFOV)
        {
            axisLerpValue = Maths.Lerp(axisLerpValue, 0, lerpVelocity);
            camera.fieldOfView = Mathf.Lerp(normalFOV, zoomFOV, axisLerpValue);
        }

        isUpdatingFOV = false;
    }

    public void ProcessZoomIn(float val)
    {
        isUpdatingFOV = true;
        axisLerpValue = Maths.Lerp(axisLerpValue, val, lerpVelocity);
        camera.fieldOfView = Mathf.Lerp(normalFOV, zoomFOV, axisLerpValue);
    }

    public void UpdateFOV()
    {
        camera.fieldOfView = normalFOV;
    }
	
}
