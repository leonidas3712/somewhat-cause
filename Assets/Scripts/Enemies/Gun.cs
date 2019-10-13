using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject particleSystem;

    // Start is called before the first frame update
    void Start()
    {
        particleSystem.SetActive(false);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartShooting()
    {
        if (!particleSystem.activeSelf) particleSystem.SetActive(true);
    }
    public void StopShooting()
    {
        if (particleSystem.activeSelf) particleSystem.SetActive(false);
    }
}
