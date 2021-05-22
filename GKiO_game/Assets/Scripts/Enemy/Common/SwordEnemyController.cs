using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A controller for all enemies.
/// It uses a state machine to manage different states of the enemy.
/// </summary>
public class SwordEnemyController : MonoBehaviour
{
    [Tooltip("A string describing the type of enemy")]
    [SerializeField] private string typeOfEnemy;

    [Tooltip("Walking speed of the enemy")]
    public float walkSpeed = 4f;

    [Tooltip("Patrolling range of the enemy. It is displayed in scene view as yellow line")]
    public float walkRange = 10f;

    [Tooltip("The distance of player from the enemy in x-axis when the enemy starts to pursue player")]
    public float playerNearX = 5f;
    [Tooltip("The distance of player from the enemy in y-axis when the enemy starts to pursue player")]
    public float playerNearY = 5f;
    [Tooltip("The distance in x-axis from enemy's transform.position to the point from which playerNearX is calculated")]
    public float playerNearOffsetX = 0;
    [Tooltip("The distance in y-axis from enemy's transform.position to the point from which playerNearY is calculated")]
    public float playerNearOffsetY = 0;


    [Tooltip("Describes how long the enemy waits on the end of its patrol range")]
    public float waitOnEndTime = 2f;

    [Tooltip("The time between the player enetring attack range and the first attack made by the enemy")]
    public float firstAttackDelay = 0.3f;

    [Tooltip("The interval between the attacks if player stays within attack range")]
    public float attackInterval = 0.5f;

    [Tooltip("Length of the attack")]
    public float attackTime = 1f;
    [Tooltip("Length of the death animation. The enemy starts to disappear after that time")]
    public float deathAnimationTime = 3f;
    [Tooltip("Length of the enemy shinking after its death")]
    public float fadeAfterDeathTime = 0.3f;
    [Tooltip("Points awarded to the user for killing the enemy")]
    public int enemyKillValue;

    [Tooltip("Distance between enemy and player for which the enemy stops approaching the player.\n" +
        "It prevents the enemy from pushing the player")]
    public float playerMargin = 0.2f;

    private Vector3 startPosition;
    
    private Vector3 velocity = Vector3.zero;
    private float direction = 1f;
    public bool IsWalking { get; private set; } = false;
    public bool IsPlayerNear { get; private set; }
    private GameObject attackedPlayer = null;

    //Components
    private Rigidbody enemyRigidBody;
    private CapsuleCollider enemyCollider;
    private HealthManager healthManager;
    private WeaponAttack enemyAttack;
    private AudioManager audioManager;
    private Renderer[] renderers;
    private SwordEnemyAnimationAdapter animationAdapter;
    private GameManager gameManager;

    //States for the state machine
    private SwordEnemyState currentState;
    public SwordEnemyPatrolWalkState PatrolWalkState { get; private set; }
    public SwordEnemyPatrolEndState PatrolEndState { get; private set; }
    public SwordEnemyPlayerNoticedState PlayerNoticedState { get; private set; }
    public SwordEnemyPlayerInAttackRangeState PlayerInAttackRangeState { get; private set; }
    public SwordEnemyPlayerAttackState PlayerAttackState { get; private set; }
    public SwordEnemyDeadState DeadState { get; private set; }

    [Tooltip("The sack which falls out of the enemy after its death.")]
    [SerializeField] private GameObject sack;

    void Start()
    {
        GetTheComponents();
        GetStartPosition();
        CreateStates();
        StartCoroutine("InvokeCheckIfPlayerIsNear");
        ChangeState(PatrolWalkState);
    }

    void Update()
    {
        currentState.Update();
        Walk();
    }

    private void FixedUpdate()
    {
        currentState.FixedUpdate();
    }
    
