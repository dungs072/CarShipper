using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    private void OnEnable()
    {

        Invoke(nameof(ShowMainMenu), .2f);
        Invoke(nameof(InitialGameData),2f);
    }
    private void InitialGameData()
    {
        int highestLevel = PlayerData.Instance.GetHighestLevel();
        if (highestLevel == -1)
        {
            UITweenManager.Instance.DirectionPanelAppear();
            levelManager.PauseGame();
        }
        else
        {
            PlayerData.Instance.SetHighestLevel(highestLevel);
            UITweenManager.Instance.HighestLevelAppear();
            UIManager.Instance.SetHighestLevelText(highestLevel);
        }
    }
    private void ShowMainMenu()
    {
        UITweenManager.Instance.MainMenuPanelAppear();
    }
    public void ExitGame()
    {
        Application.Quit();
    }

}
