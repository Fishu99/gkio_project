using System.Collections;
using UnityEngine;

//Stan, w którym goblin pod¹¿a za zauwa¿onym graczem
//Goblin pozostaje w tym stanie czas timeWhenAttack, po czym przechodzi do stanu ataku
//Oznacza to, ¿e goblin atakuje na oœlep co okreœlony czas bez sprawdzenia, czy gracz jest w zasiêgu ataku.
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
