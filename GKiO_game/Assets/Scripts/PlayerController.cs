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
    private Vector3 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        startPosition = transform.position;
        //transform.rotation.SetRotation();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        SetAnimation();
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isGrounded = false;
            isJump = true;
        }
        
    }

    private void FixedUpdate()
    {
        TurnPlayer();
        float zVelocity = playerSpeed * horizontalInput;
        playerRigidBody.velocity = new Vector3(playerRigidBody.velocity.x, playerRigidBody.velocity.y, zVelocity);
        CheckAndJump();
    }

    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
    }
    private void TurnPlayer()
    {
        if(horizontalInput > 0)
        {
            //playerRigidBody.rotation.SetLookRotation(Vector3.forward);
            playerRigidBody.rotation = Quaternion.LookRotation(Vector3.forward);
        }
        else if(horizontalInput < 0){
            //playerRigidBody.rotation.SetLookRotation(Vector3.back);
            playerRigidBody.rotation = Quaternion.LookRotation(Vector3.back);
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
        if (isGrounded)
        {
            if (Math.Abs(playerRigidBody.velocity.z) > 0.01)
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
            if(playerRigidBody.velocity.y >= 0)
                playerAnimator.Play("Male Jump Up");
            else
                playerAnimator.Play("Male Fall");
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
