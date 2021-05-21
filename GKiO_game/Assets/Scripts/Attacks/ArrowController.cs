using UnityEngine;

/// <summary>
/// Script for controlling arrows released by the player.
/// It rotates arrow along its velocity, handles collisions with terrain and enemies.
/// </summary>
public class ArrowController : MonoBehaviour
{
    private AudioManager audioManager;
    private bool isHarmful = false;
    public float arrowDamage = 30;
    public float arrowForce = 0.01f;
    Rigidbody arrowRigidBody;
    public int layerToHit = 9;
    
    void Start()
    {
        audioManager = AudioManager.instance;
        arrowRigidBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (isHarmful)
        {
            RotateByVelocity();
        }
    }

    public void Shoot()
    {
        arrowRigidBody = GetComponent<Rigidbody>();
        isHarmful = true;
        arrowRigidBody.AddForce(transform.up * arrowForce, ForceMode.Impulse);
    }

    private void RotateByVelocity()
    {
        if(arrowRigidBody.velocity.magnitude > 0)
            transform.rotation = Quaternion.FromToRotation(Vector3.up, arrowRigidBody.velocity);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isHarmful)
        {
            DamageHealth(collision);
        }
        arrowRigidBody.constraints = RigidbodyConstraints.FreezeAll;
        isHarmful = false;
        Invoke("DestroyAfterCollision", 0.5f);
    }

    private void DamageHealth(Collision collision)
    {
        if (collision.gameObject.layer == layerToHit)
        {
            HealthManager healthManager = collision.gameObject.GetComponent<HealthManager>();
            if (healthManager != null)
            {
                audioManager.Play("PlayerArrowHit");
                healthManager.Damage(arrowDamage);
            }
        }
        else
            audioManager.Play("ArrowHitObstacle");
    }

    private void DestroyAfterCollision()
    {
        Destroy(gameObject);
    }
}
