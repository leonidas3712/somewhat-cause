using UnityEngine;

public class PairOfRetractorsManager
{
    readonly float retractorThrowPower = 2f;
    readonly float retractorPullPower = 200f;
    readonly float ropePullRatio = 0.2f;
    float ropeLength = -1;
    LineRenderer lineRenderer;
    readonly float minimumLineWidth = 0.1f;
    readonly float maximumLineWidth = 0.2f;

    RetractorManager retractor1 = null;
    RetractorManager retractor2 = null;

    internal void UseRetractors()
    {
        if (!AreRetractorsAttached())
        {
            return;
        }
        float retractorsDistance = CalculateRetractorsDistance();
        bool useForces = true;
        if (retractorsDistance < ropeLength)
        {
            ropeLength -= ropePullRatio * retractorPullPower * Time.fixedDeltaTime;
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
            Vector2 force = retractorPullPower * (retractor2.transform.position - retractor1.transform.position).normalized;
            rb1.AddForce(force, ForceMode2D.Force);
            rb2.AddForce(-force, ForceMode2D.Force);
            ropeLength = Mathf.Min(ropeLength, CalculateRetractorsDistance());
        }
        DistanceJoint2D distanceJoint = retractor1.gameObject.GetComponent<DistanceJoint2D>();
        distanceJoint.distance = ropeLength;
    }

    internal void UpdateLineWidth()
    {
        if (lineRenderer == null)
        {
            return;
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

    internal void RemoveRetractors()
    {
        if (retractor1 != null)
        {
            GameObject.Destroy(retractor1.gameObject);
            retractor1 = null;
        }
        if (retractor2 != null)
        {
            GameObject.Destroy(retractor2.gameObject);
            retractor2 = null;
        }
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

    private float CalculateRetractorsDistance()
    {
        return Mathf.Abs((retractor1.transform.position - retractor2.transform.position).magnitude);
    }

    internal void RenderLine()
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

    internal void OnRetractorAttach(Material lineMaterial)
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

    internal void SetRetractor(RetractorManager retractorPrefab, Collider2D playerCollider, Vector3 mousePosition, int retractorToSet)
    {
        RetractorManager createdRetractor;
        if (retractorToSet == 1)
        {
            RemoveRetractors();
            createdRetractor = retractor1 = GameObject.Instantiate(retractorPrefab);
        }
        else
        {
            if (retractor2 != null)
            {
                GameObject.Destroy(retractor2.gameObject);
                retractor2 = null;
            }
            createdRetractor = retractor2 = GameObject.Instantiate(retractorPrefab);
            if (retractor1 != null)
            {
                Physics2D.IgnoreCollision(retractor1.GetComponent<Collider2D>(), retractor2.GetComponent<Collider2D>());
            }
        }
        createdRetractor.SetPairOfRetractors(this);
        createdRetractor.transform.position = playerCollider.transform.position;
        Physics2D.IgnoreCollision(createdRetractor.GetComponent<Collider2D>(), playerCollider);

        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        Vector3 targetLocation = ray.origin + (ray.direction * -ray.origin.z);
        Vector3 diff = targetLocation - playerCollider.transform.position;
        Vector3 force = diff.normalized * retractorThrowPower;
        createdRetractor.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
    }
}
