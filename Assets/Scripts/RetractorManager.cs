using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(FixedJoint2D))]
public class RetractorManager : MonoBehaviour
{
    public float maximumTimeForAttachment = 1f;
    bool isAttached = false;
    Rigidbody2D rb;

    RetractorAbilityManager abilityManager;

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
            if (abilityManager != null)
            {
                abilityManager.OnRetractorAttach();
            }
            else
            {
                Debug.LogError("Ability manager is null!");
            }
        }
    }

    internal void SetAbilityManager(RetractorAbilityManager abilityManager)
    {
        this.abilityManager = abilityManager;
    }

    private void AttachByCollision(Collision2D collision)
    {
        transform.position = collision.contacts[0].point;
        FixedJoint2D fixedJoint = GetComponent<FixedJoint2D>();
        fixedJoint.enabled = true;
        fixedJoint.connectedBody = collision.collider.GetComponent<Rigidbody2D>();
    }

    internal bool IsAttached() => isAttached;
}
