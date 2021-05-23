using UnityEngine;

/// <summary>
/// Animation adapter for skeleton with a dagger.
/// </summary>
public class SkeletonDaggerAnimationAdapter : SwordEnemyAnimationAdapter
{
    private Animator skeletonAnimator;
    private SwordEnemyController enemyController;
    private float speedh = 0;
    private float maxSpeedh;
    private float endAcceleration = 2f;
    private readonly float throwAnimationLength = 1.333f;
    private readonly float daggerReleaseTime = 0.57f;
    /// <summary>
    /// Resturns the time after Attack call when the dagger should be released.
    /// </summary>
    public float ActualReleaseTime
    {
        get => enemyController.attackTime / throwAnimationLength * daggerReleaseTime;
    }

    /// <summary>
    /// Resturns the time after Attack call when the dagger should be replaced.
    /// </summary>
    public float ActualReplaceTime
    {
        get => enemyController.attackTime / throwAnimationLength * throwAnimationLength;
    }


    void Start()
    {
        skeletonAnimator = GetComponent<Animator>();
        enemyController = GetComponent<SwordEnemyController>();
    }

    void Update()
    {
        SetSpeedh();
    }

    private void SetSpeedh()
    {
        maxSpeedh = enemyController.walkSpeed;
        if (enemyController.IsWalking)
            speedh += endAcceleration * Time.deltaTime;
        else
            speedh -= endAcceleration * Time.deltaTime;
        if (speedh > maxSpeedh)
            speedh = maxSpeedh;
        if (speedh < 0)
            speedh = 0;
        skeletonAnimator.SetFloat("speedh", speedh);
    }

    public override void Attack()
    {
        float attackSpeedMultiplier = throwAnimationLength / enemyController.attackTime;
        skeletonAnimator.SetFloat("throwSpeedMultiplier", attackSpeedMultiplier);
        skeletonAnimator.SetTrigger("Throw");
    }

    public override void Die()
    {
        skeletonAnimator.SetTrigger("Fall1");
    }
}
