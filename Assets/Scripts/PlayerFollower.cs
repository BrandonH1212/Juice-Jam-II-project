using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    public static GameObject PlayerObject;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerObject == null) 
        {
            GameObject[] objsWithTag = GameObject.FindGameObjectsWithTag("Player");
            if (objsWithTag.Length == 1) PlayerObject = objsWithTag[0];
            else if (objsWithTag.Length == 0) Debug.LogWarning("No GameObject with tag \"Player\" is found. Please add one before using PlayerFollower.");
            else Debug.LogWarning("More than one GameObject with tag \"Player\" is found. Please use just one player before using PlayerFollower.");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (PlayerObject != null) transform.position = PlayerObject.transform.position;
    }
}
