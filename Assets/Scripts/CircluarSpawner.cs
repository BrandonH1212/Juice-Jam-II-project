using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircluarSpawner : MonoBehaviour
{
    public GameObject ObjectToSpawn;
    public int Interval = 8;
    public GameObject ProjectileParent;

    // Start is called before the first frame update
    void Start()
    {
        var projectileStats = ProjectileParent.GetComponent<Projectile>().GetStatsAppliedAsDictionary();
        if (ObjectToSpawn != null)
        {
            for (float i = 0; i < 2f * Mathf.PI; i += (2f * Mathf.PI) / Interval)
            {
                var newObject = Instantiate(ObjectToSpawn);
                newObject.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                newObject.transform.rotation = Quaternion.EulerRotation(new Vector3(0,0, i));
                newObject.GetComponent<Projectile>().ApplyStats(projectileStats);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
