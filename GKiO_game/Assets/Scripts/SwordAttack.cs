using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    private Collider objectCollider;
    public float swordLength = 0.5f;
    public float swordDamage = 30;
    public float swordDelay = 0.5f;
    //Warstwa, na której znajduj¹ siê atakowane obiekty
    public int layerToHit = 9;
    //Zmienna informuje, czy aktualnie wykonywany jest atak
    private bool isAttacking = false;
    private float sphereCastRadius;
    void Start()
    {
        objectCollider = GetComponent<Collider>();
        sphereCastRadius = objectCollider.bounds.extents.z;
    }

    void Update()
    {
        
    }
    
    //Wykonuje atak po czasie okreœlonym przez swordDelay.
    public void Attack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            Invoke("HitAndDamage", swordDelay);
        }
    }

    //Sprawdza, czy jakiœ obiekt znajduje siê przed atakuj¹cym obiektem.
    //Je¿eli tak, odbiera mu zdrowie
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

    //Funkcja sprawdza, czy przed obiektem w odleg³oœci swordLength znajduje siê jakiœ obiekt na atakowanej warstwie
    private bool checkHit(out RaycastHit hitinfo)
    {
        Vector3 origin = objectCollider.bounds.center;
        float rayLength = objectCollider.bounds.extents.z + swordLength;
        int layerMask = 1 << layerToHit;
        return Physics.SphereCast(origin, sphereCastRadius, transform.forward, out hitinfo, rayLength, layerMask);
    }

    //Funkcja odbiera obiektowi zdrowie w iloœci okreœlonej przez swordDamage
    private void DamageHealth(RaycastHit hitinfo)
    {
        HealthManager healthManager = hitinfo.collider.GetComponent<HealthManager>();
        if (healthManager != null)
        {
            healthManager.Damage(swordDamage);
        }
    }
}
