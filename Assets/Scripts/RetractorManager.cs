using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(FixedJoint2D))]
public class RetractorManager : MonoBehaviour
{
    public Material lineMaterial;
    bool isAttached = false;
    Rigidbody2D rb;

    PairOfRetractorsManager pairOfRetractorsManager;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isAttached && collision.collider.GetComponent<Rigidbody2D>() != null)
        {
            AttachByCollision(collision);
            isAttached = true;
            if (pairOfRetractorsManager != null)
            {
                pairOfRetractorsManager.OnRetractorAttach(lineMaterial);
            }
            else
            {
                Debug.LogError("pairOfRetractorsManager is null!", this);
            }
        }
    }

    void Update()
    {
        if (isAttached)
        {
            if (GetComponent<FixedJoint2D>().connectedBody == null)
            {
                Destroy(gameObject);
            }
        }
    }

    internal void SetPairOfRetractors(PairOfRetractorsManager pairOfRetractors)
    {
        this.pairOfRetractorsManager = pairOfRetractors;
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
