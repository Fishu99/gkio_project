using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    private Collider objectCollider;
    public float swordLength = 0.5f;
    public float swordDamage = 30;
    public float swordDelay = 0.5f;
    public int layerToHit = 9;
    bool isAttacking = false;
    // Start is called before the first frame update
    void Start()
    {
        objectCollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            Invoke("HitAndDamage", swordDelay);
        }
    }

    private void HitAndDamage()
    {
        isAttacking = false;
        bool wasHit = checkHit(out RaycastHit hitinfo);
        Debug.Log(wasHit);
        if (wasHit)
        {
            DamageHealth(hitinfo);
        }
    }

    private bool checkHit(out RaycastHit hitinfo)
    {
        Vector3 origin = objectCollider.bounds.center;
        float rayLength = objectCollider.bounds.extents.z + swordLength;
        int layerMask = 1 << layerToHit;
        Debug.DrawRay(origin, transform.forward);
        return Physics.Raycast(origin, transform.forward, out hitinfo, rayLength, layerMask);
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
