using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stormfang_Phase2 : MonoBehaviour
{
    //Things the player can change, names should be self explanatory
    public Transform player;
    public float moveSpeed = 5f;
    public float dashSpeed = 5f;
    public int maxBasicDashed = 5;
    public float dashCooldown = 1f;
    public float followYDurr = 5f;
    public float dashDurr = 0.25f;
    public float spinSpeed = 180f; 
    public float spinDuration = 10f;
    public int numChainDashes = 6; //The number of times the boss dashes during his dash phase
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private GameObject projectilePrefab;
    public Transform startPoint;

    public enum BossState
    {
        Start,
        SpinAttack,
        LightningStrikes,
        BreathAttack,
        FollowY,
        BasicDash,
        ChainDash
    }

    //Private things
    private BossState currState = BossState.FollowY;
    private float stateTimer;
    private float dashTimer;
    private int chainDashCount;
    private bool isDashing = false;
    private bool isLeft = true;
    private bool isChainDashing = false;
    private int regStateCount = 0;
    private Color originalColor;
    private Transform midpoint;
    private Quaternion originalRot;
    private float cooldownTime;
    private bool targetSet = false;
    private int prev;
    private Vector2 dashTarget;

    private Vector2 dashDir;

    void Start()
    {
        midpoint = GameObject.FindGameObjectWithTag("Midpoint").transform;
        stateTimer = followYDurr;
        dashTimer = dashDurr;
        chainDashCount = 0;
        currState = BossState.FollowY;
        originalColor = sprite.color;
        originalRot = transform.rotation;
        cooldownTime = dashCooldown;
        prev = -1;
        stateChanger();
        spin();
    }

    // Update is called once per frame
    void Update()
    {
        stateTimer -= Time.deltaTime;
        if (isDashing == true)
        {
            dashTimer -= Time.deltaTime;
        }


        switch (currState)
        {
            case BossState.FollowY:
                if (stateTimer >= 0.05f * followYDurr)
                {
                    followPlayer();
                }
                break;
            case BossState.BasicDash:
                if (!isLeft)
                {
                    basicDash(Vector2.right);
                }
                else
                {
                    basicDash(Vector2.left);
                }
                isDashing = true;
                break;
            case BossState.ChainDash:
                chainDashManager();
                break;
        }

        if (isChainDashing == false)
        {
            if (stateTimer <= 0.15f * followYDurr)
            {
                StartCoroutine(FlashBlue());
            }

            if (stateTimer <= 0f || dashTimer <= 0f || regStateCount == maxBasicDashed)
            {
                stateChanger();
            }
        }
        else
        {
            if (chainDashCount == numChainDashes)
            {
                stateChanger();
            }
        }
    }

    void followPlayer()
    {
        Vector2 targetPosition = new Vector2(transform.position.x, player.position.y);
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    void basicDash(Vector2 direction)
    {
        dashDir = direction;
        transform.position += (Vector3)dashDir * dashSpeed * Time.deltaTime;
    }

    void chainDash(Vector2 direction)
    {
        dashDir = direction.normalized;
        float angle = Mathf.Atan2(dashDir.y, dashDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
        transform.position += (Vector3)dashDir * dashSpeed * Time.deltaTime;

    }

    void stateChanger()
    {
        if (currState == BossState.FollowY && regStateCount != maxBasicDashed)
        {

            currState = BossState.BasicDash;

        }
        else if (currState == BossState.FollowY && regStateCount == maxBasicDashed)
        {
            currState = BossState.ChainDash;
            isChainDashing = true;
            prev = -1;

        }
        else if (currState == BossState.BasicDash)
        {

            if (transform.position.x < midpoint.position.x)
            {
                isLeft = false;
                Vector3 scale = transform.localScale;
                scale.x *= -1; // Invert the X-axis
                transform.localScale = scale;
            }
            else
            {
                isLeft = true;
                Vector3 scale = transform.localScale;
                scale.x *= -1; // Invert the X-axis
                transform.localScale = scale;
            }
            regStateCount++;
            currState = BossState.FollowY;
            isDashing = false;
        }
        else if (currState == BossState.ChainDash)
        {
            returnToStart();

            if (transform.position.x < midpoint.position.x)
            {
                isLeft = false;
            }
            else
            {
                isLeft = true;
            }
            chainDashCount = 0;
            isChainDashing = false;
            regStateCount = 0;
            currState = BossState.FollowY;
        }

        stateTimer = followYDurr;
        dashTimer = dashDurr;
    }

    void chainDashManager()
    {
        float stopdistance = 0.1f;

        if (cooldownTime <= 0f && chainDashCount < numChainDashes)
        {

            if (!targetSet)
            {
                dashTarget = player.position;
                targetSet = true;
            }


            Vector2 pos = dashTarget - (Vector2)transform.position;

            if (pos.magnitude > stopdistance)
            {
                chainDash(pos);
            }
            else
            {
                pos = Vector2.zero;
            }

            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0f)
            {
                chainDashCount++;
                dashTimer = dashDurr;
                cooldownTime = dashCooldown;
                targetSet = false;
            }
        }
        else
        {
            cooldownTime -= Time.deltaTime;
            if (cooldownTime <= dashCooldown * 0.25)
            {
                StartCoroutine(FlashBlue());
            }
            targetSet = false;
        }
    }

    void spin()
    {
        float timeElapsed = 0f;
        Vector2 pos = (Vector2)midpoint.position - (Vector2)transform.position;

        while (Vector3.Distance(transform.position, startPoint.position) > 0.1f)
        {
            dashDir = pos.normalized;
            float angle = Mathf.Atan2(dashDir.y, dashDir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
            transform.position += (Vector3)dashDir * dashSpeed * Time.deltaTime;
        }

        while (timeElapsed < spinDuration)
        {
            // Rotate the boss around its y-axis (spinning in place)
            transform.Rotate(Vector3.up * spinSpeed * Time.deltaTime);
            timeElapsed += Time.deltaTime;
        }

    }

    void returnToStart()
    {
        while (Vector3.Distance(transform.position, startPoint.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPoint.position, 0.1f * Time.deltaTime);
        }
        transform.rotation = originalRot;
    }

    private IEnumerator FlashBlue()
    {
        sprite.color = Color.blue;
        yield return new WaitForSeconds(0.1f);
        sprite.color = originalColor;

    }

}
