using UnityEngine;

/// <summary>
/// Animation adapter for skeleton with a sword.
/// </summary>
public class SkeletonAnimationAdapter : SwordEnemyAnimationAdapter
{
    private Animator skeletonAnimator;
    private SwordEnemyController enemyController;
    private float speedh = 0;
    private float maxSpeedh;
    private float endAcceleration = 2f;
    private readonly float attackAnimationLength = 1.333f;

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
        float attackSpeedMultiplier = attackAnimationLength / enemyController.attackTime;
        skeletonAnimator.SetFloat("attackSpeedMultiplier", attackSpeedMultiplier);
        skeletonAnimator.SetTrigger("Attack1h1");
    }

    public override void Die()
    {
        skeletonAnimator.SetTrigger("Fall1");
    }
}
