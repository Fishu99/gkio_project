using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Difficulty levels of the game.
/// </summary>
public enum Difficulty
{
    Easy,
    Medium,
    Hard
}

/// <summary>
/// The class for managing information between scenes.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private Difficulty currentDifficulty = Difficulty.Hard;
    public FeatureUpgrade SwordDamageUpgrade;
    public FeatureUpgrade BowDamageUpgrade;
    public FeatureUpgrade BowForceUpgrade;
    private int currentLevel = 0;
    private SceneData[] sceneData = new SceneData[]
    {
        new SceneData { SceneName = "Level1_Dungeon", LevelName = "Dungeon", ExpectedTime = 10*60 , MusicName = "DungeonTheme"},
        new SceneData { SceneName = "Level2_Woods", LevelName = "Forest", ExpectedTime = 6*60 , MusicName = "WoodsTheme"}
    };

    public float PlayerHealth { get; set; }
    public float PlayerMaxHealth { get; private set; }
    public int PlayerLives { get; set; }
    public int PlayerMoney { get; set; }
    public int PlayerArrows { get; set; }
    public Difficulty CurrentDifficulty
    {
        get => currentDifficulty;
        set
        {
            currentDifficulty = value;
            ConfigureForCurrentDifficulty();
        }
    }
    public float PlayerSwordDamage
    {
        get => SwordDamageUpgrade.CurrentFeatureValue;
    }
    public float BowArrowDamage
    {
        get => BowDamageUpgrade.CurrentFeatureValue;
    }
    public float BowArrowForce
    {
        get => BowForceUpgrade.CurrentFeatureValue;
    }

    public ScoreCounter ScoreCounter { get; set; }
    public bool IsGameOver
    {
        get => PlayerLives == 0;
    }
    public bool IsLevelFinished { get; private set; } = false;


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

    

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (Array.Exists(sceneData, s => s.SceneName == scene.name))
        {
            ScoreCounter.StartTimeCount();
        }
    }

    void Start()
    {
        GetSettingsFromPlayerPrefs();
    }

    
    public void SelectDifficultyAndGoNext(Difficulty difficulty)
    {
        CurrentDifficulty = difficulty;
        LoadIntroduction();
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

    public void LoadCurrentLevel()
    {
        SceneData newSceneData = sceneData[currentLevel];
        ScoreCounter.NewScene(newSceneData);
        LoadScene(newSceneData.SceneName, newSceneData.MusicName);
    }

    public void LoadDifficultySelect()
    {
        LoadMenuScene("DifficultySelect");
    }

    public void LoadIntroduction()
    {
        LoadMenuScene("Introduction");
    }

    public void LoadMainMenu()
    {
        LoadMenuScene("MainMenu2");
    }

    public void LoadWorkshop()
    {
        LoadMenuScene("Workshop");
    }

    public void LoadWinScene()
    {
        LoadMenuScene("WinScene");
    }

    public void LoadFirstLevel()
    {
        ScoreCounter.NewGame();
        ScoreCounter.CurrentDifficulty = CurrentDifficulty;
        ResetPlayerStatus();
        currentLevel = 0;
        LoadCurrentLevel();
    }

    public void LoadWorkshopOrWinScene()
    {
        if (currentLevel + 1 < sceneData.Length)
            LoadWorkshop();
        else
            LoadWinScene();
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

    private void Initialize()
    {
        currentLevel = 0;
        ScoreCounter = new ScoreCounter();
        InitializeUpgrades();
        CheckFirstScene();
        ResetPlayerStatus();
    }

    private void GetSettingsFromPlayerPrefs()
    {
        int fullscreenPref = PlayerPrefs.GetInt("fullscreen", 1);
        bool fullscreen = fullscreenPref != 0;
        Resolution bestResolution = Screen.resolutions[Screen.resolutions.Length - 1];
        int width = PlayerPrefs.GetInt("swidth", bestResolution.width);
        int height = PlayerPrefs.GetInt("sheight", bestResolution.height);
        if (IsValidResolution(width, height))
        {
            Screen.SetResolution(width, height, fullscreen);
        }
        AudioManager.instance.MusicVolume = PlayerPrefs.GetFloat("musicVolume", 1);
        AudioManager.instance.SoundVolume = PlayerPrefs.GetFloat("soundVolume", 1);
    }

    private bool IsValidResolution(int width, int height)
    {
        return Array.Exists(Screen.resolutions, res => res.width == width && res.height == height);
    }

    private void InitializeUpgrades()
    {
        var swordDamageUpgradeLevels = new FeatureLevel[]
        {
            new FeatureLevel { FeatureValue = 30, Cost = 0}, //FeatureValue pierwszego elementu to pocz�tkowa warto�� swordDamage
            new FeatureLevel { FeatureValue = 40, Cost = 10},
            new FeatureLevel { FeatureValue = 60, Cost = 15},
            new FeatureLevel { FeatureValue = 80, Cost = 20}
        };
        SwordDamageUpgrade = new FeatureUpgrade(swordDamageUpgradeLevels);

        var bowDamageUpgradeLevels = new FeatureLevel[]
        {
            new FeatureLevel { FeatureValue = 30, Cost = 0},
            new FeatureLevel { FeatureValue = 45, Cost = 10},
            new FeatureLevel { FeatureValue = 60, Cost = 15},
            new FeatureLevel { FeatureValue = 80, Cost = 20}
        };
        BowDamageUpgrade = new FeatureUpgrade(bowDamageUpgradeLevels);

        var bowForceUpgradeLevels = new FeatureLevel[]
        {
            new FeatureLevel { FeatureValue = 10, Cost = 0},
            new FeatureLevel { FeatureValue = 15, Cost = 10},
            new FeatureLevel { FeatureValue = 20, Cost = 15},
            new FeatureLevel { FeatureValue = 30, Cost = 20}
        };
        BowForceUpgrade = new FeatureUpgrade(bowForceUpgradeLevels);
    }

    private void CheckFirstScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        int index = Array.FindIndex(sceneData, s => s.SceneName == sceneName);
        if (index != -1)
        {
            currentLevel = index;
            IsLevelFinished = false;
            ScoreCounter.NewGame();
            ScoreCounter.CurrentDifficulty = CurrentDifficulty;
            ScoreCounter.NewScene(sceneData[index]);
            PlayMusic(sceneData[index].MusicName);
            ResetPlayerStatus();
        }
        else
        {
            PlayMusic("MenuTheme");
        }
    }

    private void LoadMenuScene(string sceneName)
    {
        LoadScene(sceneName, "MenuTheme");
    }
   

    private void ResetPlayerStatus()
    {
        PlayerMoney = 0;
        SwordDamageUpgrade.Reset();
        ConfigureForCurrentDifficulty();
    }

    private void LoadScene(string name, string musicName = null)
    {
        var crossfadeController = FindObjectOfType<CrossfadeController>();
        if(crossfadeController == null)
        {
            SceneManager.LoadScene(name);
            PlayMusic(musicName);
        }
        else
        {
            StartCoroutine(LoadSceneWithCrossfade(name, crossfadeController, musicName));
        }
    }

    private IEnumerator LoadSceneWithCrossfade(string name, CrossfadeController crossfadeController, string musicName)
    {
        crossfadeController.StartCrossfade();
        yield return new WaitForSeconds(crossfadeController.CrossfadeTime);
        SceneManager.LoadScene(name);
        PlayMusic(musicName);
    }

    private void PlayMusic(string musicName)
    {
        if(musicName == null)
            AudioManager.instance.StopAllMusic();
        else
            AudioManager.instance.PlayMusicExclusiveIfNotPlayed(musicName);
    }

    private void ConfigureForCurrentDifficulty()
    {
        switch (currentDifficulty)
        {
            case Difficulty.Easy:
                PlayerMaxHealth = 200;
                PlayerLives = 10;
                PlayerArrows = 50;
                break;
            case Difficulty.Medium:
                PlayerMaxHealth = 200;
                PlayerLives = 5;
                PlayerArrows = 30;
                break;
            case Difficulty.Hard:
                PlayerMaxHealth = 100;
                PlayerLives = 3;
                PlayerArrows = 20;
                break;
        }
        PlayerHealth = PlayerMaxHealth;
    }

    

}
