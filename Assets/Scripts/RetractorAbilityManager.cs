using UnityEngine;

public class RetractorAbilityManager : MonoBehaviour
{
    public Collider2D playerCollider;
    public RetractorManager retractorPrefab;
    float retractorThrowPower = 2f;


    RetractorManager retractor1 = null;
    RetractorManager retractor2 = null;
    float ropeLength = 1000f; // TODO temporary inital length, remove this later, and set it to the initial length after setting the retractors.

    // User inputs
    Vector3 mousePosition;
    bool setRetractorInput = false;
    bool useRetractorInput = false;

    void UseRetractor()
    {
        // TODO
    }

    void SetRetractor()
    {
        RetractorManager createdRetractor;
        if (retractor1 == null)
        {
            createdRetractor = retractor1 = Instantiate(retractorPrefab);
        }
        else
        {
            if (retractor2 == null)
            {
                createdRetractor = retractor2 = Instantiate(retractorPrefab);
                Physics2D.IgnoreCollision(retractor1.GetComponent<Collider2D>(), retractor2.GetComponent<Collider2D>());
            }
            else
            {
                Destroy(retractor1.gameObject);
                Destroy(retractor2.gameObject);
                createdRetractor = retractor1 = Instantiate(retractorPrefab);
                retractor2 = null;
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
        if (Input.GetButtonDown("SetRetractor"))
        {
            setRetractorInput = true;
        }
        if (Input.GetButtonDown("UseRetractor"))
        {
            useRetractorInput = true;
        }
    }

    void FixedUpdate()
    {
        if (useRetractorInput)
        {
            useRetractorInput = false;
            UseRetractor();
        }
        else if (setRetractorInput)
        {
            setRetractorInput = false;
            SetRetractor();
        }
    }
}
