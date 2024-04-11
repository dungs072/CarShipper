using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LevelManager : MonoBehaviour
{
    public static event Action OnExitMainGame;
    public static event Action OnLoseGame;
    [SerializeField] private float timePerLevel;
    [SerializeField] private WFCBuilder builder;
    [SerializeField] private ShippingManager shippingManager;
    [SerializeField] private UIMovement coinUIMovement;
    [SerializeField] private LightManager lightManager;
    private float initTimePerLevel;
    private int currentLevel = 0;
    private float timeLeft = 0f;
    public bool CanDoAnything{get{return builder.CanDoAnything;}}
    private void Start()
    {
        CarController.OnRunFinishPath += NextLevel;
        PlayerController.OnFindPathWithoutParas += StopTimeCoroutine;
        WFCBuilder.OnFinishedMap += HandleTimer;
    }
    private void OnDestroy()
    {
        CarController.OnRunFinishPath -= NextLevel;
        PlayerController.OnFindPathWithoutParas -= StopTimeCoroutine;
        WFCBuilder.OnFinishedMap -= HandleTimer;
    }
    public void Reset()
    {
        currentLevel = 0;
        initTimePerLevel = timePerLevel;
        builder.SetHeightAndWidth(5);
        shippingManager.SetNummberShippingSign(1);
        timeLeft = initTimePerLevel;
        coinUIMovement.Reset();
        lightManager.Reset();
        NextLevel();
    }
    public void NextLevel()
    {
        currentLevel++;
        if (currentLevel % 5 == 0)
        {
            builder.AddHeightAndWidth(1);
            shippingManager.AddNummberShippingSign(1);
            initTimePerLevel += 2;
            coinUIMovement.AddNumberSpawn(1);
            lightManager.SetContinuous();
        }
        UIManager.Instance.SetLevel(currentLevel);
        builder.OnRegenerateBuilder();
        PlayerData.Instance.SetHighestLevel(currentLevel);

    }
    private void HandleTimer()
    {
        timeLeft = initTimePerLevel;
        StopAllCoroutines();
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        while (timeLeft > 0f)
        {
            timeLeft -= Time.deltaTime;
            UIManager.Instance.SetTime(timeLeft);
            if(initTimePerLevel*0.5>timeLeft)
            {
                UIManager.Instance.ChangeColorOverTime();
            }
            yield return null;
        }
        HandleLoseGame();
    }
    private void HandleLoseGame()
    {
        OnLoseGame?.Invoke();
        UIManager.Instance.SetLoseLevel(currentLevel);
        UITweenManager.Instance.LosePanelAppear();
        
        //UIManager.Instance.ToggleLoseMenu(true);
    }
    private void StopTimeCoroutine()
    {
        StopAllCoroutines();
    }
    public void IncreaseTime(float time)
    {
        timeLeft += time;
        UIManager.Instance.SetTime(timeLeft);
    }

    #region UI
    public void OnPlayAgain()
    {
        Reset();
        UITweenManager.Instance.CloseLosePanel();
        //UIManager.Instance.ToggleLoseMenu(false);
    }
    public void PauseGame()
    {
        Time.timeScale = 0; 
    }
    public void UnPauseGame()
    {
        Time.timeScale = 1; 
    }
    public void ExitMainGame()
    {
        coinUIMovement.Reset();
        Time.timeScale = 1;
        OnExitMainGame?.Invoke();
        StopAllCoroutines();
        
    }
    #endregion

}
