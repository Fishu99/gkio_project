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
    private Vector3 cameraOffset;
    // Start is called before the first frame update
    void Start()
    {
        cameraOffset = transform.position - player.transform.position;
        player.GetComponent<PlayerController>().playerCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + cameraOffset;
    }
}
