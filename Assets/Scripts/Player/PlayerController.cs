using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// The direction that the player is moving right now.
    /// The player position will be updated using this field, in fixedUpdate().
    /// </summary>
    private Vector2 _movementDirectionNormalized = new Vector2(0,0);
    private Rigidbody2D _rigidbody2D = null;
    private SpriteRenderer _spriteRenderer = null;
    private Animator _animator = null;
    

    // DEV:
    public GameObject BasicShot;
    public float TimeToShoot = 1;

    public float Health { get; private set; } = 100;
    public float MovementSpeed { get; private set; } = 50;
    public float Shield { get; private set; }
    public float ShieldRegenerationSpeed { get; private set; }
    public float DamageReduction { get; private set; }
    public float DamagePower { get; private set; }

    public UnityEvent OnDamage;
    public UnityEvent OnKilled;
    public UnityEvent OnShieldBroken;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        InferMovementDirection();

        TimeToShoot -= Time.deltaTime;
        if (TimeToShoot < 0) 
        {
            var newProjectileObject = Instantiate(BasicShot);
            newProjectileObject.transform.position = transform.position;
            TimeToShoot = 0.2f;
        }
    }

    void InferMovementDirection() 
    {
        Vector2 output = new(0,0);
        if (Input.GetKey(KeyCode.W)) output.y += MovementSpeed;
        if (Input.GetKey(KeyCode.A)) output.x -= MovementSpeed;
        if (Input.GetKey(KeyCode.S)) output.y -= MovementSpeed;
        if (Input.GetKey(KeyCode.D)) output.x += MovementSpeed;

        transform.rotation = Quaternion.EulerRotation(0,0, Mathf.Atan2(output.y, output.x) * 180f / (float)Math.PI);

        _movementDirectionNormalized = output;
    }

    private void FixedUpdate()
    {
        _rigidbody2D.AddForce(new Vector2(_movementDirectionNormalized.x, _movementDirectionNormalized.y));
        _rigidbody2D.velocity = Vector3.ClampMagnitude(_rigidbody2D.velocity, 100);

        // Update the animator 
        _animator.SetBool("IsMoving", _movementDirectionNormalized.magnitude > 0);

        // flip the sprite based on the velocity
        _spriteRenderer.flipX = _rigidbody2D.velocity.x > 0;

    }

    public void TakeDamage(float damage) 
    {
        if (Shield > 0)
        {
            if (Shield - damage <= 0) OnShieldBroken.Invoke();
            Shield = Math.Max(0, Shield - damage);
        }
        else
        {
            if (Health - damage <= 0) OnKilled.Invoke();
            Health = Math.Max(0, Health - damage);
        }
    }
}
