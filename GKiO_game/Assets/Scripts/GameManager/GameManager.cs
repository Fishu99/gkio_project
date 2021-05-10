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
    public FeatureUpgrade<float> SwordDamageUpgrade;

    public ScoreCounter ScoreCounter { get; set; }
    public bool IsGameOver {
        get => PlayerLives == 0;
    }
    public bool IsLevelFinished { get; private set; } = false;
    private int currentLevel = 0;
    private SceneData[] sceneData = new SceneData[]
    {
        new SceneData { SceneName = "DBDungeon", LevelName = "Dungeon", ExpectedTime = 120 },
        new SceneData { SceneName = "DBForest", LevelName = "Forest", ExpectedTime = 90 }
    };

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
        ScoreCounter = new ScoreCounter();
        CheckFirstScene();
    }

    private void InitializeUpgrades()
    {
        var swordDamageUpgradeLevels = new FeatureLevel<float>[]
        {
            new FeatureLevel<float> { FeatureValue = 20, Cost = 5},
            new FeatureLevel<float> { FeatureValue = 30, Cost = 8},
            new FeatureLevel<float> { FeatureValue = 50, Cost = 12},
            new FeatureLevel<float> { FeatureValue = 80, Cost = 20}
        };
        SwordDamageUpgrade = new FeatureUpgrade<float>(swordDamageUpgradeLevels);
    }

    private void CheckFirstScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        int index = System.Array.FindIndex(sceneData, s => s.SceneName == sceneName);
        if (index != -1)
        {
            currentLevel = index;
            IsLevelFinished = false;
            ScoreCounter.NewGame();
            ScoreCounter.CurrentDifficulty = CurrentDifficulty;
            ScoreCounter.NewScene(sceneData[index]);
            ResetPlayerStatus();
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (System.Array.Exists(sceneData, s => s.SceneName == scene.name))
        {
            ScoreCounter.StartTimeCount();
        }
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    

    public void LoadDifficultySelect()
    {
        SceneManager.LoadScene("DifficultySelect");
    }

    public void SelectDifficultyAndGoNext(Difficulty difficulty)
    {
        CurrentDifficulty = difficulty;
        LoadFirstLevel();
    }

    public void FinishLevel()
    {
        if (!IsLevelFinished)
        {
            ScoreCounter.StopTimeCount();
            ScoreCounter.AddSceneScoreToTotalScore();
            IsLevelFinished = true;
            Debug.Log("Level finished");
            
        }
    }

    public void LoadNextLevel()
    {
        
        currentLevel++;
        if(currentLevel < sceneData.Length)
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
        SceneData newSceneData = sceneData[currentLevel];
        ScoreCounter.NewScene(newSceneData);
        SceneManager.LoadScene(newSceneData.SceneName);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu2");
    }

    public void LoadFirstLevel()
    {
        ScoreCounter.NewGame();
        ScoreCounter.CurrentDifficulty = CurrentDifficulty;
        ResetPlayerStatus();
        currentLevel = 0;
        LoadCurrentLevel();
    }

    private void ResetPlayerStatus()
    {
        PlayerMoney = 0;
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

    public void AddCollectedMoney(int money)
    {
        PlayerMoney += money;
        ScoreCounter.AddCollectedMoney(money);
    }

    public void AddKilledEnemy(int enemyValue = 1)
    {
        ScoreCounter.AddKilledEnemy(enemyValue);
    }
}
