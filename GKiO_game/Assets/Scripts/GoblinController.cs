using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Goblin chodzi od punktu, w kt�rym si� go wstawi,
 * do punktu znajduj� cego si� w odleg�o�ci walkRange na prawo od punktu wstawienia goblina
 */
public class GoblinController : MonoBehaviour
{
    private Rigidbody goblinRigidBody;
    //private Animation goblinAnimation;
    private Animator goblinAnimator;
    private CapsuleCollider goblinCollider;
    private HealthManager healthManager;
    private SwordAttack swordAttack;
    //Pr�dko�� goblina
    [SerializeField]
    private float goblinSpeed = 4f;
    [SerializeField]
    private float walkRange = 10f;
    [SerializeField]
    private float playerNearDistance = 5f;
    [SerializeField]
    private float attackPeriod = 4f;
    //Pocz�tkowa pozycja goblina pobierana na starcie
    private Vector3 startPosition;
    private float direction = 1f;
    public bool IsWalking { get; private set; } = false;
    public bool IsPlayerNear { get; private set; }
    private GameObject attackedPlayer = null;

    private GoblinState currentState;
    public GoblinPatrolWalkState PatrolWalkState { get; private set; }
    public GoblinPatrolEndState PatrolEndState { get; private set; }
    public GoblinPlayerNoticedState PlayerNoticedState { get; private set; }
    public GoblinPlayerAttackState PlayerAttackState { get; private set; }
    public GoblinDeadState DeadState { get; private set; }

    void Start()
    {
        goblinRigidBody = GetComponent<Rigidbody>();
        healthManager = GetComponent<HealthManager>();
        goblinCollider = GetComponent<CapsuleCollider>();
        goblinAnimator = GetComponent<Animator>();
        swordAttack = GetComponent<SwordAttack>();
        CreateStates();
        goblinAnimator.SetBool("isWalking", false);
        startPosition = transform.position;
        StartCoroutine("InvokeCheckIfPlayerIsNear");
        ChangeState(PatrolWalkState);
    }


    private void CreateStates()
    {
        PatrolWalkState = new GoblinPatrolWalkState(this);
        PatrolEndState = new GoblinPatrolEndState(this);
        PlayerNoticedState = new GoblinPlayerNoticedState(this);
        PlayerAttackState = new GoblinPlayerAttackState(this);
        DeadState = new GoblinDeadState(this);
    }

    void Update()
    {
        currentState.Update();
    }

    private void FixedUpdate()
    {
        currentState.FixedUpdate();
    }

    

    public void ChangeState(GoblinState newState)
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
            goblinRigidBody.rotation = Quaternion.LookRotation(Vector3.right);
        }
        else if(direction < 0)
        {
            goblinRigidBody.rotation = Quaternion.LookRotation(Vector3.left);
        }
    }

    public void TurnBack()
    {
        direction *= -1;
    }

    
    public bool WalkedToLeftEnd()
    {
        return direction < 0 && goblinRigidBody.position.x < startPosition.x;
    }

    public bool WalkedToRightEnd()
    {
        return direction > 0 && goblinRigidBody.position.x > startPosition.x + walkRange;
    }

    public bool WalkedToEnd()
    {
        return WalkedToLeftEnd() || WalkedToRightEnd();
    }

    public void SetVelocityByDirection()
    {
        float xVelocity = goblinSpeed * direction;
        goblinRigidBody.velocity = new Vector3(xVelocity, goblinRigidBody.velocity.y, goblinRigidBody.velocity.z);
        IsWalking = true;
    }

    public void SetVelocityToZero()
    {
        goblinRigidBody.velocity = new Vector3(0, goblinRigidBody.velocity.y, goblinRigidBody.velocity.z);
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
        if(playerCollider.bounds.min.x > goblinCollider.bounds.max.x)
        {
            direction = 1;
            WalkInCurrentDirection();
        }
        else if (goblinCollider.bounds.min.x > playerCollider.bounds.max.x)
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
        swordAttack.Attack();
    }

    public IEnumerator AttackPeriodically()
    {
        while (true)
        {
            swordAttack.Attack();
            yield return new WaitForSeconds(attackPeriod);
        }
    }

    private IEnumerator DeathSequence()
    {
        yield return new WaitForSeconds(4);
        Destroy(gameObject);
    }
}
