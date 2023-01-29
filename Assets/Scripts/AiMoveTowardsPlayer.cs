using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiMoveTowardsPlayer : MonoBehaviour
{

    [SerializeField]
    private float _speed = 5.0f;

    [SerializeField]
    private float Health = 100;


    private GameObject _target = null;
    private Rigidbody2D _rigidbody2d = null;



    // Start is called before the first frame update
    void Start()
    {
        _target = GameObject.Find("Player");
        _rigidbody2d = GetComponent<Rigidbody2D>();

    }

    private void FixedUpdate()
    {
        if (_target != null)
        {
            Vector2 direction = (Vector2)_target.transform.position - (Vector2)transform.position;
            direction.Normalize();
            _rigidbody2d.velocity = direction * _speed;

        }
    }

}
