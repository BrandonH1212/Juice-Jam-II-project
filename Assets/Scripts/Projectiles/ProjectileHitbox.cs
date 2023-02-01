using System.Collections;
using System.Collections.Generic;
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
    
    [SerializeField]
    public GameObject OnHitEffect;
    
    // Start is called before the first frame update
    void Start()
    {
        _collider2d = GetComponent<CircleCollider2D>();
        if (_collider2d == null) Debug.LogError("ProjectileHitbox should only be added to GameObject with CircleCollider2D. Remember to put CircleCollider2D when designing your projectile.");
    }

    public void UpdatedStats(Dictionary<Stat, float> stats)
    {
        float _newSize = (float)Mathf.Clamp(stats[Stat.ProjectileSize] / (float)ConstantValues.Size, 0.1f, 100f);
        transform.localScale = new Vector3(_newSize, _newSize, transform.localScale.z);
        _hits = stats[Stat.Penetration] > 0 ? (int)stats[Stat.Penetration] : 1;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (OnHitEffect != null)
            {
                GameObject effect = Instantiate(OnHitEffect, transform.position, Quaternion.identity);
                Destroy(effect, 1f);
            }
            if (UsePenetration)
            {
                _hits--;
                if (_hits <= 0)
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
