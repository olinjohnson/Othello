using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneBehavior : MonoBehaviour
{
    public void LoadAIGame()
    {
        PlayerPrefs.AIOpponent = true;
        SceneManager.LoadScene("Main");
    }
    public void LoadPlayerGame()
    {
        PlayerPrefs.AIOpponent = false;
        SceneManager.LoadScene("Main");
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
