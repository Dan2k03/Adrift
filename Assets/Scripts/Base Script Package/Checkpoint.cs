using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    // Attach this to your checkpoints. Checkpoints should have a collider 2D set to trigger.

    [Tooltip("Drag in the respawn GameObject")]
    public Transform respawn;

    public bool singleUse = false;
    private bool used = false;
	
	private void Start()
    {
        if (respawn == null)
        {
            respawn = GameObject.FindGameObjectWithTag("Respawn").transform;
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (singleUse)
        {
            if (!used)
            {
                if (collision.CompareTag("Player"))
                {
                    respawn.transform.position = transform.position;
                }
            }
        }
        else
        {
            if (collision.CompareTag("Player"))
            {
                respawn.transform.position = transform.position;
            }
        }
    }
}
