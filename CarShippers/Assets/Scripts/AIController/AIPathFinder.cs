using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPathFinder : MonoBehaviour
{
    [SerializeField] private ShippingManager shippingManager;
    [SerializeField] private AStartPathFinding aStartPathFinding;

    #region UI
    public void HelpFindPath()
    {
        PathNode startNode = shippingManager.StartPoint.CurrentPathNode;
        PathNode endNode = shippingManager.ShippingSigns[0].CurrentPathNode;
        List<PathNode> shortestPaths = aStartPathFinding.FindPath(startNode, endNode);
        foreach(var node in shortestPaths)
        {
            node.DrawPath();
        }
        
    }
    #endregion
}
