using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Update: MonoBehaviour
{
    public PlayerMovement playerMovement;

    [Tooltip("Pressing up or down makes the camera move in that direction! Useful if you want to stop and look around.")]
    public float yLookDistance = 1f;

    [Tooltip("The amount in the x-axis the camera is offset from the player")]
    public float xOffset = 0f;
    [Tooltip("The amount in the y-axis the camera is offset from the player")]
    public float yOffset = 0f;

    [Tooltip("This makes smooth camera movement. Higher speeds mean the camera follows the player more closely")]
    public float speed = 2.0f;
    
    void Start()
    {
        if (playerMovement == null)
        {
            playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        }
    }

    void FixedUpdate()
    {
        float interpolation = speed * Time.deltaTime;

        Vector3 position = this.transform.position;
        if (playerMovement || (position.y >= (playerMovement.transform.position.y + yOffset + playerMovement.moveY * yLookDistance)))
        {
            position.y = Mathf.Lerp(this.transform.position.y, playerMovement.transform.position.y + yOffset + playerMovement.moveY * yLookDistance, interpolation);
        }
        position.x = Mathf.Lerp(this.transform.position.x, playerMovement.transform.position.x + xOffset, interpolation);

        this.transform.position = position;
    }
}
