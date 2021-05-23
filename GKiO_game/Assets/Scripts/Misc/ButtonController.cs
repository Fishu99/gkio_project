using UnityEngine;
/// <summary>
/// Class for playing button sounds.
/// </summary>
public class ButtonController : MonoBehaviour
{
    /// <summary>
    /// Plays button click sound.
    /// </summary>
    public void PlayButtonClickSound() {
        AudioManager.instance.Play("ClickButton");
    }
}
