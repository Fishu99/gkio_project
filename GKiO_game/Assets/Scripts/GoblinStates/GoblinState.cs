using UnityEngine;

public abstract class GoblinState
{
    protected GoblinController goblinController;
    protected Animation goblinAnimation;
    protected HealthManager healthManager;
    public GoblinState(GoblinController goblinController)
    {
        this.goblinController = goblinController;
        goblinAnimation = goblinController.GetComponent<Animation>();
        healthManager = goblinController.GetComponent<HealthManager>();

    }
    public abstract void Enter();

    public abstract void Update();

    public abstract void FixedUpdate();

    public abstract void Exit();
}
