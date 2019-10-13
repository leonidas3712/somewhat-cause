using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float acceleration, maxWalkingSpeed, jumpSpeed;
    float walkingDir;
    bool jumping;
    Rigidbody2D rb;
    int hp =10;

    public void TakeDamage()
    {
        hp--;
        if (hp <= 0) Destroy(gameObject);
        //die
    }
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
        rb.velocity = new Vector2(maxWalkingSpeed*walkingDir,rb.velocity.y);
    }
    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (GroundDetection.grounded)
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
                    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2);
            }
        }

    }
}
