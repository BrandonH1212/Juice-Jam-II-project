using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Target
{
    Closest,
    Furthest,
    Random,
}

public class TargetingScript : MonoBehaviour
{
    public Target TargetType;
    public List<GameObject> TargetObjects;
    public Projectile Projectile;
    
    void Start()

    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1000);
        TargetObjects = new List<GameObject>();

        foreach (Collider2D c in colliders)
        {
            if (c.gameObject.tag == "Enemy")
            {
                if (!TargetObjects.Contains(c.gameObject))
                    TargetObjects.Add(c.gameObject);
            }
        }

        TargetObjects = new List<GameObject>();
        Projectile = GetComponent<Projectile>();
    }

    void FixedUpdate()
    {
        switch (TargetType)
        {
            case Target.Closest:

                GameObject closestTarget = null;
                float closestDistance = float.MaxValue;

                foreach (GameObject t in TargetObjects)
                {
                    float distance = Vector3.Distance(transform.position, t.transform.position);
                    if (distance < closestDistance)
                    {
                        closestTarget = t;
                        closestDistance = distance;
                    }
                }


                if (closestTarget != null)
                    ApplyVelocityTowardsTarget(closestTarget);

                break;

            case Target.Furthest:

                GameObject furthestTarget = null;
                float furthestDistance = 0f;

                foreach (GameObject t in TargetObjects)
                {
                    float distance = Vector3.Distance(transform.position, t.transform.position);
                    if (distance > furthestDistance)
                    {
                        furthestTarget = t;
                        furthestDistance = distance;
                    }
                }


                if (furthestTarget != null)
                    ApplyVelocityTowardsTarget(furthestTarget);

                break;

            case Target.Random:

                if (TargetObjects.Count > 0)
                {
                    int randomIndex = Random.Range(0, TargetObjects.Count);
                    ApplyVelocityTowardsTarget(TargetObjects[randomIndex]);
                }

                break;
        }
    }

    void ApplyVelocityTowardsTarget(GameObject target)
    {
        //transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 40);
        Vector3 direction = target.transform.position - transform.position;
        direction.Normalize();

        GetComponent<Rigidbody2D>().velocity = direction * Projectile.GetStatsAppliedAsDictionary()[Stat.ProjectileSpeed];
    }
}