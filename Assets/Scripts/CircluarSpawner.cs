using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class CircluarSpawner : MonoBehaviour
{
    public GameObject ObjectToSpawn;
    public int Interval = 8;

    // Start is called before the first frame update
    void Start()
    {
        if (ObjectToSpawn != null)
        {
            for (float i = 0; i < 2f * Mathf.PI; i += (2f * Mathf.PI) / Interval)
            {
                var newObject = Instantiate(ObjectToSpawn);
                newObject.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                newObject.transform.rotation = Quaternion.EulerRotation(new Vector3(0,0, i));
            }
        }

        GetComponent<Projectile>().ApplyStats(GetComponent<Projectile>().GetStatsAppliedAsDictionary());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
