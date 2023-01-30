using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;

public class SimpleAI : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float health = 100f;
    private Transform target;
    private float baseKnockback = 10f;
    private float knockbackDelay = 0.15f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private bool beingHit = false;

    public UnityEvent OnBegin, OnDone;

    public void takeDamange(float damage, float knockbackModifier)
    {
        StopAllCoroutines();
        beingHit = true;
        OnBegin?.Invoke();
        health -= damage;
        Vector2 direction = (transform.position - target.transform.position).normalized;
        rb.AddForce(direction * baseKnockback * knockbackModifier, ForceMode2D.Impulse);
        StartCoroutine(ResetKnockback());
    }

    public void setTarget(Transform target)
    {
        this.target = target;
    }

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.Find("Player").transform;
    }

    private void Update()
    {
        //temp
        if (Input.GetKeyDown("space"))
        {
            Debug.Log(health);
            takeDamange(10f, 1f);
        }

        if (health <= 0)
        {
            StopAllCoroutines();
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        if(!beingHit)
        {
            Vector3 direction = target.position - transform.position;
            direction.Normalize();
            movement = direction;
            moveCharacter(movement);
        }
    }

    void moveCharacter(Vector2 direction)
    {
        rb.MovePosition((Vector2) transform.position + (direction * moveSpeed * Time.deltaTime));
    }

    private IEnumerator ResetKnockback()
    {
        yield return new WaitForSeconds(knockbackDelay);
        rb.velocity = Vector3.zero;
        beingHit = false;
        OnDone?.Invoke();
    }
}
