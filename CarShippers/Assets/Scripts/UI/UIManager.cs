using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    [Header("Lose Menu")]
    [SerializeField] private GameObject losePanel;
    [SerializeField] private TMP_Text loseLevelText;
    [Header("Top Display")]
    [SerializeField] private TMP_Text timeText; 
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text moneyText;
    [Header("Blur Panel")]
    [SerializeField] private GameObject blurPanel;
    [Header("Pause")]
    [SerializeField] private GameObject pausePanel;
    [Header("MainMenu")]
    [SerializeField] private GameObject mainMenuPanel;
    [Header("Setting")]
    [SerializeField] private GameObject blurSettingPanel;
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Slider sfxSlider;
    [Header("Notification")]
    [SerializeField] private GameObject notification;
    [Header("Highest level")]
    [SerializeField] private GameObject highestLevelPanel;
    [SerializeField] private TMP_Text highestLevelText;
    [Header("Direction")]
    [SerializeField] private GameObject directionPanel;
    [Header("Helper")]
    [SerializeField] private TMP_Text findPathCostText;
    [SerializeField] private TMP_Text increaseTimeCostText;
    public GameObject PausePanel{get{return pausePanel;}}
    public GameObject LosePanel{get{return losePanel;}} 
    public GameObject MainMenuPanel{get{return mainMenuPanel;}} 
    public GameObject SettingPanel{get{return settingPanel;}}
    public GameObject Notification{get{return notification;}}
    public GameObject HighestLevelPanel{get{return highestLevelPanel;}}
    public GameObject DirectionPanel{get{return directionPanel;}}   

    public static UIManager Instance{get;private set;}

    private bool doesFinishChangeColor = true;
    private void Awake() {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void ToggleBlurPanel(bool state)
    {
        blurPanel.SetActive(state);
    }
    #region TopDisplay
    public void SetTime(float time)
    {
        timeText.text = time.ToString("0")+"s";
    }
    public void ChangeColorOverTime()
    {
        if(doesFinishChangeColor)
        {
            StartCoroutine(ColorTime());
        }
        
    }
    private IEnumerator ColorTime()
    {
        doesFinishChangeColor = false;
        yield return new WaitForSeconds(0.2f);
        timeText.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        timeText.color = Color.white;
        doesFinishChangeColor = true;
    }

    public void SetLevel(int level)
    {
        levelText.text = "Level "+level.ToString();
    }
    public void SetMoney(int money)
    {
        moneyText.text = money.ToString();  
    }
    #endregion
    
    #region PauseMenu
    public void ToggleLoseMenu(bool state)
    {
        losePanel.SetActive(state);
    }
    public void SetLoseLevel(int level)
    {
        loseLevelText.text = "Your current level "+ level.ToString();
    }
    #endregion
    #region Setting
    public void SetSoundSlider(float value)
    {
        soundSlider.value = value;
    }
    public void SetSoundFXSlider(float value)
    {
        sfxSlider.value = value;
    }
    public float GetSoundSliderValue()
    {
        return soundSlider.value;
    }
    public float GetSoundFXValue()
    {
        return sfxSlider.value;
    }
    public void ToggleBlurSettingPanel(bool state)
    {
        blurSettingPanel.SetActive(state);
    }
    #endregion

    #region Highest Level
    public void SetHighestLevelText(int level)
    {
        highestLevelText.text = "Your highest level: "+level.ToString();  
    }
    
    #endregion

    #region Helper
    public void SetFindPathCost(int cost)
    {
        findPathCostText.text = "-"+cost.ToString();
    }
    public void SetIncreaseTimeCost(int cost)
    {
        increaseTimeCostText.text = "-"+cost.ToString();
    }
    #endregion

}
