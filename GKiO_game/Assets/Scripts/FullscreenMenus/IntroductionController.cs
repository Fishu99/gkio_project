using UnityEngine;

/// <summary>
/// A script controlling the Introduction scene.
/// </summary>
public class IntroductionController : MonoBehaviour
{
    private int storyPartIndex = 1;
    private GameManager gameManager;
    [SerializeField] private GameObject storyPanel;
    [SerializeField] private GameObject controlsPanel;
    [SerializeField] private GameObject storyPart1;
    [SerializeField] private GameObject storyPart2;
    [SerializeField] private GameObject storyPart3;
    void Start()
    {
        gameManager = GameManager.instance;
        storyPart1.SetActive(true);
        storyPart2.SetActive(false);
        storyPart3.SetActive(false);
        storyPanel.SetActive(true);
        controlsPanel.SetActive(false);
    }

    public void NextClick()
    {
        storyPartIndex++;
        if(storyPartIndex == 2)
        {
            storyPart2.SetActive(true);
        }
        else if(storyPartIndex == 3)
        {
            storyPart3.SetActive(true);
        }
        else if(storyPartIndex > 3)
        {
            storyPanel.SetActive(false);
            controlsPanel.SetActive(true);
        }
    }


    public void SkipClick()
    {
        LoadNextScene();
    }

    public void PlayClick()
    {
        LoadNextScene();
    }

    private void LoadNextScene()
    {
        gameManager.LoadFirstLevel();
    }

    
}
