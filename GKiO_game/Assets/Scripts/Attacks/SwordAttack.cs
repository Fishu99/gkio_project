using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The behaviour for performing attack with a sword.
/// After calling Attack method the script checks if any object on layer layerToHit
/// is within swordLength distance from the attacker.
/// If such object is detected, swordDamage is subtracted from its HealthManager.
/// </summary>
public class SwordAttack : WeaponAttack
{
    private AudioManager audioManager;
    private Collider objectCollider;
    public float swordLength = 0.5f;
    public float swordDamage = 30;
    public float swordDelay = 0.5f;
    public int layerToHit = 9;
    private bool isAttacking = false;

    void Start()
    {
        objectCollider = GetComponent<Collider>();
        audioManager = AudioManager.instance;
    }
    
    public override void Attack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            Invoke("HitAndDamage", swordDelay);
        }
    }
    public override bool IsAimInAttackRange()
    {
        return checkHit(out RaycastHit hitinfo);
    }

    private void HitAndDamage()
    {
        isAttacking = false;
        bool wasHit = checkHit(out RaycastHit hitinfo);
        Debug.Log(wasHit);
        if (wasHit)
        {
            if (hitinfo.transform.gameObject.CompareTag("Enemy"))
                HandleAudioWhenEnemyHit();
            else if (hitinfo.transform.gameObject.CompareTag("Player"))
                HandleAudioWhenPlayerHit();
            DamageHealth(hitinfo);
        }
    }
    private void HandleAudioWhenEnemyHit()
    {
        audioManager.Play("PlayerSwordHit");
        audioManager.Stop("PlayerSwordAttack1");
        audioManager.Stop("PlayerSwordAttack2");
        audioManager.Stop("PlayerSwordAttack3");
    }

    private void HandleAudioWhenPlayerHit()
    {
        audioManager.Play("PlayerHitBySword");
    }

    private bool checkHit(out RaycastHit hitinfo)
    {
        const float xMargin = 0.05f;
        Vector3 origin = objectCollider.bounds.center;
        float rayLength = objectCollider.bounds.extents.z + swordLength + xMargin;
        int layerMask = 1 << layerToHit;
        //It is necessary to subtract xMargin or for correct collision detection between two adjacent objects
        Vector3 boxExtents = objectCollider.bounds.extents - new Vector3(xMargin, 0 ,0);
        return Physics.BoxCast(origin, boxExtents, transform.forward, out hitinfo, Quaternion.identity, rayLength, layerMask);
    }

    private void DamageHealth(RaycastHit hitinfo)
    {
        HealthManager healthManager = hitinfo.collider.GetComponent<HealthManager>();
        if (healthManager != null)
        {
            healthManager.Damage(swordDamage);
        }
    }
}
