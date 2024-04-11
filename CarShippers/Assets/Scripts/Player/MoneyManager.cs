using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] private UIMovement uIMovement;
    [SerializeField] private int maxMoney;
    private int currentMoney;
    public int IncreaseAmount {get;set;} = 1;
    private void Start() {
        currentMoney = 0;
        UIManager.Instance.SetMoney(currentMoney);
        uIMovement.OnReachDestination+=IncreaseMoney;
        LevelManager.OnLoseGame+=Reset;
        LevelManager.OnExitMainGame+=Reset;
    }
    private void OnDestroy() {
        uIMovement.OnReachDestination-=IncreaseMoney;
        LevelManager.OnLoseGame-=Reset;
        LevelManager.OnExitMainGame-=Reset;
    }
    private void Reset()
    {
        currentMoney=0;
        UIManager.Instance.SetMoney(currentMoney);
    }
    public void IncreaseMoney()
    {
        currentMoney+=IncreaseAmount;
        UIManager.Instance.SetMoney(currentMoney);
    }
    public void DecreaseMoney(int value)
    {
        currentMoney-=value;
        UIManager.Instance.SetMoney(currentMoney);
    }
    public int GetCurrentMoney()
    {
        return currentMoney;
    }
}
