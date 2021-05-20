using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLightController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private PlayerController playerController;
    private Vector3 lightOffset;

    void Start()
    {
        lightOffset = transform.position - player.transform.position;
        playerController = player.GetComponent<PlayerController>();
    }

   
    void Update()
    {
        
        if (playerController.IsInDeadZone)
        {
            RotateToLookAtPlayer();
        }
        else
        {
            FollowFromSide();
        }
    }

    void FollowFromSide()
    {
        transform.position = player.transform.position + lightOffset;
        transform.rotation = Quaternion.LookRotation(Vector3.forward);
    }

    void RotateToLookAtPlayer()
    {
        Vector3 playerFollowedPoint = player.transform.position + new Vector3(lightOffset.x, lightOffset.y, 0);
        Vector3 playerDirection = playerFollowedPoint - transform.position;
        transform.rotation = Quaternion.LookRotation(playerDirection);
    }
}
