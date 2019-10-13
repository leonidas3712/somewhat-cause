using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float maxWalkingSpeed, jumpSpeed;
    float walkingDir;
    bool jumping;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        walkingDir = Input.GetAxisRaw("Horizontal");
        Jump();
    }
    private void FixedUpdate()
    {
        Walk();
    }

    private void Walk()
    {
        if (Mathf.Abs(rb.velocity.x) <= maxWalkingSpeed)
        {
            rb.velocity = new Vector2(maxWalkingSpeed * walkingDir, rb.velocity.y);
        }
    }
    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (GroundDetection.grounded)
            {
                jumping = true;
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + jumpSpeed);
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
