using UnityEngine;

/// <summary>
/// The state when the player is within attack range of the enemy.
/// In this state the enemy follows the player and attacks him periodically.
/// </summary>
public class SwordEnemyPlayerInAttackRangeState : SwordEnemyState
{
    private float timeEntered;
    private float attackInterval;
    private float firstAttackDelay;
    public bool FirstAttack { get; set; } = false;

    public SwordEnemyPlayerInAttackRangeState(SwordEnemyController goblinController) : base(goblinController)
    {

    }

    public override void Enter()
    {
        timeEntered = Time.time;
        attackInterval = enemyController.attackInterval;
        firstAttackDelay = enemyController.firstAttackDelay;
    }

    public override void Exit()
    {

    }

    public override void FixedUpdate()
    {
        enemyController.CalculateVelocityToFollowPlayer();
    }

    public override void Update()
    {
        if (!healthManager.IsAlive())
        {
            enemyController.ChangeState(enemyController.DeadState);
        }
        else if (!enemyController.IsPlayerInAttackRange())
        {
            enemyController.ChangeState(enemyController.PlayerNoticedState);
        }
        else if (!enemyController.IsPlayerNear)
        {
            enemyController.ChangeState(enemyController.PatrolWalkState);
        }
        else if (IsTimeToAttack())
        {
            enemyController.ChangeState(enemyController.PlayerAttackState);
        }
    }


    private bool IsTimeToAttack()
    {
        float timeDiff = Time.time - timeEntered;
        if(FirstAttack && timeDiff > firstAttackDelay)
        {
            FirstAttack = false;
            return true;
        }
        else if(timeDiff > attackInterval)
        {
            return true;
        }
        return false;
    }
}
