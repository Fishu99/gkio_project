using UnityEngine;

/** 
 * Stan, kiedy przeciwnik patroluje teren i jest na ko�cu patrolowanego obszaru. 
 * Przeciwnik czeka czeka wtedy czas okre�lony w timeToWait, po czym zawraca.
 */
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
