using UnityEngine;

public class GoblinDeadState : GoblinState
{
    public GoblinDeadState(GoblinController goblinController) : base(goblinController)
    {

    }

    public override void Enter()
    {
        goblinAnimation.CrossFade("death");
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
