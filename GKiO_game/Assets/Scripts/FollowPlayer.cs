using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Skrypt dla kamery, �eby pod��a�a za graczem.
 * Kamera znajduje si� ca�y czas w takim po�o�eniu wzgl�dem gracza, w jakim jest ustawion a na pocz�tku
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
