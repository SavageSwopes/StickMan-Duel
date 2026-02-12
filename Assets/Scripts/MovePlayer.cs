using System.Runtime.CompilerServices;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class MovePlayer : MonoBehaviour
{
    //adjustable movement and jump variables
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] float speed = 5f;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] int extraJumpValue = 1;

    //Movement variables
    private int extraJumps;
    public bool isMoving;

    // Ground check variables
    public LayerMask groundLayer;
    public Transform groundCheck;
    private bool isGrounded;

    // Animation variables
    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    private Animator animator;
    private int Direction;  //left is 1, 0 is right

    // attack variables
    [SerializeField] private float attackTimer = 0.23f;
    internal int attackInput = 0;
    private int comboStep = 0;
    private float lastAttackTime = 0f;
    public float comboResetTime = 1f; // Time window to reset the combo
    private bool isAttacking = false;


    private void Start()
    {
        //Component references
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        extraJumps = extraJumpValue;
    }

    private void Update()
    {
        //This gets the input from the player, works with the Input Manager
        float moveInput = Input.GetAxisRaw("Horizontal");

        //This moves the player
        if (isAttacking)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // Stop horizontal movement while attacking
        }
        else
        {
            rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);
            if (moveInput > 0)
            {
                Direction = 0;
            }
            else if (moveInput < 0)
            {
                Direction = 1;
            }
        }


        /*if (Input.GetButtonDown("Fire1") && !isAttacking && isGrounded)
        {
            StartCoroutine(PerformAttack());
        }
        if (Input.GetButtonDown("Fire1") && !isAttacking)
        {
            StartCoroutine(PerformAttack2());
        }*/

        if (isGrounded)
        {
            extraJumps = extraJumpValue;
        }

        //Checks for jump input
        if (Input.GetKeyDown(KeyCode.Space) && !isAttacking)
        {
            if (isGrounded)
            {
                //Jumping code
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
            else if (extraJumps > 0)
            {
                //Extra jump code
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                extraJumps--;
            }
        }

        if (Time.time - lastAttackTime > comboResetTime)
        {
            comboStep = 0; // Reset combo if time since last attack exceeds reset time
        }
        if (Input.GetButtonDown("Fire1") && !isAttacking && isGrounded)
        {
            StartCoroutine(PerformComboAttack());
        }


        SetAnimation(moveInput);
        FixedUpdate();
    }
    

    private IEnumerator PerformComboAttack()
    {
          isAttacking= true;
        lastAttackTime = Time.time; // Update last attack time
        comboStep++;
        Debug.Log("Combo Step: " + comboStep);
        if (comboStep > 2)
        {
            comboStep = 1; // Loop back to the first attack in the combo
        }

        if (comboStep == 1)
        {
            if (Direction == 0)
            {
                Debug.Log("Performing Attack 1 Right");
                animator.Play("Attack_1_R");
            }
            else
            {
                Debug.Log("Performing Attack 1 Left");
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
    //This is a coroutine that handles the attack animation and timing
    IEnumerator PerformAttack()
    {
        isAttacking = true;
        if (Direction == 0)
        {
            animator.Play("Attack_1_R");
        }
        else
        {
            animator.Play("Attack_1_L");
        }
        yield return new WaitForSeconds(attackTimer);
        isAttacking = false;
    }

    IEnumerator PerformAttack2()
    {
        isAttacking = true;
        if (Direction == 0)
        {
            animator.Play("Attack_2_R");
        }
        /*
        else
        {
            animator.Play("Attack_1_L");
        }*/
        yield return new WaitForSeconds(attackTimer);
        isAttacking = false;
    }
    private void FixedUpdate()
    {
        //This checks if the player is on the ground by creating a circle at the position of the groundCheck object and checking if it overlaps with any colliders on the groundLayer
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void SetAnimation(float moveInput)
    {
        // PRIORITY CHECK: If we are attacking, do not play other animations!
        if (isAttacking) return;

        if (isGrounded)
        {
            if (moveInput == 0)
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
        else
        {   //This checks if the player is moving upwards and facing right, if so it plays the jump animation for right
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
        /*
        private void SetAnimation(float moveInput)
        {
            if (isGrounded)
            {
                if (moveInput == 0 && Direction == 0)
                {  //This checks if the player is idle and facing right, if so it plays the idle animation for right
                    if (attackInput == 1)
                    {
                        animator.Play("Attack_1_R");

                    }
                    else //If not attacking, play the idle animation for right
                        animator.Play("PlayerAnimation");
                }
                else if (moveInput == 0 && Direction == 1)
                {       //This checks if the player is idle and facing left, if so it plays the idle animation for left
                    if (attackInput == 1)
                    {
                        animator.Play("Attack_1_L");
                    }
                    else //If not attacking, play the idle animation for left
                        animator.Play("Player_Idle_L");
                }
                else if (Direction == 1)
                {  //This checks if the player is moving left and facing left, if so it plays the run animation for left
                    if (attackInput == 1)
                    {
                        animator.Play("Attack_1_L");
                    }
                    else //If not attacking, play the run animation for left
                        animator.Play("PlayerRun_L");
                }
                else
                {   //This checks if the player is moving right and facing right, if so it plays the run animation for right
                    if (attackInput == 1)
                    {
                        animator.Play("Attack_1_R");
                    }
                    else //If not attacking, play the run animation for right
                        animator.Play("Player Run");
                }
            }
            else
            {   //This checks if the player is moving upwards and facing right, if so it plays the jump animation for right
                if (rb.linearVelocity.y >= 0 && Direction == 0)
                {
                    animator.Play("Player_Jump");
                }
                //This checks if the player is moving upwards and facing left, if so it plays the jump animation for left
                else if (rb.linearVelocity.y >= 0 && Direction == 1)
                {
                    animator.Play("Player_Jump_L");
                } //This checks if the player is moving downwards and facing right, if so it plays the fall animation for right
                else if (Direction == 0)
                {
                    animator.Play("Player_Fall");
                }
                else
                {
                    animator.Play("Player_Fall_L");
                }
            }
        } */
    }
