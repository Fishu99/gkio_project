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
    /// <summary>
    /// The singleton instance of the object.
    /// </summary>
    public static GameManager instance;
    /// <summary>
    /// Current difficulty of the game.
    /// </summary>
    private Difficulty currentDifficulty = Difficulty.Hard;
    /// <summary>
    /// Current level index
    /// </summary>
    private int currentLevel = 0;
    /// <summary>
    /// Information about all levels.
    /// </summary>
    private SceneData[] sceneData = new SceneData[]
    {
        new SceneData { SceneName = "Level1_Dungeon", LevelName = "Dungeon", ExpectedTime = 10*60 , MusicName = "DungeonTheme"},
        new SceneData { SceneName = "Level2_Woods", LevelName = "Forest", ExpectedTime = 6*60 , MusicName = "WoodsTheme"}
    };
    /// <summary>
    /// Current player health.
    /// </summary>
    public float PlayerHealth { get; set; }
    /// <summary>
    /// Max player health.
    /// </summary>
    public float PlayerMaxHealth { get; private set; }
    /// <summary>
    /// Number of lives of the player
    /// </summary>
    public int PlayerLives { get; set; }
    /// <summary>
    /// Player's money.
    /// </summary>
    public int PlayerMoney { get; set; }
    /// <summary>
    /// Number of player's arrows.
    /// </summary>
    public int PlayerArrows { get; set; }
    /// <summary>
    /// Current difficulty of the game.
    /// </summary>
    public Difficulty CurrentDifficulty
    {
        get => currentDifficulty;
        set
        {
            currentDifficulty = value;
            ConfigureForCurrentDifficulty();
        }
    }
    /// <summary>
    /// Current sword damage of the player.
    /// </summary>
    public float PlayerSwordDamage
    {
        get => SwordDamageUpgrade.CurrentFeatureValue;
    }
    /// <summary>
    /// Current bow damage of the player.
    /// </summary>
    public float BowArrowDamage
    {
        get => BowDamageUpgrade.CurrentFeatureValue;
    }
    /// <summary>
    /// Current force of player's bow.
    /// </summary>
    public float BowArrowForce
    {
        get => BowForceUpgrade.CurrentFeatureValue;
    }
    /// <summary>
    /// Object for counting score.
    /// </summary>
    public ScoreCounter ScoreCounter { get; set; }
    /// <summary>
    /// Is true when the game is over. I happens when the player has 0 lives.
    /// </summary>
    public bool IsGameOver
    {
        get => PlayerLives == 0;
    }
    /// <summary>
    /// Returns true if a level was finished and a new one hasn't been loaded.
    /// </summary>
    public bool IsLevelFinished { get; private set; } = false;
    /// <summary>
    /// Object managing sword damage upgrades.
    /// </summary>
    public FeatureUpgrade SwordDamageUpgrade { get; private set; }
    /// <summary>
    /// Object managing bow damage upgrades.
    /// </summary>
    public FeatureUpgrade BowDamageUpgrade { get; private set; }
    /// <summary>
    /// Object managing bow force upgrades.
    /// </summary>
    public FeatureUpgrade BowForceUpgrade { get; private set; }

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

    /// <summary>
    /// Sets the difficulty and loads the scene which should pe displayed next.
    /// </summary>
    /// <param name="difficulty">Selected difficulty of the game</param>
    public void SelectDifficultyAndGoNext(Difficulty difficulty)
    {
        CurrentDifficulty = difficulty;
        LoadIntroduction();
    }

    /// <summary>
    /// Finishes the level. It is intended to be called when player reaches the end of the level.
    /// </summary>
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

    /// <summary>
    /// Loads next level.
    /// </summary>
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

    /// <summary>
    /// Loads the current level.
    /// </summary>
    public void LoadCurrentLevel()
    {
        SceneData newSceneData = sceneData[currentLevel];
        ScoreCounter.NewScene(newSceneData);
        LoadScene(newSceneData.SceneName, newSceneData.MusicName);
    }

    /// <summary>
    /// Loads the scene with difficulty select.
    /// </summary>
    public void LoadDifficultySelect()
    {
        LoadMenuScene("DifficultySelect");
    }

    /// <summary>
    /// Loads Introduction scene.
    /// </summary>
    public void LoadIntroduction()
    {
        LoadMenuScene("Introduction");
    }

    /// <summary>
    /// Loads MainMenu scene.
    /// </summary>
    public void LoadMainMenu()
    {
        LoadMenuScene("MainMenu");
    }

    /// <summary>
    /// Loads Workshop scene.
    /// </summary>
    public void LoadWorkshop()
    {
        LoadMenuScene("Workshop");
    }

    /// <summary>
    /// Loads WinScene.
    /// </summary>
    public void LoadWinScene()
    {
        LoadMenuScene("WinScene");
    }

    /// <summary>
    /// Loads the first level and resets all player status.
    /// </summary>
    public void LoadFirstLevel()
    {
        ScoreCounter.NewGame();
        ScoreCounter.CurrentDifficulty = CurrentDifficulty;
        ResetPlayerStatus();
        currentLevel = 0;
        LoadCurrentLevel();
    }

    /// <summary>
    /// Loads the scene which should be displayed after finishing level.
    /// It is either Workshop if there are more levels
    /// or WinScene if there are no more levels.
    /// </summary>
    public void LoadWorkshopOrWinScene()
    {
        if (currentLevel + 1 < sceneData.Length)
            LoadWorkshop();
        else
            LoadWinScene();
    }

    /// <summary>
    /// Adds collected money to game score.
    /// </summary>
    /// <param name="money"></param>
    public void AddCollectedMoney(int money)
    {
        PlayerMoney += money;
        ScoreCounter.AddCollectedMoney(money);
    }

    /// <summary>
    /// Adds killed enemy to game score.
    /// </summary>
    /// <param name="enemyValue"></param>
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
