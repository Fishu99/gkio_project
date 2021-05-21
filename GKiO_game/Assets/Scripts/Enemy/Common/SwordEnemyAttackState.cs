using UnityEngine;

/// <summary>
/// The state when the enemy is attacking.
/// In this state the enemy doesn't walk, the attack animation is played and attack is performed.
/// Enemy exits this state after enemyController.attackTime.
/// </summary>
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
        enemyController.CalculateVelocityToFollowPlayer();
    }

    public override void Update()
    {
        if (!healthManager.IsAlive())
        {
            enemyController.ChangeState(enemyController.DeadState);
        }
        else if (Time.time - timeWhenStartedAttacking > attackTime)
        {
            enemyController.ChangeState(enemyController.PlayerInAttackRangeState);
        }

    }

}
