using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetection : MonoBehaviour
{
    public static bool grounded = false;
    Vector2 hitDir;
    private void OnCollisionEnter2D(Collision2D coll)
    {
        hitDir = new Vector2(Mathf.Round(coll.GetContact(0).normal.x * 10) / 10, Mathf.Round(coll.GetContact(0).normal.y * 10) / 10);
        if (hitDir == Vector2.up) grounded = true;
    }
    private void OnCollisionExit2D(Collision2D coll)
    {
        grounded = false;
        /*if (coll.contactCount!=0)
        {
            hitDir = new Vector2(Mathf.Round(coll.GetContact(0).normal.x * 10) / 10, Mathf.Round(coll.GetContact(0).normal.y * 10) / 10);
            if (hitDir == Vector2.up) grounded = false;
        }*/

    }


}
