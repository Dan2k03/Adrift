using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossHealth : MonoBehaviour
{
    [Tooltip("How many times the enemy can get hit before being destroyed.")]
    [SerializeField] private int health = 3;
    [SerializeField] private int phase2Health = 2;

    [Tooltip("If true the sprite will flash red briefly when hit by a projectile.")]
    [SerializeField] private bool flashRedOnHit = true;

    [Tooltip("Drag in the enemy's SpriteRenderer component.")]
    [SerializeField] private SpriteRenderer sprite;

    [Tooltip("Optional sound effect when enemy is hit.")]
    [SerializeField] private AudioClip hitSoundEffect;

    [Tooltip("Optional sound effect when enemy is destroyed.")]
    [SerializeField] private AudioClip deathSoundEffect;

    [Tooltip("Optional: Drag in the enemy's animator for hit and death animations.")]
    [SerializeField] private Animator animator;

    [Tooltip("Delay (in seconds) before the enemy is destroyed after losing all health. Increase this if the enemy is being destroyed before their death animation fully plays.")]
    [SerializeField] private float delayBeforeDying = 0f;

    public GameObject drop;
    public Transform dropTransform;

    private Color originalColor;

    public Color damagedColor;

    public UnityEvent onPhase2, onDeath;

    void Start()
    {
        if (flashRedOnHit)
        {
            if (sprite)
            {
                originalColor = sprite.color;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("PlayerAttack"))
        {
            Destroy(collision.gameObject);
            HitByProjectile();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack"))
        {
            HitByProjectile();
        }
    }

    private void HitByProjectile()
    {
        health--;

        if (health <= 0)
        {
            onDeath.Invoke();

            if (deathSoundEffect)
            {
                AudioSource.PlayClipAtPoint(deathSoundEffect, transform.position);
            }

            if (animator)
            {
                animator.SetTrigger("Death");
            }

            StartCoroutine(DeathDelay(delayBeforeDying));
        }
        else
        {
            if (health == phase2Health)
            {
                onPhase2.Invoke();
            }

            if (hitSoundEffect)
            {
                AudioSource.PlayClipAtPoint(hitSoundEffect, transform.position);
            }

            if (animator)
            {
                animator.SetTrigger("Hit");
            }
        }

        if (flashRedOnHit)
        {
            StartCoroutine(FlashRed());
        }
    }

    private IEnumerator FlashRed()
    {
        sprite.color = damagedColor;
        yield return new WaitForSeconds(0.1f);
        sprite.color = originalColor;
    }

    private IEnumerator DeathDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        if (drop != null && dropTransform != null)
        {
            Instantiate(drop, dropTransform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
