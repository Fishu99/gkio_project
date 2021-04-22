using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    private Collider objectCollider;
    public float swordLength = 0.5f;
    public float swordDamage = 30;
    public float swordDelay = 0.5f;
    public int layerMask = ~0;
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
        Invoke("HitAndDamage", swordDelay);
    }

    private void HitAndDamage()
    {
        Vector3 origin = objectCollider.bounds.center;
        float rayLength = objectCollider.bounds.extents.z + swordLength;
        bool wasHit = Physics.Raycast(origin, Vector3.forward, out RaycastHit hitinfo, rayLength, layerMask);
        if (wasHit)
        {
            DamageHealth(hitinfo);
        }
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
