using UnityEngine;

/**
 * Stan, w którym przeciwnik atakuje gracza.
 * Przeciwnik w tym czasie stoi w miejscu i wykonuje animacjê ataku.
 * Po zakoñczeniu ataku przeciwnik powraca do SwordEnemyPlayerNoticedState
 */
public class SwordEnemyPlayerAttackState : SwordEnemyState
{
    private float timeWhenStartedAttacking;
    private float attackTime;

    public SwordEnemyPlayerAttackState(SwordEnemyController goblinController) : base(goblinController)
    {

    }

    public override void Enter()
    {
        timeWhenStartedAttacking = Time.time;
        attackTime = enemyController.attackTime;
        enemyController.Attack();
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
        else if (Time.time - timeWhenStartedAttacking > attackTime)
        {
            enemyController.ChangeState(enemyController.PlayerNoticedState);
        }

    }

}
