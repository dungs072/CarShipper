using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    public static event Action OnReachShippingPoint;
    public static event Action<List<PathNode>> OnFindPath;
    public static event Action OnFindPathWithoutParas;
    [Header("Raycast")]
    [SerializeField] private LayerMask layerMask;
    [Header("Lines")]
    [SerializeField] private LineController lineController;
    [Header("Shipping")]
    [SerializeField] private ShippingManager shippingManager;
    [Header("Car")]
    [SerializeField] private CarController carController;
    private Controls controls;
    private PathNode previousPathNode = null;
    private List<PathNode> paths = new List<PathNode>();
    private List<ShippingData> shippingDatas = new List<ShippingData>();
    private int countShipping = 0;
    private bool finishCurrentLevel = false;
    private void Start()
    {
        controls = new Controls();
        controls.Player.ChooseNode.performed += ChooseNode;
        controls.Player.ChooseNode.canceled += UnChooseNode;
        controls.Enable();

        WFCBuilder.OnGenerateMap += ClearData;
        LevelManager.OnLoseGame+=FinishCurrentLevel;
    }
    private void OnDestroy()
    {
        WFCBuilder.OnGenerateMap -= ClearData;
        LevelManager.OnLoseGame -= FinishCurrentLevel;
    }

    private void ClearData()
    {
        lineController.Reset();
        paths.Clear();
        shippingDatas.Clear();
        countShipping = 0;
        finishCurrentLevel = false;
    }
    private void FinishCurrentLevel()
    {
        finishCurrentLevel = true;
    }

    private void FixedUpdate()
    {
        if (controls.Player.ChooseNode.IsPressed())
        {
            if (finishCurrentLevel) { return; }
            Vector2 mousePosition = controls.Player.ChooseNode.ReadValue<Vector2>();
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, layerMask))
            {
                if (hit.collider.TryGetComponent<ShippingData>(out ShippingData shippingData))
                {
                    if (previousPathNode == null)
                    {
                        if (shippingData.IsStartPoint)
                        {
                            previousPathNode = shippingData.CurrentPathNode;
                            lineController.AddPoint(shippingData.CurrentPathNode.transform);
                            paths.Add(shippingData.CurrentPathNode);
                            shippingDatas.Add(shippingData);
                        }
                        return;

                    }
                    //if (previousPathNode == shippingData.CurrentPathNode) { return; }
                    if (previousPathNode.Forward == shippingData.CurrentPathNode)
                    {
                        lineController.AddPoint(shippingData.CurrentPathNode.transform);
                        previousPathNode = shippingData.CurrentPathNode;
                        paths.Add(shippingData.CurrentPathNode);
                        shippingDatas.Add(shippingData);
                        if (shippingData.IsEndPoint)
                        {
                            HandleReachTheShippingPoint(shippingData);
                        }

                    }
                    else if (previousPathNode.Backward == shippingData.CurrentPathNode)
                    {
                        lineController.AddPoint(shippingData.CurrentPathNode.transform);
                        previousPathNode = shippingData.CurrentPathNode;
                        paths.Add(shippingData.CurrentPathNode);
                        shippingDatas.Add(shippingData);
                        if (shippingData.IsEndPoint)
                        {
                            HandleReachTheShippingPoint(shippingData);
                        }

                    }
                    else if (previousPathNode.Right == shippingData.CurrentPathNode)
                    {
                        lineController.AddPoint(shippingData.CurrentPathNode.transform);
                        previousPathNode = shippingData.CurrentPathNode;
                        paths.Add(shippingData.CurrentPathNode);
                        shippingDatas.Add(shippingData);
                        if (shippingData.IsEndPoint)
                        {
                            HandleReachTheShippingPoint(shippingData);
                        }

                    }
                    else if (previousPathNode.Left == shippingData.CurrentPathNode)
                    {
                        lineController.AddPoint(shippingData.CurrentPathNode.transform);
                        previousPathNode = shippingData.CurrentPathNode;
                        paths.Add(shippingData.CurrentPathNode);
                        shippingDatas.Add(shippingData);
                        if (shippingData.IsEndPoint)
                        {
                            HandleReachTheShippingPoint(shippingData);
                        }

                    }


                }
                else if (hit.collider.TryGetComponent<PathNode>(out PathNode pathNode))
                {
                    if (previousPathNode == pathNode) { return; }
                    if (previousPathNode == null)
                    {

                        // previousPathNode = pathNode;
                        // lineController.AddPoint(pathNode.transform);
                    }
                    else
                    {

                        if (previousPathNode.Forward == pathNode)
                        {
                            lineController.AddPoint(pathNode.transform);
                            previousPathNode = pathNode;
                            paths.Add(pathNode);
                        }
                        else if (previousPathNode.Backward == pathNode)
                        {
                            lineController.AddPoint(pathNode.transform);
                            previousPathNode = pathNode;
                            paths.Add(pathNode);
                        }
                        else if (previousPathNode.Right == pathNode)
                        {
                            lineController.AddPoint(pathNode.transform);
                            previousPathNode = pathNode;
                            paths.Add(pathNode);
                        }
                        else if (previousPathNode.Left == pathNode)
                        {
                            lineController.AddPoint(pathNode.transform);
                            previousPathNode = pathNode;
                            paths.Add(pathNode);
                        }

                    }

                    // print(pathNode.x + ":" + pathNode.y);
                }

            }

        }

    }
    private void HandleReachTheShippingPoint(ShippingData shippingData)
    {
        if (shippingData.IsTraveled) { return; }
        shippingData.IsTraveled = true;
        countShipping++;
        shippingData.gameObject.SetActive(false);
        OnReachShippingPoint?.Invoke();
        if (countShipping == shippingManager.ShippingSigns.Count)
        {
            finishCurrentLevel = true;
            OnFindPath?.Invoke(paths);
            OnFindPathWithoutParas?.Invoke();
            lineController.ToggleTempCar(false);
        }
    }
    public void MoveOnToPath(List<PathNode> newPaths, ShippingData shippingData)
    {
        foreach (var pathNode in newPaths)
        {
            lineController.AddPoint(pathNode.transform);
            previousPathNode = pathNode;
            paths.Add(pathNode);
        }
        
        if (shippingData.IsEndPoint)
        {
            shippingDatas.Add(shippingData);
            HandleReachTheShippingPoint(shippingData);
        }
    }
    #region Input
    private void ChooseNode(InputAction.CallbackContext ctx)
    {

    }
    private void UnChooseNode(InputAction.CallbackContext ctx)
    {

    }
    #endregion

    #region UI
    public void RemoveTheLastPoint()
    {
        if(carController.IsRunningCar){return;}
        if (previousPathNode == null) { return; }
        if (paths.Count == 0) { return; }

        if (shippingDatas[shippingDatas.Count - 1].CurrentPathNode == paths[paths.Count - 1]&&shippingDatas[shippingDatas.Count-1].IsEndPoint)
        {
            shippingDatas[shippingDatas.Count - 1].IsTraveled = false;
            shippingDatas[shippingDatas.Count-1].gameObject.SetActive(true);
            shippingDatas.RemoveAt(shippingDatas.Count - 1);
            countShipping--;
            
        }
        paths.RemoveAt(paths.Count - 1);
        if (paths.Count == 0)
        {
            previousPathNode = null;
        }
        else
        {
            previousPathNode = paths[paths.Count - 1];
        }
        lineController.RemoveTheLastPoint();
    }
    public void RemoveToTheShippingPoint()
    {
        if (shippingDatas.Count == 0) { return; }
        int count = paths.Count;
        for (int i = paths.Count - 1; i >= 0; i--)
        {
            // if (!RemoveTheLastPoint())
            // {
            //     break;
            // }
        }
    }
    #endregion
    public PathNode GetCurrentPathNode()
    {
        if (paths.Count == 0)
        {
            return null;
        }
        else
        {
            return paths[paths.Count-1];
        }
    }
}
