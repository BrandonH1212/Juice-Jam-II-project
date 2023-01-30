using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Projectile;

/// <summary>
/// This component should be added to all hitboxes of the projectile prefabs.
/// </summary>
public class ProjectileHitbox : MonoBehaviour
{
    Collider2D _collider2d;
    public List<StatValuePair> StatsApplied = new();

    // Start is called before the first frame update
    void Start()
    {
        _collider2d = GetComponent<Collider2D>();
        if (_collider2d == null) Debug.LogError("ProjectileHitbox should only be added to GameObject with Collider2D. Remember to put Collider2D when designing your projectile.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
