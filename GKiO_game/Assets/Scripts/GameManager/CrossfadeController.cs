using UnityEngine;

/// <summary>
/// A script for controlling the crossfade transition between scenes.
/// </summary>
public class CrossfadeController : MonoBehaviour
{
    private Animator animator;
    /// <summary>
    /// Duration of the crossfade animation.
    /// </summary>
    public float CrossfadeTime { get; private set; } = 1f;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Starts the crossfade animation.
    /// </summary>
    public void StartCrossfade()
    {
        animator.SetTrigger("start");
    }
}
