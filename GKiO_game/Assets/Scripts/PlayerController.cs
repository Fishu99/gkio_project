using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerController : MonoBehaviour
{
    //Class init
    Animator playerAnimator;
    Animator bowAnimator;
    Rigidbody playerRigidBody;
    CapsuleCollider playerCollider;
    AudioManager audioManager;
    public Camera playerCamera;

    //Variables and constants
    public float horizontalInput = 0f;
    [SerializeField] private float playerSpeed = 5f;
    [SerializeField] private float playerSprintMultiplier = 2.0f;
    [SerializeField] private float jumpForce = 1000f;
    private Vector3 startPosition;
    private float comboTime = 0.75f;
    private int comboNumber = 0;
    [SerializeField] private GameObject arrowPrefab;
    private float arrowOriginRadius = 1.3f;

    //Player states/statuses
    private bool isGrounded = true;
    private bool comboStatus = false;
    private float comboActiveTime = 0f;
    private bool isGoingToJump = false;
    private bool didPlayerJustJumped = false;
    private bool isCrouching = false;
    private int playerScore = 0;

    //For audio
    private int stepNumber = 1;
    private int stepPreviousNumber = 4;


    //Animator variables
    //--- booleans
    private bool isEnemyNoticed = false;
    private bool isSprinting = false;
    private bool isWalking = false;
    private bool isFalling = false;
    private bool isComboEnded = true;
    private bool isHavingSword = true;
    private bool isShooting = false;

    //--- triggers
    private bool isJumping = false;
    private bool isGoingToAttack = false;
    private bool isGoingToShoot = false;

    private GameObject sword;
    private GameObject bow;

    private GameObject setup;
    private GameObject target;

    GameObject ARROW;


    // Start is called before the first frame update
    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        playerCollider = GetComponent<CapsuleCollider>();
        audioManager = FindObjectOfType<AudioManager>();
        startPosition = transform.position;

        sword = GameObject.Find("Sword_1");
        bow = GameObject.Find("Wooden Bow");

        ChangeWeaponToBow();

        setup = GameObject.Find("Rig");
        var builder = FindObjectOfType<RigBuilder>();
        builder.enabled = false;

        target = GameObject.Find("Target");

        ARROW = GameObject.Find("Arrow");
        ARROW.SetActive(false);

        bowAnimator = bow.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isShooting)
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 center = playerCollider.bounds.center + Vector3.up / 2;
            Vector3 screenPlayerPosition = playerCamera.WorldToScreenPoint(center);
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, mousePosition - screenPlayerPosition);
            Vector3 radius = Vector3.up * arrowOriginRadius;
            Vector3 position = center + rotation * radius;
            target.transform.SetPositionAndRotation(position, rotation);
            var rig = setup.GetComponent<Rig>();
            rig.weight = 0.9f;
            var builder = FindObjectOfType<RigBuilder>();
            builder.enabled = true;
        }
        else
        {
            var builder = FindObjectOfType<RigBuilder>();
            builder.enabled = false;
        }
        if (comboStatus)
        {
            comboActiveTime -= Time.deltaTime;
            if (comboActiveTime <= 0f)
            {
                comboActiveTime = 0f;
                comboStatus = false;
                comboNumber = 0;
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
        CheckAndJump();
        CalculateVelocity();
        SetAnimationVariables();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            transform.position = startPosition;
        }
        if (other.gameObject.CompareTag("Collectibles"))
        {
            Destroy(other.gameObject);
            playerScore++;
            Debug.Log("Player score is: " + playerScore.ToString());
        }
    }

    private void CheckInput()
    {
        //Jump
        if (JumpKey() && isGrounded)
        {
            isGoingToJump = true;
        }

        //Crouch
        if (CrouchKey())
        {
            if (!isCrouching)
            {
                isCrouching = true;
            }        
        }
        else
        {
            if (isCrouching)
            {
                isCrouching = false;
            }
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

        //Bow&Arrow
        if (ShootKey())
        {
            isGoingToShoot= true;
            ARROW.SetActive(true);
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

    private void Shoot()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 center = playerCollider.bounds.center;
        Vector3 screenPlayerPosition = playerCamera.WorldToScreenPoint(center);
        Quaternion arrowRotation = Quaternion.FromToRotation(Vector3.up, mousePosition-screenPlayerPosition);
        Vector3 radius = Vector3.up * arrowOriginRadius;
        Vector3 arrowPosition = center + arrowRotation * radius;
        ARROW.SetActive(false);
        GameObject arrow = Instantiate(arrowPrefab, arrowPosition, arrowRotation);
        arrow.GetComponent<ArrowController>().Shoot();
        isShooting = false;
    }

    private void Attack()
    {
        GetComponent<SwordAttack>().Attack();
        if (comboNumber.Equals(0))
            audioManager.Play("PlayerSwordAttack1");
        else if (comboNumber.Equals(1))
            audioManager.Play("PlayerSwordAttack2");
        else if (comboNumber.Equals(2))
            audioManager.Play("PlayerSwordAttack3");

        comboNumber++;
        if (comboNumber.Equals(3))
            comboNumber = 0;
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
        if (isCrouching && !isGrounded)
        {
            playerRigidBody.velocity = new Vector3(xVelocity, Vector3.up.y * Physics.gravity.y,0);
        }
        else 
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
            {
                audioManager.Play("PlayerJump"); 
                playerRigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                didPlayerJustJumped = true;
            }

            isGoingToJump = false;
        }
    }
    
    private void CheckPlayerStatus()
    {
        if (isGrounded)
        {
            if (Math.Abs(horizontalInput) > 0.01f)
            {
                isWalking = true;
                bool itIsPlaying = audioManager.CheckIfIsPlaying("Player_Step" + stepPreviousNumber.ToString());
                if (!itIsPlaying)
                {
                    audioManager.Play("Player_Step" + stepNumber.ToString());
                    stepPreviousNumber = stepNumber;
                    stepNumber++;
                    if (stepNumber > 4)
                        stepNumber = 1;
                }
            }
            else
            {
                isWalking = false;
                if (audioManager.CheckIfIsPlaying("Player_Step" + stepPreviousNumber.ToString()))
                    audioManager.Stop("Player_Step" + stepPreviousNumber.ToString());
                else if (audioManager.CheckIfIsPlaying("Player_Step" + stepNumber.ToString()))
                    audioManager.Stop("Player_Step" + stepNumber.ToString());
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
                isJumping = true;
            }
            else
            {
                isJumping = false;
            }
        } 
    }
    
    private void SetAnimationVariables()
    {
        //isAttacking
        if (isGoingToAttack)
        {
            ChangeWeaponToSword();
            playerAnimator.SetTrigger("Attack");
            Attack();
            isGoingToAttack = false;
            comboActiveTime = comboTime;
            comboStatus = true;
            isComboEnded = false;
        }
        //isShooting
        if (isGoingToShoot)
        {
            ChangeWeaponToBow();
            playerAnimator.SetTrigger("Shoot");
            bowAnimator.SetTrigger("Shoot");
            audioManager.Play("ArrowShoot");
            isGoingToShoot = false;
            isShooting = true;
        }
        //isJumping
        if (isJumping && didPlayerJustJumped)
        {
            playerAnimator.SetTrigger("Jump");
            didPlayerJustJumped = false;
        }

        //isEnemyNoticed
        playerAnimator.SetBool("isEnemyNoticed", isEnemyNoticed);
        //isFalling
        playerAnimator.SetBool("isFalling", isFalling);
        //isGrounded
        playerAnimator.SetBool("isGrounded", isGrounded);
        //isCrouching
        playerAnimator.SetBool("isCrouching", isCrouching);
        //isWalking
        playerAnimator.SetBool("isWalking", isWalking);
        //isSprinting
        playerAnimator.SetBool("isSprinting", isSprinting);
        //isHavingSword
        playerAnimator.SetBool("isHavingSword", isHavingSword);
        //isComboEnded
        playerAnimator.SetBool("isComboEnded", isComboEnded);
        //isShooting
        playerAnimator.SetBool("isShooting", isShooting);
    }

    private void ChangeWeaponToSword()
    {
        sword.SetActive(true);
        bow.SetActive(false);
    }

    private void ChangeWeaponToBow()
    {
        sword.SetActive(false);
        bow.SetActive(true);
    }

    //KeyBindings
    private bool JumpKey()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    private bool CrouchKey()
    {
        return Input.GetKey(KeyCode.LeftControl);
    }

    private bool AttackSwordKey()
    {
        return Input.GetKeyDown(KeyCode.Mouse0);
    }

    private bool ShootKey()
    {
        return Input.GetKeyDown(KeyCode.Mouse1);
    }

    private bool SprintKey()
    {
        return Input.GetKey(KeyCode.LeftShift);
    }
}
