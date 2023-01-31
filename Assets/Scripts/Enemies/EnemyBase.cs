using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public float Health = 100;
    public int XpDrop = 1;
    public GameObject OnDeathEffect;
    public GameObject OnDamagedEffect;
    public GameObject OnSpawnEffect;
    private bool IsDead = false;
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
        if (!IsDead)
        {
            if (OnDeathEffect != null) Instantiate(OnDeathEffect, transform.position, new Quaternion());
            // drop xp
            GameObject xpObj = Instantiate(Resources.Load("XpOrb"), transform.position, new Quaternion()) as GameObject;
            xpObj.GetComponent<XPPickup>().xpAmount = XpDrop;
            IsDead = true;
            Destroy(gameObject);
        }
    }
}
