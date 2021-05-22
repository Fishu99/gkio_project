using UnityEngine;

/// <summary>
/// Class for counting player's score.
/// </summary>
public class ScoreCounter
{
    private int enemiesKilled = 0;
    private int moneyCollected = 0;
    private float? startTime;
    private float? stopTime;

    /// <summary>
    /// The coefficient which tells how collected money affects the score.
    /// </summary>
    public float MoneyCoefficient { get; set; } = 1f;
    /// <summary>
    /// The coefficient which tells how killed affects the score.
    /// </summary>
    public float EnemyCoefficient { get; set; } = 1f;
    /// <summary>
    /// Difficulty of the game.
    /// </summary>
    public Difficulty CurrentDifficulty { get; set; } = Difficulty.Easy;
    /// <summary>
    /// Information about current level
    /// </summary>
    public SceneData CurrentSceneData { get; set; }

    /// <summary>
    /// The time between StartTimeCount and StopTimeCount calls.
    /// </summary>
    public float TotalSceneTime
    {
        get => (stopTime - startTime).Value;
    }

    /// <summary>
    /// Returns the value of difficulty multiplier for CurrentDifficulty.
    /// </summary>
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

    /// <summary>
    /// Returns the time bonus by which the CurrentSceneScore will be multiplied.
    /// It depends on TotalSceneTime and CurrentSceneData.ExpectedTime.
    /// </summary>
    public float TimeCoefficient
    {
        get
        {
            float expectedTime = CurrentSceneData.ExpectedTime;
            return 1 + Mathf.Pow(2, (expectedTime - TotalSceneTime) / expectedTime);
        }
    }

    /// <summary>
    /// Returns the score to be displayed inside the level.
    /// It includes colleced money and killed enemies.
    /// </summary>
    public int CurrentSceneScore
    {
        get => (int)(enemiesKilled * EnemyCoefficient + moneyCollected * MoneyCoefficient);
    }

    /// <summary>
    /// Returns the final score of current level.
    /// It is equal to CurrentSceneScore multiplied by TimeCoefficient and DifficultyMultiplier.
    /// </summary>
    public int TotalSceneScore
    {
        get => (int)(TimeCoefficient * CurrentSceneScore * DifficultyMultiplier);
        
    }

    /// <summary>
    /// Returns the total game score.
    /// It is a sum of scores from all levels.
    /// </summary>
    public int TotalGameScore { get; private set; } = 0;

    /// <summary>
    /// Starts a new game
    /// </summary>
    public void NewGame()
    {
        TotalGameScore = 0;
    }

    /// <summary>
    /// Starts a new level.
    /// </summary>
    /// <param name="newSceneData">Information about new level.</param>
    public void NewScene(SceneData newSceneData)
    {
        CurrentSceneData = newSceneData;
        Reset();
    }

    /// <summary>
    /// Begins time count for current level.
    /// </summary>
    public void StartTimeCount()
    {
        startTime = Time.time;
    }

    /// <summary>
    /// Stops the time count for current level.
    /// </summary>
    public void StopTimeCount()
    {
        stopTime = Time.time;
    }

    /// <summary>
    /// Adds killed enemy to current score.
    /// </summary>
    /// <param name="enemyValue">The value of the enemy.</param>
    public void AddKilledEnemy(int enemyValue)
    {
        enemiesKilled += enemyValue;
    }

    /// <summary>
    /// Adds collected money to current score.
    /// </summary>
    /// <param name="money">Collected money value.</param>
    public void AddCollectedMoney(int money)
    {
        moneyCollected += money;
    }

    /// <summary>
    /// Adds total scene score to total game score.
    /// </summary>
    public void AddSceneScoreToTotalScore()
    {
        TotalGameScore += TotalSceneScore;
    }

    private void Reset()
    {
        enemiesKilled = 0;
        moneyCollected = 0;
        startTime = null;
        stopTime = null;
    }
}
