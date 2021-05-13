using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAlongPathController : MonoBehaviour
{
    public int currentPathIndex = 0;
    [SerializeField] private Vector3[] setPaths;
    [SerializeField] private float[] setWaitTimes;
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private int speedReverseMultiplier = 4;
    [SerializeField] private int speedForwardMultiplier = 4;

    [SerializeField][Tooltip("If platform reaches the end, it is going backwards (not straigth to point 0)")]
    private bool loop = false;
    [SerializeField][Tooltip("If is going backwards, platform has additional speed multiplier")] 
    private bool reverse = false;
    [SerializeField][Tooltip("If is going forwards, platform has additional speed multiplier")]
    private bool forward = false;

    private bool isGoodPrepared = true;
    private bool isWaiting = false;
    private bool isLooping = false;
    private float waitTime;
    public float timeLeft;

    private void OnValidate()
    {
        if (setPaths.Length != setWaitTimes.Length)
        {
            //Debug.LogError(transform.name.ToString() + ": Invalid lengths of setPaths and setWaitTimes - should be equal to themselves!");
            Debug.LogWarning("Array of Paths and WaitTimes for assigned points should have the same elements!");
            isGoodPrepared = false;
        }
        else
        {
            isGoodPrepared = true;
        }

        if (reverse && forward)
        {
            Debug.LogWarning("Reverse and Forward shouldn't be checked at the same time! Change speed value instead.");
            isGoodPrepared = false;
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

            if (reverse && isLooping)
                transform.position = Vector3.MoveTowards(transform.position, setPaths[currentPathIndex], speed * speedReverseMultiplier * Time.deltaTime);
            else if (forward && !isLooping)
                transform.position = Vector3.MoveTowards(transform.position, setPaths[currentPathIndex], speed * speedForwardMultiplier * Time.deltaTime);
            else
                transform.position = Vector3.MoveTowards(transform.position, setPaths[currentPathIndex], speed * Time.deltaTime);

            if (transform.position.x == setPaths[currentPathIndex].x &&
                transform.position.y == setPaths[currentPathIndex].y &&
                transform.position.z == setPaths[currentPathIndex].z)
            {
                if (isLooping && currentPathIndex == 0)
                {
                    isLooping = false;
                }

                waitTime = setWaitTimes[currentPathIndex];
                if (waitTime != 0.0f)
                {
                    isWaiting = true;
                    timeLeft = waitTime;
                }

                if (isLooping)
                    currentPathIndex--;
                else
                    currentPathIndex++;

                if (currentPathIndex >= setPaths.Length)
                { 
                    if(loop)
                    {
                        currentPathIndex -= 2;
                        isLooping = true;
                    }
                    else
                    {
                        currentPathIndex = 0;
                    }
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
