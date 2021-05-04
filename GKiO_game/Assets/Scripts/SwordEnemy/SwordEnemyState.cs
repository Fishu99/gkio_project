using UnityEngine;

public abstract class SwordEnemyState
{
    protected SwordEnemyController enemyController;
    protected HealthManager healthManager;
    public SwordEnemyState(SwordEnemyController enemyController)
    {
        this.enemyController = enemyController;
        healthManager = enemyController.GetComponent<HealthManager>();

    }
    public abstract void Enter();

    public abstract void Update();

    public abstract void FixedUpdate();

    public abstract void Exit();
}
