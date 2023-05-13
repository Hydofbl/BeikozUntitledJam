using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float walkSpeed;
    //[SerializeField] private float maxDistance;
    
    [HideInInspector] 
    public bool mustPatrol;
    private bool mustTurn;

    private Rigidbody2D rb;
    public Transform groundCheckPos;
    public LayerMask groundLayer;
    public Collider2D bodyCollider;

    //private Vector2 startPos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mustPatrol = true;
        //startPos = transform.position;
    }

    void Update()
    {
        if (mustPatrol)
        {
            // if (Vector2.Distance(transform.position, startPos) >= maxDistance)
            // {
            //     mustTurn = true;
            // }
            
            // ladder layer ile etkileştirerek düşmesini önlemek lazım
            mustTurn = !Physics2D.OverlapCircle(groundCheckPos.position, 0.01f, groundLayer);
            Patrol();
        }
    }

    private void FixedUpdate()
    {
        if (mustPatrol)
        {
            //if (Vector2.Distance(transform.position, startPos) < maxDistance)
                //mustTurn = !Physics2D.OverlapCircle(groundCheckPos.position, 0.1f, groundLayer);
        }
    }

    void Patrol()
    {
        if (mustTurn || bodyCollider.IsTouchingLayers(groundLayer))
        {
            Flip();
        }
        
        rb.velocity = new Vector2(walkSpeed * Time.fixedDeltaTime, rb.velocity.y);
    }

    void Flip()
    {
        mustPatrol = false;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        walkSpeed *= -1;
        mustPatrol = true;
    }
}
