using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMovement : MonoBehaviour
{
    public Animator animator;

    [SerializeField]private float horizontal;
    [SerializeField]private float vertical;
    [SerializeField]private float speed = 8f;
    [SerializeField]private float jumpingPower = 16f;
    private bool isFacingRigt = true;
    private bool isJumping = false;
    private bool isCrouching = false;

    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;

    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    [SerializeField]private Vector2 wallJumpingPower = new Vector2(20f, 32f);

    private bool isWallRunning = false;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Speed", Math.Abs(horizontal));

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            isJumping = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            animator.SetBool("IsJumping", isJumping);
        }

        if(Input.GetButtonDown("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
        /*
        if (Input.GetButtonDown("Crouch"))
        {
            isCrouching = true;
            animator.SetBool("IsCrouching", isCrouching);
        }*/





        WallJump();
        
        WallSlide();

        Flip();

        //WallRun();

        /*
        if (IsGrounded())
        {
            isJumping = false;
            //animator.SetBool("IsJumping", isJumping);
        }
        */
    }

    private void Flip()
    {
        if(isFacingRigt && horizontal < 0f || !isFacingRigt && horizontal > 0f)
        {
            isFacingRigt = !isFacingRigt;
            transform.Rotate(0f, 180f, 0f);
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        //throw new NotImplementedException();
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && horizontal != 0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
            
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -horizontal;//-transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;
            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if(transform.localScale.x != wallJumpingDirection)
            {
                isFacingRigt = !isFacingRigt;
                transform.Rotate(0f, 180f, 0f);
                /*
                localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
                */
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void WallRun()
    {
        if(IsWalled())// && !IsGrounded()  && horizontal!= 0f
        {
            //isWallRunning = true;
            rb.velocity = new Vector2(rb.velocity.x, vertical * speed * Time.deltaTime);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            isJumping = false;
            animator.SetBool("IsJumping", false);
        }
    }
}
