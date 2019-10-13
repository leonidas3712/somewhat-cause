using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetection : MonoBehaviour
{
    public static bool grounded = false;
    Vector2 hitDir;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        grounded = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        grounded = false;
    }


}
