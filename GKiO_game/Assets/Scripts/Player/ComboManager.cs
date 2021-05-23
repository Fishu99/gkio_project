using UnityEngine;

/// <summary>
/// Enum of possible attacks.
/// </summary>
enum Attack
{
    None,
    Attacking,
    AfterFirst,
    AfterSecond,
    AfterThird
}

/// <summary>
/// A class for controlling the atack combos.
/// </summary>
public class ComboManager : MonoBehaviour
{
    Animator playerAnimator;
    AudioManager audioManager;
    PlayerController playerController;

    Attack playerComboStatus;
    public bool wasAttackExecuted;
    [SerializeField] private float timeOfInvisibility1 = 0.0f;
    [SerializeField] private float timeOfInvisibility2 = 0.0f;
    [SerializeField] private float timeOfInvisibility3 = 0.0f;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerAnimator = GetComponent<Animator>();
        audioManager = AudioManager.instance;
    }
    /// <summary>
    /// Performs an attack if it's possible to attack.
    /// </summary>
    /// <returns>The time of invisibility.</returns>
    public float AttackIfPossible()
    {
        bool canPlayerAttack = CanPlayerAttack();
        if (canPlayerAttack)
        {
            switch (playerComboStatus)
            {
                case Attack.None:
                case Attack.AfterThird:
                    SwordAttackFirst();
                    wasAttackExecuted = true;
                    return timeOfInvisibility1;

                case Attack.AfterFirst:
                    SwordAttackSecond();
                    wasAttackExecuted = true;
                    return timeOfInvisibility2;

                case Attack.AfterSecond:
                    SwordAttackThird();
                    wasAttackExecuted = true;
                    return timeOfInvisibility3;

                default:
                    wasAttackExecuted = false;
                    return 0.0f;
            }
        }
        else
        {
            wasAttackExecuted = false;
            return 0.0f;
        }
    }

    private bool CanPlayerAttack()
    {
        playerComboStatus = GetAttackNumber();
        if (playerComboStatus.Equals(Attack.Attacking))
            return false;
        else
            return true;
    }


    private Attack GetAttackNumber()
    {
        AnimatorStateInfo animatorStateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);
        if (animatorStateInfo.IsTag("AttackTransition1"))
        {
            return Attack.AfterFirst;
        }
        else if (animatorStateInfo.IsTag("AttackTransition2"))
        {
            return Attack.AfterSecond;
        }
        else if (animatorStateInfo.IsTag("AttackTransition3"))
        {
            return Attack.AfterThird;
        }
        else if (animatorStateInfo.IsTag("Attack"))
        {
            return Attack.Attacking;
        }
        else
        {
            return Attack.None;
        }
    }

    private void SwordAttackFirst()
    {
        playerAnimator.SetTrigger("Attack");
        audioManager.Play("PlayerSwordAttack1");
    }

    private void SwordAttackSecond()
    {
        playerAnimator.SetTrigger("Attack2");
        audioManager.Play("PlayerSwordAttack2");
    }

    private void SwordAttackThird()
    {
        playerAnimator.SetTrigger("Attack3");
        audioManager.Play("PlayerSwordAttack3");
    }

    private bool GetAttackStatus()
    {
        return wasAttackExecuted;
    }
}
