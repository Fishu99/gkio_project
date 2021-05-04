using UnityEngine;

/**
 * Stan, w którym przeciwnik jest martwy
 */
public class SwordEnemyDeadState : SwordEnemyState
{
    public SwordEnemyDeadState(SwordEnemyController goblinController) : base(goblinController)
    {

    }

    public override void Enter()
    {
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
