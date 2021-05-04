using UnityEngine;

/**
 * Stan, kiedy przeciwnik chodzi i patroluje teren.
 * Przeciwnik idzie naprz�d a� dojdzie do ko�ca swojego terenu
 */
public class SwordEnemyPatrolWalkState : SwordEnemyState
{
    public SwordEnemyPatrolWalkState(SwordEnemyController goblinController) : base(goblinController)
    {
        
    }

    public override void Enter()
    {
    }

    public override void Exit()
    {
        
    }

    public override void FixedUpdate()
    {
        enemyController.WalkInCurrentDirection();
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
        else if(enemyController.WalkedToEnd())
        {
            enemyController.ChangeState(enemyController.PatrolEndState);
        }
    }

}
