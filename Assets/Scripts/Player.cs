using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

// there was a different way to stick to walls shown in class, but I couldn't keep up or find it

public class Player : MonoBehaviour
{
    //bendux 2D Player Movement In Unity
    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 7f;
    private bool isFacingRight = true;

    //bendux wall slide and wall jump in unity
    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;

    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private UnityEngine.Vector2 wallJumpingPower = new UnityEngine.Vector2(4f, 8f);


    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;




    void Update() {
        //variable stores left and right movement
        horizontal = Input.GetAxisRaw("Horizontal");

        //jumping. the longer jump button is held down, the bigger the jump is. jumpingPower is the max, jumps can be shorter than it with short presses.
        if (Input.GetButtonDown("Jump") && IsGrounded()) {
            rb.velocity = new UnityEngine.Vector2(rb.velocity.x, jumpingPower);
        }
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f) {
            rb.velocity = new UnityEngine.Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        WallSlide();
        WallJump();

        // flip is called when not wall jumping so wall jump can work.
        if (!isWallJumping) {
            Flip();        
        }
    }

    
    //Capitalisation the start of methods is important  
    
    //checks if player is touching ground
    private bool IsGrounded() {
        //creates a 0.2f radius circle where groundCheck is and checks if groundLayer touches it.
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    //checks if player is touching a wall
    private bool IsWalled() {
        //same as isGrounded but for wall.
        //wallCheck is on the right side of the player as player faces right by default.
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }


    //Not entirely sure how to explain WallSlide or WallJump, will come back to them later
    private void WallSlide() {
        if (IsWalled() && !IsGrounded() && horizontal != 0f) {
            isWallSliding = true;
            rb.velocity = new UnityEngine.Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else {
            isWallSliding = false;
        }
    }

    private void WallJump() {
        if (isWallSliding) {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;
        
            CancelInvoke(nameof(StopWallJumping));
        }
        else {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f) {
            isWallJumping = true;   
            rb.velocity = new UnityEngine.Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection) {
                isFacingRight = !isFacingRight;
                UnityEngine.Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);     
        }
    }

    private void StopWallJumping() {
        isWallJumping = false;

    }

    //this is where player movement actually happens. 
    //we don't let the player control movement during wall jumping, 
    //the WallJump method pushes the player off the wall when they jump, so it has its own movement code.
    private void FixedUpdate() {
        if (!isWallJumping) {
            rb.velocity = new UnityEngine.Vector2(horizontal * speed, rb.velocity.y);
        }
    }

    //changes player's direction in the horizontal axis.
    private void Flip() {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f) {
            isFacingRight = !isFacingRight;
            UnityEngine.Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

}
