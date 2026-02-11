using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class MovePlayer : MonoBehaviour
{
    //Variables
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] float speed = 5f;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] int extraJumpValue = 1; 
    private int Direction; //left is 1, 0 is right
    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    private int extraJumps;
    internal int attackInput = 0;
    public bool isMoving;
    public LayerMask groundLayer;
    public Transform groundCheck;
    private bool isGrounded;
    private Animator animator;
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
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            attackInput = 1;
        }
         else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            attackInput = 0;
        }
        //This moves the player
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);
        if (moveInput > 0)
        {
            Direction = 0;
        }
        else if (moveInput < 0)
        {
            Direction = 1;
        }

        if (isGrounded)
        {
            extraJumps = extraJumpValue;
        }
        //Checks for jump input
        if (Input.GetKeyDown(KeyCode.Space)){
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
        SetAnimation(moveInput);
        FixedUpdate();
    }

    private void FixedUpdate()
    {
        //This checks if the player is on the ground by creating a circle at the position of the groundCheck object and checking if it overlaps with any colliders on the groundLayer
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void SetAnimation(float moveInput)
    {
        if (isGrounded)
        {
            if (moveInput == 0 && Direction == 0)
            {
                if (attackInput == 1)
                {
                    animator.Play("Attack_1_R");
                }
                else
                    animator.Play("PlayerAnimation");
            }
            else if (moveInput == 0 && Direction == 1)
            {
                    if (attackInput == 1)
                    {
                        animator.Play("Attack_1_L");
                    }
                else
                    animator.Play("Player_Idle_L");
            }
            else if (Direction == 1)
            {
                if (attackInput == 1) 
                { 
                    animator.Play("Attack_1_L");
                }
                else
                animator.Play("PlayerRun_L");
            }
            else
            {
                if (attackInput == 1)
                {
                    animator.Play("Attack_1_R");
                }
                else
                animator.Play("Player Run");
            }
        }
        else
        {
            if (rb.linearVelocity.y >= 0 && Direction == 0)
            {
                animator.Play("Player_Jump");
            }
            else if (rb.linearVelocity.y >= 0 && Direction == 1)
            {
                animator.Play("Player_Jump_L");
            }
            else if (Direction == 0)
            {
                animator.Play("Player_Fall");
            }
            else
            {
                animator.Play("Player_Fall_L");
            }
        }
    }
}
