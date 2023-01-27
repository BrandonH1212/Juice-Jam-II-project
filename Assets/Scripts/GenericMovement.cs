using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericMovement : MonoBehaviour
{
    public Vector3 Speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.localPosition = new Vector3(transform.localPosition.x + Speed.x, transform.localPosition.y + Speed.y, transform.localPosition.z);
    }
}
