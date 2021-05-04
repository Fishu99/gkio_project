using UnityEngine;

//Stan, kiedy goblin patroluje teren i jest na koñcu patrolowanego obszaru. Goblin czeka wtedy 2s, po czym zawraca.
public class GoblinPatrolEndState : GoblinState
{
    private float timeWhenStartedWaiting;
    private float timeToWait = 2f;
    public GoblinPatrolEndState(GoblinController goblinController) : base(goblinController) {}

    public override void Enter()
    {
        goblinAnimator.SetBool("isWalking", false);
        goblinController.SetVelocityToZero();
        timeWhenStartedWaiting = Time.time;
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
        else if (goblinController.IsPlayerNear)
        {
            goblinController.ChangeState(goblinController.PlayerNoticedState);
        }
        else if (IsTimeToTurnBack())
        {
            goblinController.TurnBack();
            goblinController.ChangeState(goblinController.PatrolWalkState);
        }

    }

    private bool IsTimeToTurnBack()
    {
        return Time.time - timeWhenStartedWaiting > timeToWait;
    }
}
