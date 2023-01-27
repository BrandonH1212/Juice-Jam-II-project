using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera _camera;

    public Transform TargetToFollow;

    public float Zoom
    {
        get => _camera != null ? _camera.orthographicSize : 0f;
        private set => _camera.orthographicSize = Mathf.Clamp(value, 5, 50);
    }

    // Start is called before the first frame update
    void Start()
    {
        _camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Zoom += Input.GetAxis("Mouse ScrollWheel") * -10;
        if (TargetToFollow != null) 
        {
            Vector3 playerPosition = TargetToFollow.transform.localPosition;
            transform.localPosition = new Vector3(playerPosition.x, playerPosition.y, transform.localPosition.z);
        } 
    }
}
