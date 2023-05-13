using System;
using UnityEngine;

public class LadderMovement : MonoBehaviour
{
    private float vertical;
    [SerializeField] private float speed = 8f;
    private bool isLadder;
    private bool isClimbing;

    private Rigidbody2D rb;
    private PlayerManager playerManager;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerManager = GetComponent<PlayerManager>();
    }

    void Update()
    {
        vertical = Input.GetAxisRaw("Vertical");

        if (isLadder && !playerManager.isBoxModeOn && Mathf.Abs(vertical) > 0f)
        {
            isClimbing = true;
        }
    }

    private void FixedUpdate()
    {
        if (isClimbing)
        {
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(rb.velocity.x, vertical * speed);
        }
        else
        {
            rb.gravityScale = 1f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = false;
            isClimbing = false;
        }
    }
}
