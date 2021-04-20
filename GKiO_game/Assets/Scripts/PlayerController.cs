using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRigidBody;
    private Animator playerAnimator;
    public float horizontalInput = 0f;
    [SerializeField] private readonly float playerSpeed = 2f;
    [SerializeField] private float jumpForce = 1000f;
    private bool isGrounded = true;
    private bool isJump = false;
    private bool isAttack = false;
    private Vector3 startPosition;
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
        SetAnimation();
        if (JumpKeyDown() && isGrounded)
        {
            isJump = true;
        }
        if (SwordKeyDown() && !isAttack)
        {
            isAttack = true;
            Invoke("AttackEnd", 1.1f);
        }
        
    }

    private void AttackEnd()
    {
        isAttack = false;
    }

    private bool JumpKeyDown()
    {
        return Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W);
    }

    private bool SwordKeyDown()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    private void FixedUpdate()
    {
        TurnPlayer();
        CheckIfGrounded();
        CalculateVelocity();
        CheckAndJump();
    }

    private void CheckIfGrounded()
    {
        Debug.DrawRay(transform.position, Vector3.down, Color.white);
        if(Physics.Raycast(transform.position + Vector3.up * 0.04f, Vector3.down, 0.08f))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //isGrounded = true;
    }

    private void CalculateVelocity()
    {
        float xVelocity = playerSpeed * horizontalInput;
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
        if (isJump)
        {
            isJump = false;
            playerRigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void SetAnimation()
    {
        if (isAttack)
        {
            playerAnimator.Play("Male Attack 1");
        }
        else
        {
            if (isGrounded)
            {
                if (Math.Abs(horizontalInput) > 0.01)
                {
                    playerAnimator.Play("Male_Walk");
                }
                else
                {
                    playerAnimator.Play("Male Idle");
                }
            }
            else
            {
                if (playerRigidBody.velocity.y >= 0)
                    playerAnimator.Play("Male Jump Up");
                else
                    playerAnimator.Play("Male Fall");
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 8)
        {
            transform.position = startPosition;
        }
    }

}
