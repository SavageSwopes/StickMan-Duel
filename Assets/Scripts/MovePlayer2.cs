using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D;


public class MovePlayer2 : MonoBehaviour
{
    [Header("Player Component References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Player Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpingPower;
    private int extraJumps = 1;
    [SerializeField] private float fallSpeedMultiplier;
    [SerializeField] private float baseGravity;

    [Header("Grounding")]
    [SerializeField] private float groundCheckRadius = 0.5f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;

    [Header("Animation Settings")]
    private SpriteRenderer sprite;
    private Animator animator;
    private int Direction;
    public bool isWalking;

    [Header("Attack Settings")]
    [SerializeField] private BoxCollider2D attackHitbox;
    [SerializeField] private float attackTimer = 0.39f;
    [SerializeField] float comboResetTime = .7f; // Time window to reset the combo
    internal int attackInput = 0;
    private int comboStep = 0;
    private float lastAttackTime = 0f;
    private bool isAttacking = false;


    private float horizontal;

    //Test
    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        attackHitbox.enabled = false; // Ensure hitbox is disabled at the start
    }

    //Test
    private void Update()
    {
        if (Time.time - lastAttackTime > comboResetTime)
        {
            comboStep = 0; // Reset combo if time since last attack exceeds reset time
        }
        isGrounded();
        
    }


    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
    }

    #region Player_Controls
    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
        isWalking = true;
        //animator.SetBool("isWalking", isWalking);

        //SetAnimation(horizontal);
    }
    /// <summary>
    /// This is for player jumping.
    /// </summary>
    /// <param name="context"></param>
    public void Jump(InputAction.CallbackContext context)
    {
        if (extraJumps > 0)
        {
            if (context.performed)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
                extraJumps--;
            }          
        }
        //SetJumpAnimation();
    }

    private bool isGrounded()
    {
        
        if (Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer))
        {
            extraJumps = 1;
        }
        return false;
    }

    //Test
    public void Fire(InputAction.CallbackContext context)
    {
        if (!isAttacking && isGrounded())
        {
            //StartCoroutine(PerformComboAttack());
        }
    }
    #endregion


    //Test
    private IEnumerator PerformComboAttack()
    {
        isAttacking = true;
        lastAttackTime = Time.time; // Update last attack time
        comboStep++;
        if (comboStep > 2)
        {
            comboStep = 1; // Loop back to the first attack in the combo
        }

        if (comboStep == 1)
        {
            if (Direction == 0)
            {
                animator.Play("Attack_1_R");
            }
            else
            {
                animator.Play("Attack_1_L");
            }
        }
        else if (comboStep == 2)
        {
            if (Direction == 0)
            {
                animator.Play("Attack_2_R");
            }
            else
            {
                animator.Play("Attack_2_L");
            }
        }
        yield return new WaitForSeconds(attackTimer); ;
        isAttacking = false;
    }

    //Test
    private void SetAnimation(float horizontal)
    {
        // PRIORITY CHECK: If we are attacking, do not play other animations!
        if (isAttacking) return;

        if (isGrounded())
        {
            if (horizontal == 0)
            {
                // Idle Logic
                if (Direction == 0)
                {

                    animator.Play("PlayerAnimation"); // Right Idle
                }
                else
                {
                    animator.Play("Player_Idle_L"); // Left Idle
                }
            }
            else
            {
                // Run Logic
                if (Direction == 0)
                {
                    animator.Play("Player Run"); // Right Run
                }
                else
                {
                    animator.Play("PlayerRun_L"); // Left Run
                }
            }
        }
    }

    private void SetJumpAnimation()
    {
          //This checks if the player is moving upwards and facing right, if so it plays the jump animation for right
            if (rb.linearVelocity.y >= 0)
            {
                if (Direction == 0)
                {
                    animator.Play("Player_Jump");
                }
                else
                {
                    animator.Play("Player_Jump_L");
                }
            }
            else if (Direction == 0)
            //This checks if the player is moving downwards and facing right, if so it plays the fall animation for right
            {
                animator.Play("Player_Fall");
            }
            else
            {
                animator.Play("Player_Fall_L");
            }
    }
}