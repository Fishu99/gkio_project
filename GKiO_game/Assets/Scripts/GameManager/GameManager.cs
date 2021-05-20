using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Poziomy trudno�ci gry
 */
public enum Difficulty
{
    Easy,
    Medium,
    Hard
}


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private AudioManager audioManager;
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
    public int PlayerArrows { get; set; }
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
    public FeatureUpgrade SwordDamageUpgrade;
    public FeatureUpgrade BowDamageUpgrade;
    public FeatureUpgrade BowForceUpgrade;

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

    /**
     * Inicjalizacja gameManagera po uruchomieniu
     * 
     */
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
        int width = PlayerPrefs.GetInt("width", bestResolution.width);
        int height = PlayerPrefs.GetInt("height", bestResolution.height);
        if(IsValidResolution(width, height))
        {
            Screen.SetResolution(width, height, fullscreen);
        }
    }

    private bool IsValidResolution(int width, int height)
    {
        return Array.Exists(Screen.resolutions, res => res.width == width && res.height == height);
    }

    /**
     * W tej metodzie s� tworzone obiekty klasy FeatureUpgrade, kt�re opisuj� kolejne poziomy ulepsze�.
     */
    private void InitializeUpgrades()
    {
        var swordDamageUpgradeLevels = new FeatureLevel[]
        {
            new FeatureLevel { FeatureValue = 30, Cost = 0}, //FeatureValue pierwszego elementu to pocz�tkowa warto�� swordDamage
            new FeatureLevel { FeatureValue = 40, Cost = 1},
            new FeatureLevel { FeatureValue = 60, Cost = 2},
            new FeatureLevel { FeatureValue = 80, Cost = 3}
        };
        SwordDamageUpgrade = new FeatureUpgrade(swordDamageUpgradeLevels);

        var bowDamageUpgradeLevels = new FeatureLevel[]
        {
            new FeatureLevel { FeatureValue = 30, Cost = 0},
            new FeatureLevel { FeatureValue = 45, Cost = 2},
            new FeatureLevel { FeatureValue = 60, Cost = 3},
            new FeatureLevel { FeatureValue = 80, Cost = 4}
        };
        BowDamageUpgrade = new FeatureUpgrade(bowDamageUpgradeLevels);

        var bowForceUpgradeLevels = new FeatureLevel[]
        {
            new FeatureLevel { FeatureValue = 10, Cost = 0},
            new FeatureLevel { FeatureValue = 15, Cost = 2},
            new FeatureLevel { FeatureValue = 20, Cost = 3},
            new FeatureLevel { FeatureValue = 30, Cost = 4}
        };
        BowForceUpgrade = new FeatureUpgrade(bowForceUpgradeLevels);
    }

    /**
     * Ta funkcja pozwala za�adowa� odpowiednie warto�ci r�nych parametr�w
     * je�eli gra nie zosta�a uruchomiona w menu g��wnym, ale na jakim� z poziom�w.
     * Dzi�ki temu mo�na wygodnie testowa� poziomy
     */
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
        audioManager = AudioManager.instance;
    }

    /**
     * Po za�adowaniu sceny w��czany jest licznik czasu
     * odmierzaj�cy czas przej�cia poziomu
     */
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (System.Array.Exists(sceneData, s => s.SceneName == scene.name))
        {
            ScoreCounter.StartTimeCount();
        }
    }

    void Start()
    {
        GetSettingsFromPlayerPrefs();
        Debug.Log("starting game manager");
    }

    
    void Update()
    {
        
    }

    

    

    public void SelectDifficultyAndGoNext(Difficulty difficulty)
    {
        CurrentDifficulty = difficulty;
        //LoadFirstLevel();
        LoadIntroduction();
    }

    /**
     * Czynno�ci wykonywane po tym, jak gracz dojdzie do ko�ca poziomu
     */
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
        LoadScene(newSceneData.SceneName);
        if(newSceneData.MusicName != null)
        {
            audioManager.PlayMusicExclusive(newSceneData.MusicName);
        }
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

    private void LoadWinScene()
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

    private void LoadMenuScene(string sceneName)
    {
        audioManager.StopAllMusic();
        LoadScene(sceneName);
    }
   

    private void ResetPlayerStatus()
    {
        PlayerMoney = 0;
        SwordDamageUpgrade.Reset();
        ConfigureForCurrentDifficulty();
    }

    

    private void LoadScene(string name)
    {
        var crossfadeController = FindObjectOfType<CrossfadeController>();
        if(crossfadeController == null)
        {
            SceneManager.LoadScene(name);
        }
        else
        {
            StartCoroutine(LoadSceneWithCrossfade(name, crossfadeController));
        }
    }

    private IEnumerator LoadSceneWithCrossfade(string name, CrossfadeController crossfadeController)
    {
        crossfadeController.StartCrossfade();
        yield return new WaitForSeconds(crossfadeController.CrossfadeTime);
        SceneManager.LoadScene(name);
    }

    /**
     * Ustawia r�ne parametry gracza w zale�no�ci od aktualnego poziomu trudno�ci
     */
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

    /**
     * Dodaje zebrane pieni�dze do pieni�dzy gracza i do wyniku.
     */
    public void AddCollectedMoney(int money)
    {
        PlayerMoney += money;
        ScoreCounter.AddCollectedMoney(money);
    }

    /**
     * Dodaje zabitego gracza do wyniku.
     */
    public void AddKilledEnemy(int enemyValue = 1)
    {
        ScoreCounter.AddKilledEnemy(enemyValue);
    }

}
