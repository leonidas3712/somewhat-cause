using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RetractorManager : MonoBehaviour
{
    public float maximumTimeForAttachment = 1f;
    GameObject attachedObject = null;
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Invoke("DestroyIfNoAttach", maximumTimeForAttachment);
    }

    void DestroyIfNoAttach()
    {
        if (attachedObject == null)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        AttachByCollision(collision);
    }

    private void AttachByCollision(Collision2D collision)
    {
        rb.simulated = false;
        attachedObject = collision.collider.gameObject;
        transform.SetParent(attachedObject.transform);
        transform.position = collision.contacts[0].point;
        Vector2 normal = collision.contacts[0].normal;
        transform.rotation = Quaternion.FromToRotation(Vector2.up, normal);
    }

    internal bool IsAttached() => attachedObject != null;

    internal GameObject GetAttachedObject() => attachedObject;
}
