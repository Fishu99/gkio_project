using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Adapter animacji dla goblina.
 */
public class GoblinAnimationAdapter : SwordEnemyAnimationAdapter
{
    private Animator goblinAnimator;
    private SwordEnemyController enemyController;
    //Szybkoœæ poruszania siê goblina, dla której animacja wygl¹da naturalnie
    private readonly float walkAnimationSpeed = 0.8f;
    private readonly float attackAnimationLength = 1f;
    void Start()
    {
        goblinAnimator = GetComponent<Animator>();
        enemyController = GetComponent<SwordEnemyController>();
    }

    void Update()
    {
        //Obliczamy ile razy trzeba przyspieszyæ animacjê chodzenia, aby wygl¹da³a naturalnie
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
