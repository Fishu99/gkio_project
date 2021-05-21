using UnityEngine;

/// <summary>
/// The state when player is close to the enemy (the enemy notices the player).
/// In this state the enemy follows player and checks if the player is within attack range.
/// </summary>
public class SwordEnemyPlayerNoticedState : SwordEnemyState
{
    public SwordEnemyPlayerNoticedState(SwordEnemyController goblinController) : base(goblinController) { }
    private float timeEntered;
    private float attackInterval;

    public override void Enter()
    {
        timeEntered = Time.time;
        attackInterval = enemyController.attackInterval;
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
        else if (enemyController.IsPlayerInAttackRange())
        {
            enemyController.PlayerInAttackRangeState.FirstAttack = true;
            enemyController.ChangeState(enemyController.PlayerInAttackRangeState);
        }
        else if (!enemyController.IsPlayerNear)
        {
            enemyController.ChangeState(enemyController.PatrolWalkState);
        }
    }
}
