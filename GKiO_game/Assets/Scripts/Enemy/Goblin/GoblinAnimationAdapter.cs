using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Animation adapter for goblin.
/// </summary>
public class GoblinAnimationAdapter : SwordEnemyAnimationAdapter
{
    private Animator goblinAnimator;
    private SwordEnemyController enemyController;
    //Walk speed for which the animation looks natural, ie. it doesn't slip.
    private readonly float walkAnimationSpeed = 0.8f;
    private readonly float attackAnimationLength = 1f;
    void Start()
    {
        goblinAnimator = GetComponent<Animator>();
        enemyController = GetComponent<SwordEnemyController>();
    }

    void Update()
    {
        //The multiplier is computed to adjust animation speed to actual walking speed.
        float walkSpeedMultiplier = enemyController.walkSpeed / walkAnimationSpeed;
        goblinAnimator.SetFloat("walkSpeedMultiplier", walkSpeedMultiplier);
        goblinAnimator.SetBool("isWalking", enemyController.IsWalking);
    }

    public override void Attack()
    {
        float attackSpeedMultiplier = attackAnimationLength/enemyController.attackTime;
        goblinAnimator.SetFloat("attackSpeedMultiplier", attackSpeedMultiplier);
        goblinAnimator.SetTrigger("attack");
    }

    public override void Die()
    {
        goblinAnimator.SetTrigger("die");
    }
}
