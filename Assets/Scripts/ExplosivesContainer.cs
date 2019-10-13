using System.Collections;
using UnityEngine;

public class ExplosivesContainer : MonoBehaviour
{
    public float impulseForExplosion = 15f;
    public float explosionRadius = 15f;
    public float explosionForce = 7f;

    void OnCollisionEnter2D(Collision2D collision)
    {
        //on collision with something, start a coroutine
        StartCoroutine("OnImpulse");
    }

    IEnumerator OnImpulse()
    {
        Vector3 initialVelocity, newVelocity;
        initialVelocity = transform.GetComponent<Rigidbody2D>().velocity;

        yield return null;
        yield return null;
        yield return null;
        yield return null;
        yield return null;

        newVelocity = transform.GetComponent<Rigidbody2D>().velocity;

        //impulse = magnitude of change
        Vector3 result = initialVelocity - newVelocity;

        if (result.magnitude > impulseForExplosion)
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, explosionRadius, Vector2.zero);
            foreach (RaycastHit2D hit in hits)
            {
                Rigidbody2D objectRb = hit.collider.GetComponent<Rigidbody2D>();
                if (objectRb != null)
                {
                    objectRb.AddForce(-hit.normal * explosionForce, ForceMode2D.Impulse);
                }
            }
            Destroy( Instantiate(Resources.Load("Explooosion"),transform.position, Quaternion.Euler(Vector3.zero)),1);
            Destroy(gameObject);
        }
    }
}
