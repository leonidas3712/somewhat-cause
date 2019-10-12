using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float acceleration, maxWalkingSpeed, jumpSpeed;
    float walkingDir;
    bool grounded = true, jumping;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        walkingDir = Input.GetAxis("Horizontal");
        Jump();
    }
    private void FixedUpdate()
    {
        Walk();
    }

    private void Walk()
    {
        if (walkingDir > 0)
        {
            if (rb.velocity.x > 0)
            {
                if (rb.velocity.x < maxWalkingSpeed)
                    rb.AddForce(Vector2.right * acceleration * walkingDir);
            }
            else
                rb.AddForce(Vector2.right * acceleration * walkingDir);
        }
        if (walkingDir < 0)
        {
            if (rb.velocity.x < 0)
            {
                if (rb.velocity.x > -maxWalkingSpeed)
                    rb.AddForce(Vector2.right * acceleration * walkingDir);
            }
            else
                rb.AddForce(Vector2.right * acceleration * walkingDir);
        }

    }
    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (grounded)
            {
                jumping = true;
                rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            }
        }
        if (Input.GetButtonUp("Jump"))
        {
            if (jumping)
            {
                jumping = false;
                if (rb.velocity.y > 0)
                    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 3);
            }
        }

    }
}
