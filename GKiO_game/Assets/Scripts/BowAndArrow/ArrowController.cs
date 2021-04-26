using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    //Informuje o tym, czy strza³a jest niebezpieczna. 
    //Strza³a jest niebezpieczna od wywo³ania funkcji Shoot do trafienia strza³y w dowoln¹ przeszkodê
    private bool isHarmful = false;
    public float arrowDamage = 30;
    public float arrowForce = 0.01f;
    Rigidbody arrowRigidBody;
    //Warstwa, na której znajduj¹ siê atakowane obiekty
    public int layerToHit = 9;
    
    void Start()
    {
        arrowRigidBody = GetComponent<Rigidbody>();
    }

    
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (isHarmful)
        {
            RotateByVelocity();
        }
    }

    private void RotateByVelocity()
    {
        if(arrowRigidBody.velocity.magnitude > 0)
            transform.rotation = Quaternion.FromToRotation(Vector3.up, arrowRigidBody.velocity);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Arrow collision!");
        if (isHarmful)
        {
            DamageHealth(collision);
        }
        arrowRigidBody.constraints = RigidbodyConstraints.FreezeAll;
        isHarmful = false;
        Invoke("DestroyAfterCollision", 0.5f);
    }

    public void Shoot()
    {
        arrowRigidBody = GetComponent<Rigidbody>();
        isHarmful = true;
        arrowRigidBody.AddForce(transform.up * arrowForce, ForceMode.Impulse);
    }


    private void DamageHealth(Collision collision)
    {
        if (collision.gameObject.layer == layerToHit)
        {
            HealthManager healthManager = collision.gameObject.GetComponent<HealthManager>();
            if (healthManager != null)
            {
                healthManager.Damage(arrowDamage);
            }
        }
    }

    private void DestroyAfterCollision()
    {
        Destroy(gameObject);
    }
}
