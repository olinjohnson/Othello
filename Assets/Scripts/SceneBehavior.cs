using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneBehavior : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionsMenu;

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
    public void ToggleMainMenu()
    {
        if(mainMenu.activeInHierarchy)
        {
            mainMenu.SetActive(false);
            optionsMenu.SetActive(true);
        }
        else
        {
            optionsMenu.SetActive(false);
            mainMenu.SetActive(true);
        }
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
