using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foe : MonoBehaviour
{
    Gun gun;
    GameObject player;
    [SerializeField]
    float sightRange;
    int layerMask;
    bool playerSighted;
    // Start is called before the first frame update
    void Start()
    {
        layerMask = ~(LayerMask.GetMask("enemies"));
        player = GameObject.Find("Player");
        gun = GetComponent<Gun>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerSighted) gun.Fire();
        else SearchForPlayer();
    }
    
    void SearchForPlayer()
    {
        RaycastHit2D playerSight = Physics2D.Raycast(transform.position, player.transform.position-transform.position, sightRange, layerMask);
        if(playerSight.collider)
        if (playerSight.collider.tag == "Player") playerSighted = true;
    }
}
