using UnityEngine;

//Stan, kiedy goblin chodzi i patroluje teren.
//Goblin idzie naprzód a¿ dojdzie do koñca swojego terenu
public class GoblinPatrolWalkState : GoblinState
{
    public GoblinPatrolWalkState(GoblinController goblinController) : base(goblinController)
    {
        
    }

    public override void Enter()
    {
        goblinAnimator.SetBool("isWalking", true);
    }

    public override void Exit()
    {
        
    }

    public override void FixedUpdate()
    {
        goblinController.WalkInCurrentDirection();
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
        else if(goblinController.WalkedToEnd())
        {
            goblinController.ChangeState(goblinController.PatrolEndState);
        }
    }

}
