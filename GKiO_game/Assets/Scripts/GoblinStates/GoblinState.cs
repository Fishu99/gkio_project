using UnityEngine;

public abstract class GoblinState
{
    protected GoblinController goblinController;
    protected Animator goblinAnimator;
    protected HealthManager healthManager;
    public GoblinState(GoblinController goblinController)
    {
        this.goblinController = goblinController;
        goblinAnimator = goblinController.GetComponent<Animator>();
        healthManager = goblinController.GetComponent<HealthManager>();

    }
    public abstract void Enter();

    public abstract void Update();

    public abstract void FixedUpdate();

    public abstract void Exit();
}
