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
    AudioManager audioManager;
    CapsuleCollider playerCollider;
    ComboManager comboManager;
    PlayerStatusController statusController; 
    Rigidbody playerRigidBody;
    HealthManager healthManager;
    public Camera playerCamera;

    //Variables and constants
    [SerializeField] private float playerSpeed = 5f;
    [SerializeField] private float playerSprintMultiplier = 2.0f;
    [SerializeField] private float playerAttackCooldown = 0.5f;
    [SerializeField] private float jumpForce = 1000f;
    [SerializeField] private GameObject arrowPrefab;
        
    
    public float arrowDamage = 30f;    
    private float arrowOriginRadius = 1.3f;
    private Vector3 startPosition;
    private GameObject lastCheckpoint;
    public float horizontalInput = 0f;

    //Player states & statuses
    public int arrows = 987;
    public int lives = 3;
    private bool isGrounded = true;
    private bool isGoingToJump = false;
    private bool isCrouching = false;
    private bool didPlayerJustJumped = false;

    public bool IsInDeadZone { get; private set; } = false;

    //Combo & attacks
    private float attackCooldown;
    public bool comboKeyPressed = false;
    private bool isAttackReady = true;
    private bool isAttacking = false;

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
    private bool isDead = false;

    //--- triggers
    private bool isJumping = false;
    private bool isGoingToShoot = false;

    //GameObjects
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
        healthManager = GetComponent<HealthManager>();
        statusController = GetComponent<PlayerStatusController>();
        comboManager = GetComponent<ComboManager>();
        audioManager = AudioManager.instance;
        startPosition = transform.position;
        sword = GameObject.Find("Sword_1");
        bow = GameObject.Find("Wooden Bow");

        //ChangeWeaponToBow();
        ChangeWeaponToSword();

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
        CheckHealth();
        if (isDead)
            return;

        if (isShooting)
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
        if (attackCooldown >= 0.0f)
        {
            attackCooldown -= Time.deltaTime;
            if (attackCooldown < 0.0f)
                isAttackReady = true;
        }
        horizontalInput = Input.GetAxis("Horizontal");
        CheckIfGrounded();
        CheckPlayerStatus();
        CheckInput();
    }

    private void FixedUpdate()
    {
        if (isDead)
        {
            StopPlayer();
        }
        else
        {
            TurnPlayer();
            CheckAndJump();
            CheckIfIsAttacking();
            CalculateVelocity();
        }
        SetAnimationVariables();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            DieOfDeadZone();
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Checkpoint"))
        {
            if (other.gameObject.CompareTag("Checkpoint"))
                lastCheckpoint = other.gameObject;
            else if (other.gameObject.CompareTag("FinishZone"))
            {
                StopPlayer();
                statusController.FinishLevel();
            }
        }
    }

    private void CheckIfIsAttacking()
    {
        AnimatorStateInfo animState = playerAnimator.GetCurrentAnimatorStateInfo(0);
        isAttacking = animState.IsTag("Attack");
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
        if (AttackSwordKey() && isGrounded && isAttackReady)
        {
            isAttackReady = false;
            attackCooldown = playerAttackCooldown;
            comboKeyPressed = true;
        }

        //Bow&Arrow
        if (ShootKey() && !isAttacking)
        {
            StartArrowShotIfHasArrows();
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

    private void StartArrowShotIfHasArrows()
    {
        if (arrows > 0)
        {
            isGoingToShoot = true;
            ARROW.SetActive(true);
            arrows--;
        }
    }

    private void StopPlayer()
    {
        playerRigidBody.velocity = new Vector3(0, playerRigidBody.velocity.y, playerRigidBody.velocity.z);
    }

    private void Shoot()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 center = playerCollider.bounds.center;
        Vector3 screenPlayerPosition = playerCamera.WorldToScreenPoint(center);
        Quaternion arrowRotation = Quaternion.FromToRotation(Vector3.up, mousePosition - screenPlayerPosition);
        Vector3 radius = Vector3.up * arrowOriginRadius;
        Vector3 arrowPosition = center + arrowRotation * radius;
        ARROW.SetActive(false);
        GameObject arrow = Instantiate(arrowPrefab, arrowPosition, arrowRotation);
        ArrowController arrowController = arrow.GetComponent<ArrowController>();
        arrowController.arrowDamage = arrowDamage;
        arrowController.Shoot();
        isShooting = false;
    }

    private void Attack()
    {
        GetComponent<SwordAttack>().Attack();
    }

    private void CheckIfGrounded()
    {
        float colliderRadius = playerCollider.radius;
        Vector3 colliderSphereCenter = playerCollider.bounds.min + new Vector3(colliderRadius, colliderRadius, colliderRadius);
        int playerMask = ~(1 << 6 | 1 << 10 | 1 << 11);
        //Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, 0.2f)
        if (Physics.CheckSphere(colliderSphereCenter, colliderRadius + 0.2f, playerMask))
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
        if (!isAttacking)
        {
            float xVelocity = playerSpeed * horizontalInput;
            if (isSprinting)
            {
                xVelocity *= playerSprintMultiplier;
            }

            if (isCrouching && !isGrounded)
            {
                playerRigidBody.velocity = new Vector3(xVelocity, Vector3.up.y * Physics.gravity.y, 0);
            }
            else
                playerRigidBody.velocity = new Vector3(xVelocity, playerRigidBody.velocity.y, playerRigidBody.velocity.z);
        }

    }

    private void TurnPlayer()
    {
        if (horizontalInput > 0)
        {
            playerRigidBody.rotation = Quaternion.LookRotation(Vector3.right);
        }
        else if (horizontalInput < 0)
        {
            playerRigidBody.rotation = Quaternion.LookRotation(Vector3.left);
        }
    }

    private void CheckAndJump()
    {
        if (isGoingToJump)
        {
            if (isGrounded)
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
            if (!isAttacking)
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

    private void CheckHealth()
    {
        if (!isDead && healthManager.Health == 0)
        {
            audioManager.Play("PlayerDeath");
            DieOfNoHealth();
        }
    }

    private void SetAnimationVariables()
    {
        //isAttacking
        if (comboKeyPressed)
        {
            ChangeWeaponToSword();
            float invisibilityOfAttack = comboManager.AttackIfPossible();
            if (comboManager.wasAttackExecuted)
            {

                Invoke("Attack", invisibilityOfAttack);
                comboKeyPressed = false;
                comboManager.wasAttackExecuted = false;
            }
        }
        if (isGoingToShoot)
        {
            RotatePlayerForShooting();
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

    private void RotatePlayerForShooting()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 center = playerCollider.bounds.center;
        Vector3 screenPlayerPosition = playerCamera.WorldToScreenPoint(center);
        if (mousePosition.x - screenPlayerPosition.x > 0)
        {
            playerRigidBody.rotation = Quaternion.LookRotation(Vector3.right);
        }
        else
        {
            playerRigidBody.rotation = Quaternion.LookRotation(Vector3.left);
        }
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

    private void DieOfNoHealth()
    {
        playerAnimator.SetTrigger("Die");
        isDead = true;
        Invoke("RespawnOrGameOver", 3);
    }

    private void DieOfDeadZone()
    {
        playerAnimator.SetTrigger("Die");
        audioManager.Play("PlayerDeath");
        IsInDeadZone = true;
        isDead = true;
        healthManager.SetZero();
        Invoke("RespawnOrGameOver", 1f);
    }

    private void RespawnOrGameOver()
    {
        lives--;
        if (lives > 0)
        {
            Respawn();
        }
        else
        {
            Debug.Log("Game over!");
        }

    }

    private void Respawn()
    {
        isDead = false;
        IsInDeadZone = false;
        playerAnimator.SetTrigger("Respawn");
        ReturnToCheckpoint();
        healthManager.SetMax();
    }

    private void ReturnToCheckpoint()
    {
        if (lastCheckpoint == null)
        {
            transform.position = startPosition;
        }
        else
        {
            transform.position = lastCheckpoint.transform.position;
        }
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
