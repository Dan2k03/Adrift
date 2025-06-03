using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    [Tooltip("Drag the hitbox object in here.")]
    [SerializeField] private Collider2D hitBox;

    [Tooltip("Drag the player's SpriteRenderer in here.")]
    [SerializeField] private SpriteRenderer playerSprite;

    [Tooltip("Optionally the player's animator in here.")]
    [SerializeField] private Animator animator;

    [Tooltip("Can type in what element we want to have")]
    [SerializeField] private string element;

    public AudioClip meleeSFX;

    public bool canMelee;
    public float hitBoxTime = 0.1f;
    //This one serves as a melee cooldown
    public float cooldown = 0.5f;
    public Collider2D leftBox;

    //Elemental Cooldowns and stuff
    public float basicElementAttackCooldown = 1f;



    private bool facingRight = true;
    //Matches to cooldown
    private float cooldownTimer = 0;
    private Coroutine meleeCoroutine;

    //Elemental Coroutines
    private Coroutine basicElemAttkCoroutine;

    //Elemental Ability cooldown timers
 
    //EA stands for Element Attack sorry for the crappy naming conventions, just need to shorten it for my sanity :(
    private float basicEACooldownTime = 0;


    private void Start()
    {
        hitBox.enabled = false;
        leftBox.enabled = false;

    }

    private void Update()
    {
        //if (facingRight)
        //{
            if (transform.localScale.x < 0)
            {
                facingRight = false;

               //transform.localPosition = new Vector3(-transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
            }
        //}
        else if (transform.localScale.x > 0)
        {
            facingRight = true;

            //transform.localPosition = new Vector3(-transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
        }

        if (Input.GetKeyDown(KeyCode.B) && basicEACooldownTime <= 0.02f)
        {
            UnityEngine.Debug.Log("B Pressed");
            if (basicElemAttkCoroutine != null)
            {
                StopCoroutine(basicElemAttkCoroutine);
            }

            basicElemAttkCoroutine = StartCoroutine(elementBasicAttackCoroutine());
        }


        if (Input.GetButtonDown("Fire1") && canMelee && cooldownTimer <= 0.02f)
        {
            if (meleeCoroutine != null)
            {
                StopCoroutine(meleeCoroutine);
            }

            meleeCoroutine = StartCoroutine(MeleeCoroutine());
        }


      

        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
            basicEACooldownTime -= Time.deltaTime;
        }
    }

    private IEnumerator MeleeCoroutine()
    {
        UnityEngine.Debug.Log("Pow!");
        if (animator != null)
        {
            animator.SetTrigger("Melee");
        }

        //AudioSource.PlayClipAtPoint(meleeSFX, transform.position, 0.5f);

        cooldownTimer = cooldown;
        if (facingRight)
        {
            hitBox.enabled = true;
        }
        else
        {
            leftBox.enabled = true;
        }
        yield return new WaitForSeconds(hitBoxTime);
        hitBox.enabled = false;
        leftBox.enabled = false;
    }

    private IEnumerator elementBasicAttackCoroutine()
    {
        if (animator != null)
        {
            //TODO: When we make animations, gotta change this. I have it as basic melee for test purposes :)
            animator.SetTrigger("Melee");
        }

        switch (element)
        {
            case "Fire":
                UnityEngine.Debug.Log("FireAttack");
                break;
            case "Lightning":
                UnityEngine.Debug.Log("LightningAttack");
                break;
            case "Water":
                UnityEngine.Debug.Log("WaterAttack");
                break;

        }


        //Make audio for this too... code is above in MeleeCoroutine, could be a way to do dynamic audio for the ultimate maybe?
        basicEACooldownTime = basicElementAttackCooldown;
        if (facingRight)
        {
            hitBox.enabled = true;
        }
        else
        {
            leftBox.enabled = true;
        }

        yield return new WaitForSeconds(hitBoxTime);
        hitBox.enabled = false;
        leftBox.enabled = false;
    }

    public void EnableMeleeAttack(bool _canMelee)
    {
        canMelee = _canMelee;
    }
}
