using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InheritVelocityDirection : MonoBehaviour
{
    private Rigidbody2D _rigidbody2d;
    private Rigidbody2D _parentRigidbody2d;

    [SerializeField] private float delay = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _parentRigidbody2d = transform.parent.GetComponent<Rigidbody2D>();
        if (_rigidbody2d == null) Debug.LogError("InheritParentVelocity should only be added to GameObject with Rigidbody2D. Remember to put Rigidbody2D when designing your projectile.");
        if (_parentRigidbody2d == null) Debug.LogError("InheritParentVelocity should only be added to GameObject with Rigidbody2D. Remember to put Rigidbody2D when designing your projectile.");
        Invoke("ApplyVelocity", delay);
    }

    void ApplyVelocity()
    {
        _rigidbody2d.velocity = _parentRigidbody2d.velocity;
    
    }
}
