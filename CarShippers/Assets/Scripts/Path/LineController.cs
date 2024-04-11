using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private GameObject carPrefab;
    private List<Transform> points;
    private GameObject carInstance;
    private Transform previousPoint;
    private void Start() {
        points = new List<Transform>(); 
    }
    public void Reset()
    {
        points.Clear();
        lineRenderer.positionCount = 0;
        previousPoint = null;
        Destroy(carInstance);
    }
    public void ToggleTempCar(bool state)
    {
        carInstance.SetActive(state);
    }

    public void AddPoint(Transform point)
    {
        if(previousPoint==null)
        {
            carInstance = Instantiate(carPrefab,point.position,Quaternion.identity);
        }
        else
        {
            carInstance.transform.position = point.position;
            Vector3 direction = (point.position-previousPoint.position).normalized;
            carInstance.transform.rotation = Quaternion.LookRotation(direction);
        }
        previousPoint = point;
        points.Add(point);
        
        lineRenderer.positionCount++;
        for (int i = 0;i<lineRenderer.positionCount;i++)
        {
            lineRenderer.SetPosition(i, points[i].position+Vector3.up*1.1f);
        }
    }
    public void RemoveTheLastPoint()
    {
        if(points.Count==0){return;}
        Vector3 temp = points[points.Count-1].position;
        points.RemoveAt(points.Count-1);
        lineRenderer.positionCount--;
        if(points.Count==0)
        {
            previousPoint = null;
            Destroy(carInstance);
        }
        else
        {
            previousPoint = points[points.Count-1];
            Vector3 direction = (temp-previousPoint.position).normalized;
            carInstance.transform.rotation = Quaternion.LookRotation(direction);
            carInstance.transform.position = points[points.Count-1].position;
        }
    }
}
