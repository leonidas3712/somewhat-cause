using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RetractorManager : MonoBehaviour
{
    public float maximumTimeForAttachment = 1f;
    bool isAttached = false;
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Invoke("DestroyIfNoAttach", maximumTimeForAttachment);
    }

    void DestroyIfNoAttach()
    {
        if (!isAttached)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isAttached && collision.collider.GetComponent<Rigidbody2D>() != null)
        {
            isAttached = true;
            AttachByCollision(collision);
        }
    }

    private void AttachByCollision(Collision2D collision)
    {
        Vector2 normal = collision.contacts[0].normal;
        transform.position = collision.contacts[0].point;
        transform.rotation = Quaternion.FromToRotation(Vector2.up, normal);

        FixedJoint2D fixedJoint = GetComponent<FixedJoint2D>();
        fixedJoint.connectedBody = collision.collider.GetComponent<Rigidbody2D>();
        fixedJoint.connectedAnchor = collision.collider.transform.InverseTransformPoint(collision.contacts[0].point);
        fixedJoint.enabled = true;

        transform.position = collision.contacts[0].point;
        transform.rotation = Quaternion.FromToRotation(Vector2.up, normal);
    }

    internal bool IsAttached() => isAttached;
}
