using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera _camera;
    public Transform TargetToFollow;
    public float zoomSpeed = 4f;
    public float minZoom = 10f;
    public float maxZoom = 35f;
    public float zoom;
    public float zoomEasing = 10f;
    public float followSpeed = 2f;

    void Start()
    {
        _camera = GetComponent<Camera>();
    }
    private void FixedUpdate()
    {
        zoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        zoom = Mathf.Clamp(zoom, minZoom, maxZoom);

        _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, zoom, Time.deltaTime * zoomEasing);
        if (TargetToFollow != null)
        {
            Vector2 camera = transform.position;
            Vector2 target = TargetToFollow.position;
            Vector2 NewPos = Vector2.Lerp(camera, target, Time.deltaTime * followSpeed * Vector2.Distance(camera, target));
            transform.position = new Vector3(NewPos.x, NewPos.y, transform.position.z);

        }
    }
}