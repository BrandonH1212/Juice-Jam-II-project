using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class CircluarSpawner : MonoBehaviour
{
    public GameObject ObjectToSpawn;
    public bool SpawnAsChildren = true;
    public int Interval = 8;

    // Start is called before the first frame update
    void Start()
    {
        if (ObjectToSpawn != null)
        {
            int spawnedCount = 0;
            for (float i = 0; i < 2f * Mathf.PI; i += (2f * Mathf.PI) / Interval)
            {
                if (spawnedCount > Interval - 1) break;
                GameObject newObject = null;

                if (SpawnAsChildren) newObject = Instantiate(ObjectToSpawn, transform);
                else Instantiate(ObjectToSpawn);

                newObject.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                newObject.transform.rotation = Quaternion.EulerRotation(new Vector3(0,0, i));
                spawnedCount++;
            }
        }

        GetComponent<Projectile>().ApplyStats(GetComponent<Projectile>().GetStatsAppliedAsDictionary());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
