using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject Player;
    public GameObject Enemy;
    public float TimeToSpawn = 1;
    public float TotalTime = 1;
    public int intensity = 1;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        TimeToSpawn -= Time.deltaTime;
        TotalTime += Time.deltaTime;
        //print(Mathf.Sqrt(TotalTime/10));

        if (TimeToSpawn <= 0) 
        {
            var unitVectorPlayer = new Vector3(Random.value - 0.5f, Random.value - 0.5f, 0);
            Quaternion playerRotation = Player.transform.rotation;
            Vector3 spawnPos = Player.transform.position + unitVectorPlayer * 300;

            GameObject newEnemy = Instantiate(Enemy, spawnPos, playerRotation);
            newEnemy.transform.localScale = new Vector3(5 + (intensity / 3), 5 + (intensity / 3), 1);
            var _comp = newEnemy.GetComponent<EnemyBase>();
            
            _comp.Health = 10 * intensity;
            _comp.XpDrop = 1 * intensity;

            TimeToSpawn = 3 - Mathf.Sqrt(TotalTime / 10)  + Random.value * 1;

            if (TimeToSpawn < 0.3f)
            {
                intensity++;
                TotalTime = TotalTime / 4;
                

            }
            
            print(TimeToSpawn);
        }
    }
}
