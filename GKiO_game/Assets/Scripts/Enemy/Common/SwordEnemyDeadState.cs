using UnityEngine;

/// <summary>
/// State whe the enemy is dead.
/// </summary>
public class SwordEnemyDeadState : SwordEnemyState
{
    public SwordEnemyDeadState(SwordEnemyController goblinController) : base(goblinController)
    {

    }

    public override void Enter()
    {
        enemyController.SetVelocityToZero();
        enemyController.StartCoroutine("DeathSequence");
    }

    public override void Exit()
    {

    }

    public override void FixedUpdate()
    {

    }

    public override void Update()
    {


    }

}
