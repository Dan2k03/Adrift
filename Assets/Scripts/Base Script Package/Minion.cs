using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour
{
    public float speed = 3f;
    public Transform followTransform;
    public bool autoFollowPlayer;
    public float stunTime = 0f;
    private float stunnedTimer;

    void Start()
    {
        if (autoFollowPlayer)
        {
            followTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {
        if (followTransform != null && stunnedTimer < 0.01)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(followTransform.position.x, followTransform.position.y), Time.deltaTime * speed);
        }

        if (stunnedTimer > 0)
        {
            stunnedTimer -= Time.deltaTime;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("PlayerAttack"))
        {
            stunnedTimer = stunTime;
        }
    }
}
