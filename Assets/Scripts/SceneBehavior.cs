using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SceneBehavior : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionsMenu;

    public TextMeshProUGUI captionHead;

    public Slider depthSlider;
    public Toggle statsToggle;

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
            depthSlider.value = PlayerPrefs.searchDepth;
            statsToggle.isOn = PlayerPrefs.showStats;
        }
        else
        {
            UpdatePrefs();
            optionsMenu.SetActive(false);
            mainMenu.SetActive(true);

        }
    }
    public void UpdatePrefs()
    {
        PlayerPrefs.searchDepth = (int)depthSlider.value;
        PlayerPrefs.showStats = statsToggle.isOn;
    }
    public void DepthSliderUpdate()
    {
        captionHead.text = $"AI Search Depth: {depthSlider.value}";
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void OpenRulesWebPage()
    {
        Application.OpenURL("https://www.worldothello.org/about/about-othello/othello-rules/official-rules/english");
    }
}
