using UnityEngine;

public class RetractorAbilityManager : MonoBehaviour
{
    public Material lineMaterial;
    public Collider2D playerCollider;
    public RetractorManager retractorPrefab;
    public float ropePullRatio = 0.1f;
    readonly float retractorThrowPower = 2f;
    readonly float refractorPullPower = 100f;
    float ropeLength = -1;
    readonly float minimumLineWidth = 0.1f;
    readonly float maximumLineWidth = 0.2f;

    LineRenderer lineRenderer;

    RetractorManager retractor1 = null;
    RetractorManager retractor2 = null;

    // User inputs
    Vector3 mousePosition;
    bool setRetractor1 = false;
    bool setRetractor2 = false;
    bool useRetractorInput = false;

    void UseRetractor()
    {
        if (!AreRetractorsAttached())
        {
            return;
        }
        float retractorsDistance = CalculateRetractorsDistance();
        bool useForces = true;
        if (retractorsDistance < ropeLength)
        {
            ropeLength -= ropePullRatio * refractorPullPower * Time.fixedDeltaTime;
            ropeLength = Mathf.Max(ropeLength, 0f);
        }
        // This is checked again on purpose! Keep it like that!
        if (retractorsDistance < ropeLength)
        {
            useForces = false;
        }
        if (useForces)
        {
            Rigidbody2D rb1 = retractor1.GetComponent<Rigidbody2D>();
            Rigidbody2D rb2 = retractor2.GetComponent<Rigidbody2D>();
            Vector2 force = refractorPullPower * (retractor2.transform.position - retractor1.transform.position).normalized;
            rb1.AddForce(force, ForceMode2D.Force);
            rb2.AddForce(-force, ForceMode2D.Force);
            ropeLength = Mathf.Min(ropeLength, CalculateRetractorsDistance());
        }
        DistanceJoint2D distanceJoint = retractor1.gameObject.GetComponent<DistanceJoint2D>();
        distanceJoint.distance = ropeLength;
    }

    private float CalculateRetractorsDistance()
    {
        return Mathf.Abs((retractor1.transform.position - retractor2.transform.position).magnitude);
    }

    internal void OnRetractorAttach()
    {
        if (!AreRetractorsAttached())
        {
            return;
        }
        ropeLength = CalculateRetractorsDistance();
        DistanceJoint2D distanceJoint = retractor1.gameObject.AddComponent<DistanceJoint2D>();
        distanceJoint.maxDistanceOnly = true;
        distanceJoint.distance = ropeLength;
        distanceJoint.connectedBody = retractor2.GetComponent<Rigidbody2D>();
        lineRenderer = retractor2.gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = maximumLineWidth * 1.5f;
        lineRenderer.material = lineMaterial;
        lineRenderer.endColor = lineRenderer.startColor = Color.black;
        lineRenderer.enabled = true;
        lineRenderer.positionCount = 2;
    }

    private bool AreRetractorsAttached()
    {
        if (retractor1 == null || retractor2 == null)
        {
            return false;
        }
        if (!retractor1.IsAttached() || !retractor2.IsAttached())
        {
            return false;
        }
        return true;
    }

    void SetRetractor(int retractorToSet)
    {
        RetractorManager createdRetractor;
        if (retractorToSet == 1)
        {
            if (retractor1 != null)
            {
                Destroy(retractor1.gameObject);
                retractor1 = null;
            }
            if (retractor2 != null)
            {
                Destroy(retractor2.gameObject);
                retractor2 = null;
            }
            createdRetractor = retractor1 = Instantiate(retractorPrefab);
        }
        else
        {
            if (retractor2 != null)
            {
                Destroy(retractor2.gameObject);
                retractor2 = null;
            }
            createdRetractor = retractor2 = Instantiate(retractorPrefab);
            if (retractor1 != null)
            {
                Physics2D.IgnoreCollision(retractor1.GetComponent<Collider2D>(), retractor2.GetComponent<Collider2D>());
            }
        }
        createdRetractor.SetAbilityManager(this);
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
        RenderLine();
    }

    private void RenderLine()
    {
        if (lineRenderer != null)
        {
            if (retractor1 == null || retractor2 == null)
            {
                lineRenderer.enabled = false;
                return;
            }
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, new Vector3(retractor1.transform.position.x, retractor1.transform.position.y, -1));
            lineRenderer.SetPosition(1, new Vector3(retractor2.transform.position.x, retractor2.transform.position.y, -1));
        }
    }

    void FixedUpdate()
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
        if (useRetractorInput)
        {
            UseRetractor();
        }
        if (AreRetractorsAttached())
        {
            if (ropeLength == 0)
            {
                lineRenderer.startWidth = minimumLineWidth;
            }
            else
            {
                lineRenderer.startWidth = Mathf.Lerp(maximumLineWidth, minimumLineWidth, CalculateRetractorsDistance() / ropeLength);
            }
        }
    }
}
