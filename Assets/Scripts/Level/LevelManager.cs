using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject Player;
    public GameObject Enemy;
    public float TimeToSpawn = 1;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
    

    }

    // Update is called once per frame
    void Update()
    {
        TimeToSpawn -= Time.deltaTime;
        if (TimeToSpawn <= 0) 
        {
            var unitVectorPlayer = new Vector3(Random.value - 0.5f, Random.value - 0.5f, 0);
            Quaternion playerRotation = Player.transform.rotation;
            Vector3 spawnPos = Player.transform.position + unitVectorPlayer * 300;

            Instantiate(Enemy, spawnPos, playerRotation);
            TimeToSpawn = 0.02f + Random.value * 0.05f;
        }
    }
}
