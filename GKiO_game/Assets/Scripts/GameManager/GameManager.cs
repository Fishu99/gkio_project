using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Poziomy trudnoœci gry
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
    public float PlayerArrowDamage
    {
        get => ArrowDamageUpgrade.CurrentFeatureValue;
    }
    public FeatureUpgrade SwordDamageUpgrade;
    public FeatureUpgrade ArrowDamageUpgrade;

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

    /**
     * W tej metodzie s¹ tworzone obiekty klasy FeatureUpgrade, które opisuj¹ kolejne poziomy ulepszeñ.
     */
    private void InitializeUpgrades()
    {
        var swordDamageUpgradeLevels = new FeatureLevel[]
        {
            new FeatureLevel { FeatureValue = 30, Cost = 0}, //FeatureValue pierwszego elementu to pocz¹tkowa wartoœæ swordDamage
            new FeatureLevel { FeatureValue = 40, Cost = 1},
            new FeatureLevel { FeatureValue = 60, Cost = 2},
            new FeatureLevel { FeatureValue = 80, Cost = 3}
        };
        SwordDamageUpgrade = new FeatureUpgrade(swordDamageUpgradeLevels);

        var bowDamageUpgradeLevels = new FeatureLevel[]
        {
            new FeatureLevel { FeatureValue = 30, Cost = 0},
            new FeatureLevel { FeatureValue = 45, Cost = 3},
            new FeatureLevel { FeatureValue = 60, Cost = 6},
            new FeatureLevel { FeatureValue = 80, Cost = 10}
        };
        ArrowDamageUpgrade = new FeatureUpgrade(bowDamageUpgradeLevels);
    }

    /**
     * Ta funkcja pozwala za³adowaæ odpowiednie wartoœci ró¿nych parametrów
     * je¿eli gra nie zosta³a uruchomiona w menu g³ównym, ale na jakimœ z poziomów.
     * Dziêki temu mo¿na wygodnie testowaæ poziomy
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
    }

    /**
     * Po za³adowaniu sceny w³¹czany jest licznik czasu
     * odmierzaj¹cy czas przejœcia poziomu
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

    /**
     * Czynnoœci wykonywane po tym, jak gracz dojdzie do koñca poziomu
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

    public void LoadWorkshop()
    {
        SceneManager.LoadScene("Workshop");
    }

    private void ResetPlayerStatus()
    {
        PlayerMoney = 0;
        SwordDamageUpgrade.Reset();
        ConfigureForCurrentDifficulty();
    }

    private void LoadWinScene()
    {
        SceneManager.LoadScene("WinScene");
    }

    /**
     * Ustawia ró¿ne parametry gracza w zale¿noœci od aktualnego poziomu trudnoœci
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
     * Dodaje zebrane pieni¹dze do pieniêdzy gracza i do wyniku.
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

    /**
     * Kupuje wskazane ulepszenie, je¿eli dan¹ wartoœæ mo¿na jeszcze ulepszyæ i jeœli gracza na to staæ.
     */
    public void BuyUpgrade(FeatureUpgrade featureUpgrade)
    {
        if (featureUpgrade.IsUpgradePossible)
        {
            int cost = featureUpgrade.UpgradeCost;
            if (cost <= PlayerMoney)
            {
                PlayerMoney -= cost;
                featureUpgrade.Upgrade();
            }
        }
    }
}
