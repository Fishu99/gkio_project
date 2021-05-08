using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Skrypt dla kamery, ¿eby pod¹¿a³a za graczem.
 * Kamera znajduje siê ca³y czas w takim po³o¿eniu wzglêdem gracza, w jakim jest ustawion a na pocz¹tku
 */
public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private PlayerController playerController;
    private Vector3 cameraOffset;
    // Start is called before the first frame update
    void Start()
    {
        cameraOffset = transform.position - player.transform.position;
        playerController = player.GetComponent<PlayerController>();
        playerController.playerCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = player.transform.position + cameraOffset;
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
        //transform.rotation.SetLookRotation(playerDirection);
        //transform.rotation.SetFromToRotation(transform.forward, playerDirection);
    }
}
