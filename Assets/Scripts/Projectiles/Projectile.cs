using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;

/// <summary>
/// This class should be at the top of all projectile prefabs.
/// </summary>
public class Projectile : MonoBehaviour
{
    [System.Serializable]
    public class StatValuePair
    {
        public Stat Stat;
        public float Value;
        public StatValuePair(Stat stat, float value)
        {
            Stat = stat;
            Value = value;
        }
    }

    public List<StatValuePair> StatsApplied = new();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Apply stats to all of the ProjectileHitbox scripts found in this gameObject and all the children.
    /// </summary>
    /// <param name="stats"></param>
    public void ApplyStats(Dictionary<Stat, float> stats) 
    {
        StatsApplied.Clear();
        GetComponents<ProjectileHitbox>().Concat(GetComponentsInChildren<ProjectileHitbox>()).ToList().ForEach(x => 
        {
            x.StatsApplied = StatsApplied;
        });
        foreach (KeyValuePair<Stat, float> pair in stats) StatsApplied.Add(new(pair.Key, pair.Value));
    }

    public Dictionary<Stat, float> GetStatsAppliedAsDictionary() 
    {
        Dictionary<Stat, float> result = new Dictionary<Stat, float>();
        StatsApplied.ForEach(x => { result.Add(x.Stat, x.Value); });
        return result;
    }
}

