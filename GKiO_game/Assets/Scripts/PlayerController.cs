using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRigidBody;
    private Animator playerAnimator;
    public float horizontalInput = 0f;
    [SerializeField] private readonly float playerSpeed = 5f;
    [SerializeField] private readonly float playerSprintMultiplier = 1.5f;
    [SerializeField] private float jumpForce = 1000f;    
    private Vector3 startPosition;

    // Animator variables
    private bool isGrounded = true;
    private bool isAttacking = false;
    private bool isEnemyNoticed = false;
    private bool isFalling = false;
    private bool isJumping = false;
    private bool isSprinting = false;
    private bool isWalking = false;


    // Start is called before the first frame update
    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        CheckIfGrounded();
        CheckPlayerStatus();        
        CheckInput();
    }

    private void FixedUpdate()
    {
        TurnPlayer();
        CalculateVelocity();
        CheckAndJump();
        SetAnimationVariables();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //isGrounded = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            transform.position = startPosition;
        }
    }

    private void CheckInput()
    {
        // Jump 
        if (JumpKeyDown() && isGrounded)
            isJumping = true;

        // Sword
        if (SwordKeyDown())
            isAttacking = true;
        else
            isAttacking = false;

        // Sprint
        if (SprintKeyDown() && isGrounded)
            isSprinting = true;
        else
            isSprinting = false;
    }

    private void CheckIfGrounded()
    {
        Debug.DrawRay(transform.position, Vector3.down, Color.white);
        if (Physics.Raycast(transform.position + Vector3.up * 0.04f, Vector3.down, 0.08f))
        {
            isGrounded = true;
            isFalling = false;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void CalculateVelocity()
    {
        float xVelocity = playerSpeed * horizontalInput;
        if (isSprinting)
        {
            xVelocity *= playerSprintMultiplier;
        }
        playerRigidBody.velocity = new Vector3(xVelocity, playerRigidBody.velocity.y, playerRigidBody.velocity.z);
    }
    
    private void TurnPlayer()
    {
        if(horizontalInput > 0)
        {
            playerRigidBody.rotation = Quaternion.LookRotation(Vector3.right);
        }
        else if(horizontalInput < 0){
            playerRigidBody.rotation = Quaternion.LookRotation(Vector3.left);
        }
    }
    
    private void CheckAndJump()
    {
        if (isJumping)
        {
            if(isGrounded)
                playerRigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            else if(playerRigidBody.velocity.y <= 0)
                isJumping = false;
        }
    }
    
    private void CheckPlayerStatus()
    {
        if (isGrounded)
        {
            if (Math.Abs(horizontalInput) > 0.01)
            {
                isWalking = true;
            }
            else
            {
                isWalking = false;
            }
        }
        else
        {
            if (playerRigidBody.velocity.y < 0)
            {
                isFalling = true;
            }
            else
            {
                isFalling = false;
            }
        } 
    }
    
    private void SetAnimationVariables()
    {
        //isAttacking
        playerAnimator.SetBool("isAttacking", isAttacking);
        //isEnemyNoticed
        playerAnimator.SetBool("isEnemyNoticed", isEnemyNoticed);        
        //isFalling
        playerAnimator.SetBool("isFalling", isFalling);
        //isGrounded
        playerAnimator.SetBool("isGrounded", isGrounded);
        //isJumping
        playerAnimator.SetBool("isJumping", isJumping);
        //isWalking
        playerAnimator.SetBool("isWalking", isWalking);
        //isSprinting
        playerAnimator.SetBool("isSprinting", isSprinting);
    }
    
    private bool JumpKeyDown()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }
    
    private bool SwordKeyDown()
    {
        //return Input.GetKeyDown(KeyCode.Space);
        //return Input.GetKey(KeyCode.Space);
        return Input.GetKey(KeyCode.Mouse0);
    }
   
    private bool SprintKeyDown()
    {
        return Input.GetKey(KeyCode.LeftShift);
    }
}
