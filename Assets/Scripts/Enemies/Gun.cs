using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        PlayerController pc = other.GetComponent<PlayerController>();
        if (pc) pc.TakeDamage();
    }
}
