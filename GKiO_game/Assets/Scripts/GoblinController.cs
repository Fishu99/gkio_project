using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Goblin chodzi od punktu, w którym siê go wstawi,
 * do punktu znajduj¹ cego siê w odleg³oœci walkRange na prawo od punktu wstawienia goblina
 */
public class GoblinController : MonoBehaviour
{
    private Rigidbody goblinRigidBody;
    private Animation goblinAnimation;
    //Prêdkoœæ goblina
    [SerializeField] private float goblinSpeed = 4f;
    [SerializeField] private float walkRange = 10f;
    //Pocz¹tkowa pozycja goblina pobierana na starcie
    private Vector3 startPosition;
    private float direction = 1f;
    //Czy goblin jest na koñcu swojego obszaru
    private bool isOnEnd = false;
    //Czas, kiedy goblin doszed³ do koñca swojego terenu
    private float beginReturnTime = 0;
    //Czas, jaki goblin czeka na koñcu swoojego obszaru przed zawróceniem
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
            goblinRigidBody.rotation = Quaternion.LookRotation(Vector3.right);
        }
        else if(direction < 0)
        {
            goblinRigidBody.rotation = Quaternion.LookRotation(Vector3.left);
        }
    }

    private void FixedUpdate()
    {
        
        if (isOnEnd)
        {
            FixedUpdateOnEnd();
        }
        else
        {
            CalculateVelocity();
            CheckIfOnEnd();
        }
    }

    private void FixedUpdateOnEnd()
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
        if(WalkedToLeftEnd() || WalkedToRightEnd())
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
        if (WalkedToRightEnd())
        {
            direction = -1f;
        }
        else if (WalkedToLeftEnd())
        {
            direction = 1f;
        }
    }

    private bool WalkedToLeftEnd()
    {
        return direction < 0 && goblinRigidBody.position.x < startPosition.x;
    }

    private bool WalkedToRightEnd()
    {
        return direction > 0 && goblinRigidBody.position.x > startPosition.x + walkRange;
    }

    private void CalculateVelocity()
    {
        float xVelocity = goblinSpeed * direction;
        goblinRigidBody.velocity = new Vector3(xVelocity, goblinRigidBody.velocity.y, goblinRigidBody.velocity.z);
    }
}
