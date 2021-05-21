using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script for the camera which makes it follow player.
/// The camera maintains the same distance from the player as at the beginning.
/// The only exception is when player falls into water or lava:
/// it stays in a fixed place and rotates to follow the player.
/// </summary>
public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private PlayerController playerController;
    private Vector3 cameraOffset;

    void Start()
    {
        cameraOffset = transform.position - player.transform.position;
        playerController = player.GetComponent<PlayerController>();
        playerController.playerCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
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
        transform.position = player.transform.position + cameraOffset;
        transform.rotation = Quaternion.LookRotation(Vector3.forward);
    }

    void RotateToLookAtPlayer()
    {
        Vector3 playerFollowedPoint = player.transform.position + new Vector3(cameraOffset.x, cameraOffset.y, 0);
        Vector3 playerDirection = playerFollowedPoint - transform.position;
        transform.rotation = Quaternion.LookRotation(playerDirection);
    }
}
