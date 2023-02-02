using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;
using UnityEngine.Rendering;

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
        //ApplyStats(GetStatsAppliedAsDictionary());

    }


    /// <summary>
    /// Apply stats to all of the ProjectileHitbox scripts found in this gameObject and all the children.
    /// </summary>
    /// <param name="stats"></param>
    public void ApplyStats(Dictionary<Stat, float> stats)
    {
        StatsApplied.Clear();
        var hitboxes = GetComponents<ProjectileHitbox>().Concat(GetComponentsInChildren<ProjectileHitbox>());

        foreach (var hitbox in hitboxes)
        {
            hitbox.StatsApplied = StatsApplied;
        }
        foreach (KeyValuePair<Stat, float> pair in stats)
        {
            StatsApplied.Add(new(pair.Key, pair.Value));
        }
        foreach (var hitbox in hitboxes)
        {
            hitbox.UpdatedStats(stats);
        }


    }
    public Dictionary<Stat, float> GetStatsAppliedAsDictionary() 
    {
        Dictionary<Stat, float> result = new Dictionary<Stat, float>();
        StatsApplied.ForEach(x => { result.Add(x.Stat, x.Value); });
        return result;
    }
}

