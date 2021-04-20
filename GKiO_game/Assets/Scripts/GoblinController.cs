using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinController : MonoBehaviour
{
    private Rigidbody goblinRigidBody;
    private Animation goblinAnimation;
    [SerializeField] private float goblinSpeed = 4f;
    [SerializeField] private float walkRange = 10f;
    private Vector3 startPosition;
    private float direction = 1f;
    private bool isOnEnd = false;
    private float beginReturnTime = 0;
    private float endWaitingTime = 2;
    // Start is called before the first frame update
    void Start()
    {
        goblinRigidBody = GetComponent<Rigidbody>();
        goblinAnimation = GetComponent<Animation>();
        startPosition = transform.position;
        goblinAnimation.Play("walk");
        RotateByDirection();
    }

    // Update is called once per frame
    void Update()
    {
        if (isOnEnd)
        {
            goblinAnimation.Play("idle");
        }
        else
        {
            goblinAnimation.Play("walk");
        }
    }

    private void RotateByDirection()
    {
        if(direction > 0)
        {
            goblinRigidBody.rotation = Quaternion.LookRotation(Vector3.forward);
        }
        else if(direction < 0)
        {
            goblinRigidBody.rotation = Quaternion.LookRotation(Vector3.back);
        }
    }

    private void FixedUpdate()
    {
        
        if (isOnEnd)
        {
            UpdateOnEnd();
        }
        else
        {
            CalculateVelocity();
            CheckIfOnEnd();
        }
    }

    private void UpdateOnEnd()
    {
        float currentTime = Time.time;
        if(currentTime - beginReturnTime > endWaitingTime)
        {
            SetDirectionOnEnd();
            RotateByDirection();
            isOnEnd = false;
        }
    }

    private void CheckIfOnEnd()
    {
        if(direction > 0 && goblinRigidBody.position.z > startPosition.z + walkRange)
        {
            BeginReturn();
        }
        else if (direction < 0 && goblinRigidBody.position.z < startPosition.z)
        {
            BeginReturn();
        }
    }

    private void BeginReturn()
    {
        isOnEnd = true;
        beginReturnTime = Time.time;
    }

    private void SetDirectionOnEnd()
    {
        if (direction > 0 && goblinRigidBody.position.z > startPosition.z + walkRange)
        {
            direction = -1f;
        }
        else if (direction < 0 && goblinRigidBody.position.z < startPosition.z)
        {
            direction = 1f;
        }
    }
    


    private void CalculateVelocity()
    {
        float zVelocity = goblinSpeed * direction;
        goblinRigidBody.velocity = new Vector3(goblinRigidBody.velocity.x, goblinRigidBody.velocity.y, zVelocity);
    }
}
