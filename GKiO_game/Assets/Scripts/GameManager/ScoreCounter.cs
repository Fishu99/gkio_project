using UnityEngine;

public class ScoreCounter
{
    public float MoneyCoefficient { get; set; } = 1f;
    public float EnemyCoefficient { get; set; } = 1f;
    public Difficulty CurrentDifficulty { get; set; } = Difficulty.Easy;
    public SceneData CurrentSceneData { get; set; }
    private int enemiesKilled = 0;
    private int moneyCollected = 0;
    private float? startTime;
    private float? stopTime;

    public float TotalSceneTime
    {
        get => (stopTime - startTime).Value;
    }

    public float DifficultyMultiplier
    {
        get
        {
            switch (CurrentDifficulty)
            {
                case Difficulty.Easy:
                    return 1;
                case Difficulty.Medium:
                    return 2;
                case Difficulty.Hard:
                    return 4;
                default:
                    return 0;
            }
        }
    }

    public float TimeCoefficient
    {
        get
        {
            float expectedTime = CurrentSceneData.ExpectedTime;
            return 1 + Mathf.Pow(2, (expectedTime - TotalSceneTime) / expectedTime);
        }
    }

    public int CurrentSceneScore
    {
        get => (int)(enemiesKilled * EnemyCoefficient + moneyCollected * MoneyCoefficient);
    }

    public int TotalSceneScore
    {
        get => (int)(TimeCoefficient * CurrentSceneScore * DifficultyMultiplier);
        
    }

    public int TotalGameScore { get; private set; } = 0;

    public void NewGame()
    {
        TotalGameScore = 0;
    }

    public void NewScene(SceneData newSceneData)
    {
        CurrentSceneData = newSceneData;
        Reset();
    }

    public void StartTimeCount()
    {
        startTime = Time.time;
    }

    public void StopTimeCount()
    {
        stopTime = Time.time;
    }

    public void AddKilledEnemy(int enemyValue)
    {
        enemiesKilled += enemyValue;
    }

    public void AddCollectedMoney(int money)
    {
        moneyCollected += money;
    }

    private void Reset()
    {
        enemiesKilled = 0;
        moneyCollected = 0;
        startTime = null;
        stopTime = null;
    }

    public void AddSceneScoreToTotalScore()
    {
        TotalGameScore += TotalSceneScore;
    }
}
