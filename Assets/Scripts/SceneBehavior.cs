using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneBehavior : MonoBehaviour
{
    public void LoadAIGame()
    {
        SceneManager.LoadScene("Main");
    }
}
