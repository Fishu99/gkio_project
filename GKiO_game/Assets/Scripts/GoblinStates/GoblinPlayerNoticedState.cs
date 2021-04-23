using System.Collections;
using UnityEngine;

public class GoblinPlayerNoticedState : GoblinState
{
    public GoblinPlayerNoticedState(GoblinController goblinController) : base(goblinController) { }
    private float timeEntered;
    private float timeWhenAttack = 3;

    public override void Enter()
    {
        goblinAnimation.CrossFade("walk");
        timeEntered = Time.time;
    }

    public override void Exit()
    {
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
        else if(Time.time - timeEntered > timeWhenAttack)
        {
            goblinController.ChangeState(goblinController.PlayerAttackState);
        }
        else
        {
            if(goblinController.IsWalking)
                goblinAnimation.CrossFade("walk");
            else
                goblinAnimation.CrossFade("idle");
        }

    }



}
