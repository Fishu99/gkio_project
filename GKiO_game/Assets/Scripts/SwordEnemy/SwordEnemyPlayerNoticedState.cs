using System.Collections;
using UnityEngine;

/**
 * Stan, w którym przeciwnik pod¹¿a za zauwa¿onym graczem
 * Przeciwnik pozostaje w tym stanie czas timeWhenAttack, po czym przechodzi do stanu ataku
 * Oznacza to, ¿e przeciwnik atakuje na oœlep co okreœlony czas bez sprawdzenia, czy gracz jest w zasiêgu ataku.
 */
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
        else if (!enemyController.IsPlayerNear)
        {
            enemyController.ChangeState(enemyController.PatrolWalkState);
        }
        else if(Time.time - timeEntered > attackInterval)
        {
            enemyController.ChangeState(enemyController.PlayerAttackState);
        }

    }



}
