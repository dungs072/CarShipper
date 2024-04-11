using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public static event Action OnCarRunning;
    public static event Action OnStartRunCar;
    public static event Action OnRunFinishPath;
    [SerializeField] private GameObject carPrefab;
    [SerializeField] private float carSpeed = 10f;
    [SerializeField] private float carRotateSpeed = 10f;
    [SerializeField] private UIMovement uIMovement;
    [SerializeField] private ShippingManager shippingManager;
    [SerializeField] private List<GameObject> boxes;

    private GameObject carInstance = null;
    private List<PathNode> paths;
    private int countNode = 0;
    private int countShippingNode = 0;
    private List<GameObject> boxInstances;
    public bool IsRunningCar{get{return carInstance!=null;}}
    private void Start()
    {
        WFCBuilder.OnGenerateMap += Reset;
        PlayerController.OnFindPath += RunCar;
        LevelManager.OnExitMainGame += Reset;
        boxInstances = new List<GameObject>();
    }
    private void OnDestroy()
    {
        WFCBuilder.OnGenerateMap -= Reset;
        PlayerController.OnFindPath -= RunCar;
        LevelManager.OnExitMainGame -= Reset;
    }
    private void Reset()
    {
        for (int i = 0; i < boxInstances.Count; i++)
        {
            Destroy(boxInstances[i]);
        }
        boxInstances.Clear();
        countNode = 0;
        countShippingNode = 0;
        if (carInstance == null) { return; }
        Destroy(carInstance);
        carInstance = null;


    }
    private void Update()
    {
        if (carInstance == null) { return; }
        if (countNode == paths.Count)
        {
            OnRunFinishPath?.Invoke();
            uIMovement.SpawnUI();
            return;
        }
        if (IsInDestination(carInstance.transform.position, paths[countNode].transform.position))
        {
            if (shippingManager.ShippingSigns[countShippingNode].CurrentPathNode == paths[countNode])
            {
                var boxInstance = Instantiate(boxes[UnityEngine.Random.Range(0, boxes.Count)],
                                    paths[countNode].transform.position, Quaternion.identity);
                boxInstances.Add(boxInstance);
                countShippingNode++;
            }
            
            countNode++;
        }
        else
        {
            Vector3 targetPoint = paths[countNode].transform.position;
            Vector3 direction = targetPoint - carInstance.transform.position;
            direction.y = 0f;
            carInstance.transform.position += direction.normalized * carSpeed * Time.deltaTime;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            carInstance.transform.rotation = Quaternion.Slerp(carInstance.transform.rotation, targetRotation, carRotateSpeed * Time.deltaTime);
            OnCarRunning?.Invoke();

        }

    }
    private void RunCar(List<PathNode> paths)
    {
        if (paths.Count == 0) { return; }
        carInstance = Instantiate(carPrefab, paths[0].transform.position, Quaternion.identity);
        countNode = 1;
        this.paths = paths;
        OnStartRunCar?.Invoke();

    }
    public bool IsInDestination(Vector3 currentPosition, Vector3 targetPosition)
    {
        return (targetPosition - currentPosition).sqrMagnitude <= 4f*4f;
    }


}
