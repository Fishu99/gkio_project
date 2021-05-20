using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatController : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.End))
        {
            GameManager.instance.FinishLevel();
        }
    }
}
