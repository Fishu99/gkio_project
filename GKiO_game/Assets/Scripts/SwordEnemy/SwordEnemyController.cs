using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Kontroler do przeciwnika, który patroluje i atakuje przy u¿yciu miecza lub innej broni bia³ej
 */
public class SwordEnemyController : MonoBehaviour
{
    
    //Prêdkoœæ chodzenia przeciwnika
    public float walkSpeed = 4f;

    //Zakres poruszanie siê przeciwnika (najwiêksza odleg³oœæ w osi x od pocz¹tkowego po³o¿enia)
    public float walkRange = 10f;

    //Odleg³oœæ od gracza przy której przeciwnik zaczyna go œledziæ
    public float playerNearDistance = 5f;
    
    //Czas jaki przeciwnik czeka na koñcu
    public float waitOnEndTime = 2f;
    //Odstêp pomiêdzy atakami
    public float attackInterval = 3f;
    //Czas trwania ataku
    public float attackTime = 1f;


    //Pocz¹tkowa pozycja przeciwnika. Jest pobierana na starcie na podstawie po³o¿enia na scenie
    private Vector3 startPosition;

    private float direction = 1f;
    public bool IsWalking { get; private set; } = false;
    public bool IsPlayerNear { get; private set; }
    private GameObject attackedPlayer = null;

    //Komponenty
    private Rigidbody enemyRigidBody;
    private CapsuleCollider enemyCollider;
    private HealthManager healthManager;
    private WeaponAttack enemyAttack;
    //Adapter animacji
    private SwordEnemyAnimationAdapter animationAdapter;

    //Stany
    private SwordEnemyState currentState;
    public SwordEnemyPatrolWalkState PatrolWalkState { get; private set; }
    public SwordEnemyPatrolEndState PatrolEndState { get; private set; }
    public SwordEnemyPlayerNoticedState PlayerNoticedState { get; private set; }
    public SwordEnemyPlayerAttackState PlayerAttackState { get; private set; }
    public SwordEnemyDeadState DeadState { get; private set; }

    void Start()
    {
        GetTheComponents();
        GetStartPosition();
        CreateStates();
        StartCoroutine("InvokeCheckIfPlayerIsNear");
        ChangeState(PatrolWalkState);
    }

    private void GetTheComponents()
    {
        enemyRigidBody = GetComponent<Rigidbody>();
        healthManager = GetComponent<HealthManager>();
        enemyCollider = GetComponent<CapsuleCollider>();
        enemyAttack = GetComponent<WeaponAttack>();
        animationAdapter = GetComponent<SwordEnemyAnimationAdapter>();
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
        PlayerAttackState = new SwordEnemyPlayerAttackState(this);
        DeadState = new SwordEnemyDeadState(this);
    }

    void Update()
    {
        currentState.Update();
    }

    private void FixedUpdate()
    {
        currentState.FixedUpdate();
    }

    private void SetAnimationVariables()
    {
        
    }
    

    public void ChangeState(SwordEnemyState newState)
    {
        if(currentState != null)
            currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void WalkInCurrentDirection()
    {
        SetVelocityByDirection();
        RotateByDirection();
    }

    public void RotateByDirection()
    {
        if(direction > 0)
        {
            enemyRigidBody.rotation = Quaternion.LookRotation(Vector3.right);
        }
        else if(direction < 0)
        {
            enemyRigidBody.rotation = Quaternion.LookRotation(Vector3.left);
        }
    }

    public void TurnBack()
    {
        direction *= -1;
    }

    
    public bool WalkedToLeftEnd()
    {
        return direction < 0 && enemyRigidBody.position.x < startPosition.x;
    }

    public bool WalkedToRightEnd()
    {
        return direction > 0 && enemyRigidBody.position.x > startPosition.x + walkRange;
    }

    public bool WalkedToEnd()
    {
        return WalkedToLeftEnd() || WalkedToRightEnd();
    }

    public void SetVelocityByDirection()
    {
        float xVelocity = walkSpeed * direction;
        enemyRigidBody.velocity = new Vector3(xVelocity, enemyRigidBody.velocity.y, enemyRigidBody.velocity.z);
        IsWalking = true;
    }

    public void SetVelocityToZero()
    {
        enemyRigidBody.velocity = new Vector3(0, enemyRigidBody.velocity.y, enemyRigidBody.velocity.z);
        IsWalking = false;
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
        Collider[] colliders = Physics.OverlapSphere(transform.position, playerNearDistance, playerMask);
        if(colliders.Length > 0)
        {
            IsPlayerNear = true;
            attackedPlayer = colliders[0].gameObject;
        }
        else
        {
            IsPlayerNear = false;
        }
    }

    public void CalculateVelocityToFollowPlayer()
    {
        
        Collider playerCollider = attackedPlayer.GetComponent<Collider>();
        if(playerCollider.bounds.min.x > enemyCollider.bounds.max.x)
        {
            direction = 1;
            WalkInCurrentDirection();
        }
        else if (enemyCollider.bounds.min.x > playerCollider.bounds.max.x)
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

    public void Attack()
    {
        enemyAttack.Aim = attackedPlayer;
        enemyAttack.Attack();
        animationAdapter.Attack();
    }

    private IEnumerator DeathSequence()
    {
        animationAdapter.Die();
        yield return new WaitForSeconds(4);
        Destroy(gameObject);
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
        Gizmos.DrawWireSphere(transform.position, playerNearDistance);
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
}
