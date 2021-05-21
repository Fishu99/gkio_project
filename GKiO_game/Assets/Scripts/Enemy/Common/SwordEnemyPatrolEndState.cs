using UnityEngine;

/// <summary>
/// The state in which the enemy is waiting at the end of its patrolling range.
/// The enemy stays in this state for enemyController.waitOnEndTime.
/// </summary>
public class SwordEnemyPatrolEndState : SwordEnemyState
{
    private float timeWhenStartedWaiting;
    private float timeToWait;
    public SwordEnemyPatrolEndState(SwordEnemyController goblinController) : base(goblinController) {}

    public override void Enter()
    {
        enemyController.SetVelocityToZero();
        timeWhenStartedWaiting = Time.time;
        timeToWait = enemyController.waitOnEndTime;
    }

    public override void Exit()
    {

    }

    public override void FixedUpdate()
    {

    }

    public override void Update()
    {
        if (!healthManager.IsAlive())
        {
            enemyController.ChangeState(enemyController.DeadState);
        }
        else if (enemyController.IsPlayerNear)
        {
            enemyController.ChangeState(enemyController.PlayerNoticedState);
        }
        else if (IsTimeToTurnBack())
        {
            enemyController.TurnBack();
            enemyController.ChangeState(enemyController.PatrolWalkState);
        }
    }

    private bool IsTimeToTurnBack()
    {
        return Time.time - timeWhenStartedWaiting > timeToWait;
    }
}
