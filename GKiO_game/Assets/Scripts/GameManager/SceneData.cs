/// <summary>
/// A class for storing information about each level of the game.
/// </summary>
public class SceneData
{
    /// <summary>
    /// Name of the Unity scene with the level.
    /// </summary>
    public string SceneName { get; set; }
    /// <summary>
    /// Name of the level.
    /// </summary>
    public string LevelName { get; set; }
    /// <summary>
    /// The expected time in which player is supposed to pass the level.
    /// </summary>
    public float ExpectedTime { get; set; }
    /// <summary>
    /// Name of he music to be played in this level.
    /// </summary>
    public string MusicName { get; set; }
}