    /// <summary>
    /// Changes the current state of the enemy.
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeState(SwordEnemyState newState)
    {
        if(currentState != null)
            currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    /// <summary>
    /// Makes the enemy walk in its current direction.
    /// </summary>
    public void WalkInCurrentDirection()
    {
        SetVelocityByDirection();
        RotateByDirection();
    }

    /// <summary>
    /// Makes the enemy rotated according to its current direction.
    /// </summary>
    public void RotateByDirection()
    {
        if(direction > 0)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.right);
        }
        else if(direction < 0)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.left);
        }
    }

    /// <summary>
    /// Reverses the direction of the enemy
    /// </summary>
    public void TurnBack()
    {
        direction *= -1;
    }

    /// <summary>
    /// Tells if the enemy walked to left end of its patrolling range.
    /// </summary>
    /// <returns>true if the enemy walked to left end of its patrolling range</returns>
    public bool WalkedToLeftEnd()
    {
        return direction < 0 && transform.position.x < startPosition.x;
    }

    /// <summary>
    /// Tells if the enemy walked to right end of its patrolling range.
    /// </summary>
    /// <returns>true if the enemy walked to right end of its patrolling range</returns>
    public bool WalkedToRightEnd()
    {
        return direction > 0 && transform.position.x > startPosition.x + walkRange;
    }

    /// <summary>
    /// Tells if the enemy walked to either end of its patrolling range.
    /// </summary>
    /// <returns>true if the enemy walked to either end of its patrolling range</returns>
    public bool WalkedToEnd()
    {
        return WalkedToLeftEnd() || WalkedToRightEnd();
    }

    /// <summary>
    /// Sets the velocity so that the enemy will move with its walk speed in its curent direction.
    /// </summary>
    public void SetVelocityByDirection()
    {
        float xVelocity = walkSpeed * direction;
        velocity = new Vector3(xVelocity, 0, 0);
        IsWalking = true;
    }

    /// <summary>
    /// Sets the enemy velocity to 0.
    /// </summary>
    public void SetVelocityToZero()
    {
        velocity = Vector3.zero;
        IsWalking = false;
    }

    /// <summary>
    /// Calculates the enemy velocity, so that it will follow player.
    /// </summary>
    public void CalculateVelocityToFollowPlayer()
    {
        
        Collider playerCollider = attackedPlayer.GetComponent<Collider>();
        if(playerCollider.bounds.min.x > enemyCollider.bounds.max.x + playerMargin)
        {
            direction = 1;
            WalkInCurrentDirection();
        }
        else if (enemyCollider.bounds.min.x > playerCollider.bounds.max.x + playerMargin)
        {
            direction = -1;
            WalkInCurrentDirection();
        }
        else
        {
            SetVelocityToZero();
        }

        if (WalkedToEnd())
        {
            SetVelocityToZero();
        }
    }

    /// <summary>
    /// Tells if the player is within attack range of the enemy.
    /// </summary>
    /// <returns>true if the player is within attack range</returns>
    public bool IsPlayerInAttackRange()
    {
        return enemyAttack.IsAimInAttackRange();
    }

    /// <summary>
    /// Begins the attack.
    /// </summary>
    public void Attack()
    {
        enemyAttack.Aim = attackedPlayer;
        enemyAttack.Attack();
        animationAdapter.Attack();
    }

    private void GetTheComponents()
    {
        enemyRigidBody = GetComponent<Rigidbody>();
        healthManager = GetComponent<HealthManager>();
        enemyCollider = GetComponent<CapsuleCollider>();
        enemyAttack = GetComponent<WeaponAttack>();
        animationAdapter = GetComponent<SwordEnemyAnimationAdapter>();
        renderers = GetComponentsInChildren<Renderer>();
        audioManager = AudioManager.instance;
        gameManager = GameManager.instance;
    }

    private void GetStartPosition()
    {
        startPosition = transform.position;
    }

    private void CreateStates()
    {
        PatrolWalkState = new SwordEnemyPatrolWalkState(this);
        PatrolEndState = new SwordEnemyPatrolEndState(this);
        PlayerNoticedState = new SwordEnemyPlayerNoticedState(this);
        PlayerInAttackRangeState = new SwordEnemyPlayerInAttackRangeState(this);
        PlayerAttackState = new SwordEnemyPlayerAttackState(this);
        DeadState = new SwordEnemyDeadState(this);
    }

    private void Walk()
    {
        transform.Translate(velocity * Time.deltaTime, Space.World);
        transform.position = new Vector3(transform.position.x, startPosition.y, startPosition.z);
    }

    private IEnumerator InvokeCheckIfPlayerIsNear()
    {
        while (healthManager.IsAlive())
        {
            CheckIfPlayerIsNear();
            yield return new WaitForSeconds(.1f);
        }
    }

    private void CheckIfPlayerIsNear()
    {
        int playerMask = 1 << 6;
        Vector3 halfExtents = new Vector3(playerNearX, playerNearY, 2);
        Vector3 center = transform.position + Vector3.up * playerNearOffsetY + transform.forward * playerNearOffsetX;
        Collider[] colliders = Physics.OverlapBox(center, halfExtents, Quaternion.identity, playerMask);
        if (colliders.Length > 0)
        {
            IsPlayerNear = true;
            attackedPlayer = colliders[0].gameObject;
        }
        else
        {
            IsPlayerNear = false;
        }
    }

    private IEnumerator DeathSequence()
    {
        PlayDeathSound();
        ReportKilling();
        DisablePhysics();
        animationAdapter.Die();
        yield return new WaitForSeconds(deathAnimationTime);
        Instantiate(sack, transform.position + new Vector3(0.5f,0.25f,0), Quaternion.identity);
        yield return StartCoroutine(FadeAfterDeath());
        Destroy(gameObject);
    }

    private IEnumerator FadeAfterDeath()
    {
        float fadeStartTime = Time.time;
        float fadeDuration = 0;
        Vector3 initialScale = transform.localScale;
        while((fadeDuration = Time.time - fadeStartTime) < fadeAfterDeathTime)
        {
            float alpha = 1 - fadeDuration / fadeAfterDeathTime;
            transform.localScale = initialScale * alpha;
            yield return null;
        }
        yield break;
    }

    private void SetAlphaForAllMaterials(float alpha)
    {
        foreach(var renderer in renderers)
        {
            Material material = renderer.material;
            material.color = new Color(material.color.r, material.color.g, material.color.b, alpha);
            Debug.Log(material.color);
        }
    }

    private void DisablePhysics()
    {
        enemyRigidBody.isKinematic = true;
        enemyRigidBody.detectCollisions = false;
    }

    private void ReportKilling()
    {
        gameManager?.AddKilledEnemy(enemyKillValue);
    }

    void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
        {
            GetTheComponents();
            GetStartPosition();
        }
        DrawPlayerNearDistanceSphere();
        DrawWalkRange();
    }

    private void DrawPlayerNearDistanceSphere()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Vector3 size = 2 * new Vector3(playerNearX, playerNearY, 2);
        Vector3 center = transform.position + Vector3.up * playerNearOffsetY + transform.forward * playerNearOffsetX;
        Gizmos.DrawWireCube(center, size);
    }

    private void DrawWalkRange()
    {
        if (startPosition != null && enemyCollider != null)
        {
            Gizmos.color = new Color(1, 1, 0, 1);
            Vector3 endPosition = startPosition + new Vector3(walkRange, 0, 0);
            Vector3 offsetY = Vector3.up * enemyCollider.bounds.extents.y;
            Gizmos.DrawLine(startPosition + offsetY, endPosition + offsetY);
        }
    }

    private void PlayDeathSound()
    {
        if (typeOfEnemy.Equals("Goblin"))
            audioManager.Play("GoblinDeath");
        else if (typeOfEnemy.Equals("Skeleton"))
            audioManager.Play("SkeletonDeath");
    }

}
