using System.Collections;
using UnityEngine;

public class GoblinPlayerNoticedState : GoblinState
{
    IEnumerator attackCoroutine;
    public GoblinPlayerNoticedState(GoblinController goblinController) : base(goblinController) { }

    public override void Enter()
    {
        goblinAnimation.Play("walk");
        attackCoroutine = goblinController.AttackPeriodically();
        goblinController.StartCoroutine(attackCoroutine);
    }

    public override void Exit()
    {
        goblinController.StopCoroutine(attackCoroutine);
    }

    public override void FixedUpdate()
    {
        goblinController.CalculateVelocityToFollowPlayer();
    }

    public override void Update()
    {
        if (!healthManager.IsAlive())
        {
            goblinController.ChangeState(goblinController.DeadState);
        }
        else if (!goblinController.IsPlayerNear)
        {
            goblinController.ChangeState(goblinController.PatrolWalkState);
        }

    }



}
