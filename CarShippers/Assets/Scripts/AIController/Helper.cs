using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper : MonoBehaviour
{
    [SerializeField] private MoneyManager moneyManager;
    [SerializeField] private ShippingManager shippingManager;
    [SerializeField] private AStartPathFinding aStartPathFinding;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private UITweenManager uITweenManager;
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private CarController carController;
    
    [Header("Increase Time")]
    [SerializeField] private float increaseTime = 5f;
    [SerializeField] private int increaseTimeCost = 75;
    [Header("Find path")]
    [SerializeField] private int findPathCost = 100;
    
    private int currentIncreaseTimeCost = 0;
    private int currentFindPathCost = 0;   
    private void Start() {
        currentIncreaseTimeCost = increaseTimeCost;
        currentFindPathCost = findPathCost;
        UIManager.Instance.SetFindPathCost(currentFindPathCost);
        UIManager.Instance.SetIncreaseTimeCost(currentIncreaseTimeCost);
    }
    private void Reset()
    {

    }
    #region UI
    public void HelpFindPath()
    {
        if(carController.IsRunningCar){return;}
        if(!levelManager.CanDoAnything){return;}
        if (shippingManager.StartPoint == null) { return; }
        int currentMoney = moneyManager.GetCurrentMoney();
        if (currentMoney >= currentFindPathCost)
        {
            
            soundManager.PlayBuySound();
            moneyManager.DecreaseMoney(currentFindPathCost);
            PathNode startNode = playerController.GetCurrentPathNode();
            if (startNode == null)
            {

                startNode = shippingManager.StartPoint.CurrentPathNode;
            }
            ShippingData desShippingData = null;
            foreach (var shippingData in shippingManager.ShippingSigns)
            {
                if (!shippingData.IsTraveled)
                {
                    desShippingData = shippingData;
                    break;
                }
            }
            currentFindPathCost+=2;
            UIManager.Instance.SetFindPathCost(currentFindPathCost);
            if (desShippingData == null) { return; }
            List<PathNode> shortestPaths = aStartPathFinding.FindPath(startNode, desShippingData.CurrentPathNode);
            playerController.MoveOnToPath(shortestPaths, desShippingData);

        }
        else
        {
            uITweenManager.ShowNotification();
        }

    }

    public void HelpIncreaseTime()
    {
        if(carController.IsRunningCar){return;}
        if(!levelManager.CanDoAnything){return;}
        int currentMoney = moneyManager.GetCurrentMoney();
        if (currentMoney >= currentIncreaseTimeCost)
        {
            soundManager.PlayBuySound();
            moneyManager.DecreaseMoney(currentIncreaseTimeCost);
            levelManager.IncreaseTime(increaseTime);
            currentIncreaseTimeCost+=2;
            UIManager.Instance.SetIncreaseTimeCost(currentIncreaseTimeCost);

        }
        else
        {
            uITweenManager.ShowNotification();
        }
    }
    #endregion
}
