using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.transform.rotation = Quaternion.EulerRotation(new Vector3(0,0,Random.value * 360));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
