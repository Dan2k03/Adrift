using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 0.3f;

    [Tooltip("Set this to true if you want this GameObject to flip itself on the x-axis when it reaches the end of its patrol path.")]
    public bool flipOnDirectionChanged;

    private bool isRight = true;
    private Vector3 pointAPosition;
    private Vector3 pointBPosition;

    void Start()
    {
        pointAPosition = new Vector3(pointA.position.x, pointA.position.y, 0);
        pointBPosition = new Vector3(pointB.position.x, pointB.position.y, 0);
    }

    void Update()
    {
        Vector3 thisPosition = new(transform.position.x, transform.position.y, 0);

        if (isRight)
        {
            transform.position = Vector3.MoveTowards(transform.position, pointB.position, speed * Time.deltaTime);

            if (Vector2.Distance(thisPosition, pointBPosition) < 0.05f)
            {
                isRight = false;

                if (flipOnDirectionChanged)
                {
                    FlipByScale();
                }
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, pointA.position, speed * Time.deltaTime);

            if (Vector2.Distance(thisPosition, pointAPosition) < 0.05f)
            {
                isRight = true;

                if (flipOnDirectionChanged)
                {
                    FlipByScale();
                }
            }
        }
    }

    public void FlipByScale()
    {
        // Flip sprite by multiplying x by -1
        Vector3 objectScale = transform.localScale;
        objectScale.x *= -1;
        transform.localScale = objectScale;
    }
}