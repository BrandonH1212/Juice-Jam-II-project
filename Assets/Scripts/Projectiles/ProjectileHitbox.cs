using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using static Projectile;

/// <summary>
/// This component should be added to all hitboxes of the projectile prefabs.
/// </summary>
public class ProjectileHitbox : MonoBehaviour
{
    CircleCollider2D _collider2d;
    public List<StatValuePair> StatsApplied = new();
    public bool DestroyOneOnHit = false;
    public bool UsePenetration = true;
    private int _hits = 0;

    Dictionary<Stat, float> _statsApplied = new();

    [SerializeField]
    public GameObject OnHitEffect;

    [SerializeField]
    public GameObject OnExplodeEffect;

    public Dictionary<Stat, float> GetStatsAppliedAsDictionary()
    {
        Dictionary<Stat, float> stats = new();
        foreach (StatValuePair pair in StatsApplied)
        {
            stats.Add(pair.Stat, pair.Value);
        }
        return stats;
    }

    // Start is called before the first frame update
    void Start()
    {
        _collider2d = GetComponent<CircleCollider2D>();
        if (_collider2d == null) Debug.LogError("ProjectileHitbox should only be added to GameObject with CircleCollider2D. Remember to put CircleCollider2D when designing your projectile.");
        _collider2d.isTrigger = true;
    }

    public void UpdatedStats(Dictionary<Stat, float> stats)
    {
        _statsApplied = stats;
        float _newSize = (float)Mathf.Clamp(stats[Stat.ProjectileSize] / (float)ConstantValues.Size, 0.1f, 100f);
        transform.localScale = new Vector3(_newSize, _newSize, transform.localScale.z);

    }

    
    private void DoSplashDamage(Vector2 position, float radius, float damage)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, radius * (float)ConstantValues.SplashRange, LayerMask.GetMask("enemy"));
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.tag == "Enemy")
            {
                collider.gameObject.GetComponent<EnemyBase>().ApplyDamage(damage);
                if (OnExplodeEffect != null) Instantiate(OnExplodeEffect, collider.transform.position, Quaternion.identity);
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (_statsApplied.ContainsKey(Stat.SplashDamage) && _statsApplied.ContainsKey(Stat.SplashRadius))
            {
                DoSplashDamage(transform.position, _statsApplied[Stat.SplashRadius], _statsApplied[Stat.SplashDamage]);
            }
            
            if (OnHitEffect != null)
            {
                GameObject effect = Instantiate(OnHitEffect, transform.position, Quaternion.identity);
                Destroy(effect, 1f);
            }
            if (UsePenetration)
            {
                _hits++;
                var maxHits = 0;
                if (_statsApplied.ContainsKey(Stat.Penetration)) maxHits = (int)_statsApplied[Stat.Penetration];
                if (_hits > maxHits)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }


    }
}
