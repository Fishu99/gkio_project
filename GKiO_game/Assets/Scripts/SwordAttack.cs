using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : WeaponAttack
{
    private AudioManager audioManager;
    private Collider objectCollider;
    public float swordLength = 0.5f;
    public float swordDamage = 30;
    public float swordDelay = 0.5f;
    //Warstwa, na kt�rej znajduj� si� atakowane obiekty
    public int layerToHit = 9;
    //Zmienna informuje, czy aktualnie wykonywany jest atak
    private bool isAttacking = false;
    void Start()
    {
        objectCollider = GetComponent<Collider>();
        audioManager = AudioManager.instance;
    }

    void Update()
    {
        
    }
    
    //Wykonuje atak po czasie okre�lonym przez swordDelay.
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

    //Sprawdza, czy jaki� obiekt znajduje si� przed atakuj�cym obiektem.
    //Je�eli tak, odbiera mu zdrowie
    private void HitAndDamage()
    {
        isAttacking = false;
        bool wasHit = checkHit(out RaycastHit hitinfo);
        Debug.Log(wasHit);
        if (wasHit)
        {
            audioManager.Play("PlayerSwordHit");
            DamageHealth(hitinfo);
        }
    }

    //Funkcja sprawdza, czy przed obiektem w odleg�o�ci swordLength znajduje si� jaki� obiekt na atakowanej warstwie
    private bool checkHit(out RaycastHit hitinfo)
    {
        const float xMargin = 0.05f;
        Vector3 origin = objectCollider.bounds.center;
        float rayLength = objectCollider.bounds.extents.z + swordLength + xMargin;
        int layerMask = 1 << layerToHit;
        //xMargin trzeba odj��, bo inaczej Unity nie wykryje kolizji kiedy dwa obiekty si� stykaj�
        Vector3 boxExtents = objectCollider.bounds.extents - new Vector3(xMargin, 0 ,0);
        return Physics.BoxCast(origin, boxExtents, transform.forward, out hitinfo, Quaternion.identity, rayLength, layerMask);
    }

    //Funkcja odbiera obiektowi zdrowie w ilo�ci okre�lonej przez swordDamage
    private void DamageHealth(RaycastHit hitinfo)
    {
        HealthManager healthManager = hitinfo.collider.GetComponent<HealthManager>();
        if (healthManager != null)
        {
            healthManager.Damage(swordDamage);
        }
    }
}
