using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Target
{
    Closest,
    Furthest,
    Random,
}

public class HomingScript : MonoBehaviour
{
    public Target TargetType;
    public List<GameObject> TargetObjects;
    private ProjectileHitbox Projectile;

    private GameObject _target;
    private Rigidbody2D _rigidbody2D;

    void Start()

    {
        Projectile = GetComponent<ProjectileHitbox>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    
        GetTarget();
    }


    void GetTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1000, LayerMask.GetMask("enemy"));
        TargetObjects = new List<GameObject>();
        float bestDistance = 0;
        foreach (Collider2D c in colliders)
        {

            if (c.gameObject.tag == "Enemy")
            {
                float distance = Vector3.Distance(transform.position, c.transform.position);
                switch (TargetType)
                {
                    case Target.Closest:
                        if (distance < bestDistance || bestDistance == 0)
                        {
                            bestDistance = distance;
                            _target = c.gameObject;
                        }
                        break;
                    case Target.Furthest:
                        if (distance > bestDistance || bestDistance == 0)
                        {
                            bestDistance = distance;
                            _target = c.gameObject;
                        }
                        break;
                    case Target.Random:
                        TargetObjects.Add(c.gameObject);
                        break;
                }
            }
        }
        _target = TargetObjects[Random.Range(0, TargetObjects.Count)];
    }


    void FixedUpdate()
    {
        if (_target != null)
        {
            ApplyVelocityTowardsTarget(_target);
        }
        else
        {
            GetTarget();
        }
    }

    void ApplyVelocityTowardsTarget(GameObject target)
    {
        Vector3 direction = target.transform.position - transform.position;
        direction.Normalize();

        _rigidbody2D.velocity = direction * ((float)Projectile._statsApplied[Stat.ProjectileSpeed] * (float)ConstantValues.ProjectileSpeed) * Time.deltaTime;
        _rigidbody2D.velocity = new Vector3(_rigidbody2D.velocity.x, _rigidbody2D.velocity.y, 1);
    }
}