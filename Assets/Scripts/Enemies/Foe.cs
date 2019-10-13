using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foe : MonoBehaviour
{
    GameObject player;
    [SerializeField]
    float sightRange, loseRange;
    int layerMask;
    bool playerSighted;
    float PlayerDistance;
    public GameObject particleSystem;
    Vector2 playerDir;

    // Start is called before the first frame update
    void Start()
    {
        layerMask = ~(LayerMask.GetMask("enemies"));
        player = GameObject.Find("Player");
        particleSystem.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        playerDir = player.transform.position - transform.position;
        particleSystem.transform.rotation = Quaternion.Euler(Mathf.Atan2(playerDir.y, playerDir.x) * Mathf.Rad2Deg * -1,90,0);
         
        PlayerDistance = Vector2.Distance(transform.position, player.transform.position);
        if (playerSighted)
        {
            if (loseRange > PlayerDistance)
            {
                StartShooting();
            }
            else
            {
                playerSighted = false;
                StopShooting();
            }
        }
        else SearchForPlayer();
    }

    void SearchForPlayer()
    {
        RaycastHit2D playerSight = Physics2D.Raycast(transform.position, player.transform.position - transform.position, sightRange, layerMask);
        if (playerSight.collider)
            if (playerSight.collider.tag == "Player") playerSighted = true;
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
