using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialVelocityInDirection : MonoBehaviour
{
    [SerializeField]
    private Vector2 Direction;

    [SerializeField] private float Speed;
    [SerializeField] private bool UseProjectileSpeed = true;

    private Rigidbody2D _rigidbody2d;
    private ProjectileHitbox _projectileHitbox;

    // Start is called before the first frame update
    void Start()
    {
        _projectileHitbox = GetComponent<ProjectileHitbox>();
        _rigidbody2d = GetComponent<Rigidbody2D>();


        if (UseProjectileSpeed) Speed = (float)ConstantValues.ProjectileSpeed / _projectileHitbox.GetStatsAppliedAsDictionary()[Stat.ProjectileSpeed];

        _rigidbody2d.velocity = Direction * Speed;



    }

}
