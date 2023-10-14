using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

// there was a different way to stick to walls shown in class, but I couldn't keep up or find it

public class Player : MonoBehaviour
{
    //bendux 2D Player Movement In Unity
    private float horizontal;
    private float speed = 6f;
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
    private UnityEngine.Vector2 wallJumpingPower = new UnityEngine.Vector2(9f, 7f);


    [SerializeField] private Rigidbody2D rb;
    
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;




    void Update() {
        //horizontal stores -1 if moving left, 1 if moving right, 0 if no direction
        horizontal = Input.GetAxisRaw("Horizontal");

        //jumping. the longer jump button is held down, the bigger the jump is. jumpingPower is the max, 
        //jumps can be shorter than it with short presses.
        if (Input.GetButtonDown("Jump") && IsGrounded()) {
            rb.velocity = new UnityEngine.Vector2(rb.velocity.x, jumpingPower);
        }
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f) {
            rb.velocity = new UnityEngine.Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        WallSlide();
        WallJump();

        //flip is called when not wall jumping so wall jump can work.
        if (!isWallJumping) {
            Flip();        
        }
    }

    private void FixedUpdate() {
        //player doesn't control horizontal movement during wall jumping, WallJump has its own movement code.
        if (!isWallJumping) {
            //this is where horizontal player movement happens 
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


    
    //Capitalisation the start of methods is important  
    

    private bool IsGrounded() {
        //creates a 0.2f radius circle where groundCheck is and checks if groundLayer touches it.
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private bool IsWalled() {
        //same as isGrounded but for wall.
        //wallCheck is on the right side of the player as player faces right by default.
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }


    private void WallSlide() {
        // if the player moves into a wall while in the air,
        // they wall slide and can wall jump
        if (IsWalled() && !IsGrounded() && horizontal != 0f) {
            isWallSliding = true;
            //
            rb.velocity = new UnityEngine.Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else {
            isWallSliding = false;
        }
    }


    private void WallJump() {
        if (isWallSliding) {
            isWallJumping = false;
            //stores the direction the player should face
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;
        
            CancelInvoke(nameof(StopWallJumping));
        }
        else {
            //time limit on wall jump. ensures player can't spam and constantly wall jump with that
            wallJumpingCounter -= Time.deltaTime;
        }

        
        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f) {
            isWallJumping = true;
            //player wall jump changes both x and y   
            rb.velocity = new UnityEngine.Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;
            
            //ensures that player is facing correct direction for the wall jump
            if (transform.localScale.x != wallJumpingDirection) {
                isFacingRight = !isFacingRight;
                UnityEngine.Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            //Waits for the length of WallJumpingDuration and then calls StopWallJumping
            Invoke(nameof(StopWallJumping), wallJumpingDuration);     
        }
    }

    private void StopWallJumping() {
        isWallJumping = false;
    }
}
