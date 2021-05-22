using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script for controlling the Pause menu.
/// </summary>
public class PauseController : MonoBehaviour
{
    /// <summary>
    /// Tells if the game is paused.
    /// </summary>
    public bool IsPaused { get; private set; } = false;
    [SerializeField] private GameObject pauseMenu;
    private GameManager gameManager;
    private Animator animator;
    private readonly float animationTime = 0.4f;
    void Start()
    {
        gameManager = GameManager.instance;
        pauseMenu.SetActive(false);
        animator = pauseMenu.GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPaused)
                Resume();
            else
                Pause();
        }
    }

    /// <summary>
    /// Pauses the game.
    /// </summary>
    public void Pause()
    {
        animator.SetTrigger("darken");
        IsPaused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    /// <summary>
    /// Resumes the game.
    /// </summary>
    public void Resume()
    {
        StartCoroutine(ResumeCoroutine());
    }

    /// <summary>
    /// Quits the level and returns to main menu.
    /// </summary>
    public void Quit()
    {
        IsPaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        gameManager.LoadMainMenu();
    }

    private IEnumerator ResumeCoroutine()
    {
        animator.SetTrigger("lighten");
        yield return new WaitForSecondsRealtime(animationTime);
        IsPaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
}
