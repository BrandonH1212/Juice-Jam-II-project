using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircluarSpawner : MonoBehaviour
{
    public GameObject ObjectToSpawn;
    public float Radius = 8;
    public int Interval = 8;

    // Start is called before the first frame update
    void Start()
    {
        if (ObjectToSpawn != null)
        {
            for (float i = 0; i < 2f * Mathf.PI; i += (2f * Mathf.PI) / Interval)
            {
                var directionVector = new Vector3(Mathf.Sin(i), Mathf.Cos(i));
                var initialPosition = directionVector * Radius + transform.position;
                var newObject = Instantiate(ObjectToSpawn);
                newObject.transform.position = new Vector3(initialPosition.x, initialPosition.y, transform.position.z);
                newObject.transform.rotation = Quaternion.EulerRotation(new Vector3(0,0, i * Mathf.Rad2Deg));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
