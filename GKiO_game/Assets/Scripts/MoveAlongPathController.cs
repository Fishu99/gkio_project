using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAlongPathController : MonoBehaviour
{
    private int currentPathIndex = 0;
    [SerializeField] private Vector3[] setPaths;
    [SerializeField] private float[] setWaitTimes;
    [SerializeField] private float speed;
    
    private bool isGoodPrepared = true;
    private bool isWaiting = false;
    private float waitTime;
    private float timeLeft;

    private void OnValidate()
    {
        if (setPaths.Length != setWaitTimes.Length)
        {
            //Debug.LogError(transform.name.ToString() + ": Invalid lengths of setPaths and setWaitTimes - should be equal to themselves!");
            isGoodPrepared = false;
        }
        else
        {
            isGoodPrepared = true;
        }
            
    }


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!isWaiting && isGoodPrepared)
        {
            transform.position = Vector3.MoveTowards(transform.position, setPaths[currentPathIndex], speed * Time.deltaTime);
            if (transform.position.x == setPaths[currentPathIndex].x &&
                transform.position.y == setPaths[currentPathIndex].y &&
                transform.position.z == setPaths[currentPathIndex].z)
            {
                waitTime = setWaitTimes[currentPathIndex];
                if (waitTime != 0.0f)
                {
                    isWaiting = true;
                    timeLeft = waitTime;
                }

                currentPathIndex++;
                if (currentPathIndex >= setPaths.Length)
                { 
                    currentPathIndex = 0;
                }
            }
        }
        else
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0.0f)
                isWaiting = false;
        }
    }
}
