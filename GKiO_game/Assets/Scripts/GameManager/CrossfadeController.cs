using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossfadeController : MonoBehaviour
{
    private Animator animator;
    public float CrossfadeTime { get; private set; } = 1f;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void StartCrossfade()
    {
        animator.SetTrigger("start");
    }
}
