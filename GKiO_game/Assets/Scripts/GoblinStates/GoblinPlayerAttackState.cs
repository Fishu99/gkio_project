using UnityEngine;

public class GoblinPlayerAttackState : GoblinState
{
    private float timeWhenStartedAttacking;
    private float attackTime;

    public GoblinPlayerAttackState(GoblinController goblinController) : base(goblinController)
    {

    }

    public override void Enter()
    {
        attackTime = goblinAnimation["attack3"].length;
        timeWhenStartedAttacking = Time.time;
        goblinAnimation.CrossFade("attack3", 0.1f);
        goblinController.Attack();
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
            goblinController.ChangeState(goblinController.DeadState);
        }
        else if (Time.time - timeWhenStartedAttacking > attackTime)
        {
            goblinController.ChangeState(goblinController.PlayerNoticedState);
        }

    }

}
