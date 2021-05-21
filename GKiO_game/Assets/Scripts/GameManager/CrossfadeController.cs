using UnityEngine;

/// <summary>
/// A script for controlling the crossfade transition between scenes.
/// </summary>
public class CrossfadeController : MonoBehaviour
{
    private Animator animator;
    public float CrossfadeTime { get; private set; } = 1f;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void StartCrossfade()
    {
        animator.SetTrigger("start");
    }
}
