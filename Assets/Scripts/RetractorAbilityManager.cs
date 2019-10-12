using UnityEngine;

public class RetractorAbilityManager : MonoBehaviour
{
    public Collider2D playerCollider;
    public RetractorManager retractorPrefab;
    float retractorThrowPower = 2f;
    float refractorPullPower = 100f;


    RetractorManager retractor1 = null;
    RetractorManager retractor2 = null;

    // User inputs
    Vector3 mousePosition;
    bool setRetractor1 = false;
    bool setRetractor2 = false;
    bool useRetractorInput = false;

    void UseRetractor()
    {
        if (retractor1 == null || retractor2 == null)
        {
            return;
        }
        if (!retractor1.IsAttached() || !retractor2.IsAttached())
        {
            return;
        }
        // Both retractors are attached.
        Rigidbody2D rb1 = retractor1.GetAttachedObject().GetComponent<Rigidbody2D>();
        Rigidbody2D rb2 = retractor2.GetAttachedObject().GetComponent<Rigidbody2D>();
        if (rb1 == null || rb2 == null)
        {
            Debug.LogError("NO RIGIDBODY");
            return;
        }
        Vector2 force = refractorPullPower * (retractor2.transform.position - retractor1.transform.position).normalized;
        rb1.AddForce(force, ForceMode2D.Force);
        rb2.AddForce(-force, ForceMode2D.Force);
    }

    void SetRetractor(int retractorToSet)
    {
        RetractorManager createdRetractor;
        if (retractorToSet == 1)
        {
            if (retractor1 != null)
            {
                Destroy(retractor1.gameObject);
            }
            if (retractor2 != null)
            {
                Destroy(retractor2.gameObject);
            }
            createdRetractor = retractor1 = Instantiate(retractorPrefab);
        }
        else
        {
            if (retractor2 != null)
            {
                Destroy(retractor2.gameObject);
            }
            createdRetractor = retractor2 = Instantiate(retractorPrefab);
            if (retractor1 != null)
            {
                Physics2D.IgnoreCollision(retractor1.GetComponent<Collider2D>(), retractor2.GetComponent<Collider2D>());
            }
        }
        createdRetractor.transform.position = transform.position;
        Physics2D.IgnoreCollision(createdRetractor.GetComponent<Collider2D>(), playerCollider);

        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        Vector3 targetLocation = ray.origin + (ray.direction * -ray.origin.z);
        Vector3 diff = targetLocation - transform.position;
        Vector3 force = diff.normalized * retractorThrowPower;
        createdRetractor.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = Input.mousePosition;
        useRetractorInput = Input.GetButton("UseRetractor");
        if (Input.GetMouseButtonDown(0))
        {
            setRetractor1 = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            setRetractor2 = true;
        }
    }

    void FixedUpdate()
    {
        if (useRetractorInput)
        {
            UseRetractor();
        }
        else
        {
            if (setRetractor1)
            {
                setRetractor1 = false;
                SetRetractor(1);
            }
            if (setRetractor2)
            {
                setRetractor2 = false;
                SetRetractor(2);
            }
        }
    }
}
