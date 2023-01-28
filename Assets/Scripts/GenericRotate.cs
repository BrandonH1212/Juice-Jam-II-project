using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericRotate : MonoBehaviour
{
    public float DegreePerSecond = 30;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.Rotate(new Vector3(0,0,1), DegreePerSecond * 0.02f);
    }
}
