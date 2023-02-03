using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TargetType
{
    Closest,
    Furthest,
    Any,
}
public class InitialVelocityTowardsEnemy : MonoBehaviour
{
    [SerializeField] private TargetType _targetType;
    [SerializeField] private Vector2 _targetOffset = Vector2.zero;
    [SerializeField] private bool _useRange = false;


    private Rigidbody2D _rigidbody2d;
    private ProjectileHitbox _projectile;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _projectile = GetComponent<ProjectileHitbox>();
        if (_rigidbody2d == null) Debug.LogError("InitialVelocityTowardsEnemy should only be added to GameObject with Rigidbody2D. Remember to put Rigidbody2D when designing your projectile.");
        if (_projectile == null) Debug.LogError("InitialVelocityTowardsEnemy should only be added to GameObject with Projectile. Remember to put Projectile when designing your projectile.");

        Collider2D[] colliders;
        if (_useRange)
        {
            colliders = Physics2D.OverlapCircleAll(transform.position, _projectile.GetStatsAppliedAsDictionary()[Stat.Range] * (float)ConstantValues.Range, LayerMask.GetMask("enemy"));
        }
        else
        {
            colliders = Physics2D.OverlapCircleAll(transform.position, 150, LayerMask.GetMask("enemy"));
        }

        if (colliders.Length == 0) return;


        GameObject target = null;

        foreach (Collider2D c in colliders)
        {
            switch (_targetType)
            {
                case TargetType.Closest:
                    if (c.gameObject.tag == "Enemy")
                    {
                        if (target == null)
                        {
                            target = c.gameObject;
                        }
                        else
                        {
                            if (Vector2.Distance(transform.position, c.gameObject.transform.position) <
                                Vector2.Distance(transform.position, target.transform.position))
                            {
                                target = c.gameObject;
                            }
                        }
                    }
                    break;
                case TargetType.Furthest:
                    if (c.gameObject.tag == "Enemy")
                    {
                        if (target == null)
                        {
                            target = c.gameObject;
                        }
                        else
                        {
                            if (Vector2.Distance(transform.position, c.gameObject.transform.position) >
                                Vector2.Distance(transform.position, target.transform.position))
                            {
                                target = c.gameObject;
                            }
                        }
                    }
                    break;
                case TargetType.Any:
                    if (c.gameObject.tag == "Enemy")
                    {
                        target = c.gameObject;
                        break;
                    }
                    break;

            }

        }
        _rigidbody2d.AddForce(((Vector2)target.transform.position + _targetOffset - (Vector2)transform.position).normalized * _projectile.GetStatsAppliedAsDictionary()[Stat.ProjectileSpeed] / (float)ConstantValues.ProjectileSpeed);
    }
}