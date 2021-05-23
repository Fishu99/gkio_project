using UnityEngine;

/// <summary>
/// Base class for enemy states.
/// </summary>
public abstract class SwordEnemyState
{
    /// <summary>
    /// The SwordEnemyController component of the enemy.
    /// </summary>
    protected SwordEnemyController enemyController;
    /// <summary>
    /// The SwordEnemyController component of the enemy.
    /// </summary>
    protected HealthManager healthManager;
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="enemyController">The SwordEnemyController component of the enemy.</param>
    public SwordEnemyState(SwordEnemyController enemyController)
    {
        this.enemyController = enemyController;
        healthManager = enemyController.GetComponent<HealthManager>();

    }
    /// <summary>
    /// The Enter method is called when state machine transitions to the state.
    /// </summary>
    public abstract void Enter();

    /// <summary>
    /// Update is called in every frame of the state.
    /// </summary>
    public abstract void Update();

    /// <summary>
    /// Update for physics calculations.
    /// </summary>
    public abstract void FixedUpdate();

    /// <summary>
    /// The Enter method is called when state machine leaves the state.
    /// </summary>
    public abstract void Exit();
}
