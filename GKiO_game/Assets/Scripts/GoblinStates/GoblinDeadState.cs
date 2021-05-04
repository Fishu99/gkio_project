using UnityEngine;

//Stan, kiedy goblin jest martwy
public class GoblinDeadState : GoblinState
{
    public GoblinDeadState(GoblinController goblinController) : base(goblinController)
    {

    }

    public override void Enter()
    {
        goblinAnimator.SetTrigger("die");
        goblinController.StartCoroutine("DeathSequence");
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
