using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Difficulty
{
    Easy,
    Medium,
    Hard
}


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private Difficulty currentDifficulty = Difficulty.Hard;
    public Difficulty CurrentDifficulty {
        get => currentDifficulty;
        set
        {
            currentDifficulty = value;
            ConfigureForCurrentDifficulty();
        }
    }
    public float PlayerHealth { get; set; }
    public float PlayerMaxHealth { get; private set; }
    public int PlayerLives { get; set; }
    public int PlayerMoney { get; set; }
    public int Score { get; set; }
    public bool IsGameOver {
        get => PlayerLives == 0;
    }
    public bool IsLevelFinished { get; private set; } = false;
    private int currentLevel = 0;
    private string[] levelNames = new string[] {"DBDungeon", "DBForest" };

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Initialize();
    }

    private void Initialize()
    {
        ResetPlayerStatus();
        currentLevel = 0;
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void FinishLevel()
    {
        if (!IsLevelFinished)
        {
            IsLevelFinished = true;
            Debug.Log("Level finished");
            
        }
    }

    public void LoadNextLevel()
    {
        
        currentLevel++;
        if(currentLevel < levelNames.Length)
        {
            LoadCurrentLevel();
        }
        else
        {
            LoadWinScene();
        }
        IsLevelFinished = false;
    }

    private void LoadCurrentLevel()
    {
        SceneManager.LoadScene(levelNames[currentLevel]);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadFirstLevel()
    {
        ResetPlayerStatus();
        currentLevel = 0;
        LoadCurrentLevel();
    }

    private void ResetPlayerStatus()
    {
        PlayerMoney = 0;
        Score = 0;
        ConfigureForCurrentDifficulty();
    }

    private void LoadWinScene()
    {
        SceneManager.LoadScene("WinScene");
    }

    private void ConfigureForCurrentDifficulty()
    {
        switch (currentDifficulty)
        {
            case Difficulty.Easy:
                PlayerMaxHealth = 200;
                PlayerLives = 10;
                break;
            case Difficulty.Medium:
                PlayerMaxHealth = 200;
                PlayerLives = 5;
                break;
            case Difficulty.Hard:
                PlayerMaxHealth = 100;
                PlayerLives = 3;
                break;
        }
        PlayerHealth = PlayerMaxHealth;
    }
}
