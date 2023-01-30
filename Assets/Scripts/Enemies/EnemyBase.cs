using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public float Health = 100;
    public GameObject OnDeathEffect;
    public GameObject OnDamagedEffect;
    public GameObject OnSpawnEffect;

    // Start is called before the first frame update
    void Start()
    {
        if (OnSpawnEffect != null) Instantiate(OnSpawnEffect, transform.position, new Quaternion());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile")) 
        {
            var damageReceived = collision.gameObject.GetComponent<ProjectileHitbox>().StatsApplied.Find(x => x.Stat == Stat.Damage).Value;
            if (OnDamagedEffect != null) Instantiate(OnDamagedEffect, transform.position, new Quaternion());
            ApplyDamage(damageReceived);
        }
    }

    public void ApplyDamage(float damage) 
    {
        Health -= damage;
        if (Health <= 0) OnDeath();
    }

    void OnDeath() 
    {
        if (OnDeathEffect != null) Instantiate(OnDeathEffect, transform.position, new Quaternion());
        Destroy(gameObject);
    }
}
