using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Class init
    Animator playerAnimator;
    Rigidbody playerRigidBody;
    CapsuleCollider playerCollider;

    //Variables and constants
    public float horizontalInput = 0f;
    [SerializeField] private readonly float playerSpeed = 5f;
    [SerializeField] private readonly float playerSprintMultiplier = 1.5f;
    [SerializeField] private float jumpForce = 1000f;
    private Vector3 startPosition;
    private float comboTime = 0.75f;

    //Player states/statuses
    private bool isGrounded = true;
    private bool comboStatus = false;
    private float comboActiveTime = 0f;


    //Animator variables
    //--- booleans
    private bool isEnemyNoticed = false;
    private bool isSprinting = false;
    private bool isWalking = false;
    private bool isFalling = false;
    private bool isComboEnded = true;
    private bool isHavingSword = true;

    //--- triggers
    private bool isGoingToJump = false;
    private bool isGoingToAttack = false;


    // Start is called before the first frame update
    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        playerCollider = GetComponent<CapsuleCollider>();
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (comboStatus)
        {
            comboActiveTime -= Time.deltaTime;
            if (comboActiveTime <= 0f)
            {
                comboActiveTime = 0f;
                comboStatus = false;
                isComboEnded = true;
            }
        }
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            transform.position = startPosition;
        }
    }

    private void CheckInput()
    {
        //Jump
        if (JumpKey() && isGrounded)
        {
            isGoingToJump = true;
        }

        //Sword
        if (AttackSwordKey())
        {
            //AnimatorStateInfo animState = playerAnimator.GetCurrentAnimatorStateInfo(0);
            //float middleOfAnimation = 0.5f; 
            //if(animState.IsTag("Attack"))
            //{
            //    isGoingToAttack = (animState.normalizedTime >= middleOfAnimation);
            //}
            //else
            //{
                isGoingToAttack = true;
            //}             
        }

        //Sprint
        if (SprintKey() && isGrounded)
        {
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }
    }

    private void Attack()
    {
        GetComponent<SwordAttack>().Attack();
    }

    private void CheckIfGrounded()
    {
        float colliderRadius = playerCollider.radius;
        Vector3 colliderSphereCenter = playerCollider.bounds.min + new Vector3(colliderRadius, colliderRadius, colliderRadius);
        int playerMask = ~(1 << 6);
        //Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, 0.2f)
        if (Physics.CheckSphere(colliderSphereCenter, colliderRadius + 0.1f, playerMask))
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
        if (isGoingToJump)
        {
            if(isGrounded)
                playerRigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    
    private void CheckPlayerStatus()
    {
        if (isGrounded)
        {
            if (Math.Abs(horizontalInput) > 0.01f)
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

            if (playerRigidBody.velocity.y > 0)
            {
                isGoingToJump = true;
            }
            else
            {
                isGoingToJump = false;
            }
        } 
    }
    
    private void SetAnimationVariables()
    {
        //isAttacking
        if (isGoingToAttack)
        {
            playerAnimator.SetTrigger("Attack");
            Attack();
            isGoingToAttack = false;
            comboActiveTime = comboTime;
            comboStatus = true;
            isComboEnded = false;
        }
        //isJumping
        if (isGoingToJump)
        {
            playerAnimator.SetTrigger("Jump");
        }

        //isEnemyNoticed
        playerAnimator.SetBool("isEnemyNoticed", isEnemyNoticed);
        //isFalling
        playerAnimator.SetBool("isFalling", isFalling);
        //isGrounded
        //playerAnimator.SetBool("isGrounded", isGrounded);
        //isWalking
        playerAnimator.SetBool("isWalking", isWalking);
        //isSprinting
        playerAnimator.SetBool("isSprinting", isSprinting);
        //isHavingSword
        playerAnimator.SetBool("isHavingSword", isHavingSword);
        //isComboEnded
        playerAnimator.SetBool("isComboEnded", isComboEnded);
    }

    //KeyBindings

    private bool JumpKey()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    private bool AttackSwordKey()
    {
        return Input.GetKeyDown(KeyCode.Mouse0);
    }

    private bool SprintKey()
    {
        return Input.GetKey(KeyCode.LeftShift);
    }
}
