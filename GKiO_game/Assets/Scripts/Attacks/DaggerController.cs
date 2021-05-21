using UnityEngine;

/// <summary>
/// Class for controlling a dagger.
/// It includes methods for computing the correct angle of throw so as to hit the aim.
/// </summary>
public class DaggerController : MonoBehaviour
{
    private AudioManager audioManager;
    private bool isHarmful = false;
    public float damage = 30;
    public float initialSpeed = 6f;
    public float rotationSpeed = 6f;
    Rigidbody daggerRigidBody;
    public int layerToHit = 6;

    private void Awake()
    {
        audioManager = AudioManager.instance;
        daggerRigidBody = GetComponent<Rigidbody>();
        DisablePhysics();
    }

    public void Throw(Vector3 aim)
    {
        Vector3 velocity = FindVelocity(aim);
        if (velocity != Vector3.zero)
        {
            EnablePhysics();
            daggerRigidBody.velocity = velocity;
            daggerRigidBody.angularVelocity = transform.up * rotationSpeed;
            isHarmful = true;
        }
    }

    private void DisablePhysics()
    {
        daggerRigidBody.isKinematic = true;
        daggerRigidBody.detectCollisions = false;
    }

    private void EnablePhysics()
    {
        daggerRigidBody.isKinematic = false;
        daggerRigidBody.detectCollisions = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isHarmful)
        {
            if (collision.gameObject.CompareTag("Player"))
                audioManager.Play("PlayerHitByDagger");
            DamageHealth(collision);
        }
        daggerRigidBody.constraints = RigidbodyConstraints.FreezeAll;
        isHarmful = false;
        Invoke("DestroyAfterCollision", 0.5f);
    }

    private float FindThrowAngle(Vector3 aim)
    {
        Vector3 origin = transform.position;
        Vector3 horizontalDisp = new Vector3(aim.x - origin.x, 0, aim.z - origin.z);
        float dxz = horizontalDisp.magnitude;
        float dy = aim.y - origin.y;
        float g = Mathf.Abs(Physics.gravity.y);
        float v = initialSpeed;
        float delta = Mathf.Pow(v, 4) - 2 * g * dy * v * v - g * g * dxz * dxz;
        if (delta < 0)
            return float.NaN;
        float angle = Mathf.Atan((v * v - Mathf.Sqrt(delta)) / (g * dxz));
        return Mathf.Rad2Deg*angle;
    }

    private float FindDirectAngle(Vector3 aim)
    {
        Vector3 origin = transform.position;
        Vector3 horizontalDisp = new Vector3(aim.x - origin.x, 0, aim.z - origin.z);
        float dxz = horizontalDisp.magnitude;
        float dy = aim.y - origin.y;
        float angle = Mathf.Atan(dy / dxz);
        return Mathf.Rad2Deg * angle;
    }

    private Vector3 FindVelocity(Vector3 aim)
    {
        float angle = FindThrowAngle(aim);
        Debug.Log(angle);
        if (float.IsNaN(angle))
            angle = FindDirectAngle(aim);
        Vector3 origin = transform.position;
        Vector3 horizontalDisp = new Vector3(aim.x - origin.x, 0, aim.z - origin.z);
        Vector3 horizontalDispNorm = horizontalDisp.normalized;
        Vector3 axis = Vector3.Cross(horizontalDispNorm, Vector3.up);
        Quaternion rotation = Quaternion.AngleAxis(angle, axis);
        Vector3 velocityUnitVector = rotation * horizontalDispNorm;
        Vector3 velocity = velocityUnitVector.normalized * initialSpeed;
        return velocity;
    }

    

    private void DamageHealth(Collision collision)
    {
        if (collision.gameObject.layer == layerToHit)
        {
            HealthManager healthManager = collision.gameObject.GetComponent<HealthManager>();
            if (healthManager != null)
            {
                healthManager.Damage(damage);
            }
        }
    }

    private void DestroyAfterCollision()
    {
        Destroy(gameObject);
    }
}
