using UnityEngine;

/// <summary>
/// A script allowing cheating for testing purposes.
/// It allows to finish level by pressing End key.
/// </summary>
public class CheatController : MonoBehaviour
{

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.End))
        {
            GameManager.instance.FinishLevel();
        }
    }
}
