using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Transform destination;
    Transform player;
    public GameObject healthLast;
    private Transform respawn;

    private void Update()
    {
        respawn = GameObject.FindGameObjectWithTag("Respawn").transform;



        if (player != null && healthLast.activeInHierarchy)
        {
            player.position = destination.position;
        }
   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = null;
        }
    }
}