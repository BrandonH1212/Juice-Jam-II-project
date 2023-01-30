using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Remove all the auto-destroy scripts of the children.
/// </summary>
public class RemoveAutoDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponentsInChildren<SelfDestruct>().ToList().ForEach(x => 
        {
            Destroy(x);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
