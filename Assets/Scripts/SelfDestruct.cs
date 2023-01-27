using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    public float TimeToDestruct = 3;
    public float RemainingTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        RemainingTime = TimeToDestruct;
    }

    // Update is called once per frame
    void Update()
    {
        RemainingTime -= Time.deltaTime;
        if (RemainingTime <= 0) Destroy(this.gameObject);
    }
}
