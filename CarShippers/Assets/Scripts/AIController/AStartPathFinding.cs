using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStartPathFinding : MonoBehaviour
{
    public List<PathNode> FindPath(PathNode start, PathNode end)
    {
        List<PathNode> openSet = new List<PathNode>();
        HashSet<PathNode> closedSet = new HashSet<PathNode>();

        openSet.Add(start);

        while (openSet.Count > 0)
        {
            PathNode current = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < current.fCost || openSet[i].fCost == current.fCost && openSet[i].hCost < current.hCost)
                {
                    current = openSet[i];
                }
            }

            openSet.Remove(current);
            closedSet.Add(current);

            if (current == end)
            {
                return RetracePath(start, end);
            }
            if (current.Forward != null)
            {
                SeekNeighbor(current.Forward,current,end,openSet,closedSet);
            }
            if(current.Backward != null)
            {
                SeekNeighbor(current.Backward,current,end,openSet,closedSet);
            }
            if(current.Left != null)
            {
                SeekNeighbor(current.Left,current,end,openSet,closedSet);
            }
            if(current.Right!=null)
            {
                SeekNeighbor(current.Right,current,end,openSet,closedSet);
            }
        }

        return null;
    }
    private void SeekNeighbor(PathNode neighbor,PathNode current,PathNode end, List<PathNode> openSet, HashSet<PathNode> closedSet)
    {
        if (!closedSet.Contains(neighbor))
        {
            float newCostToNeighbor = current.gCost + GetDistance(current, neighbor);
            if (newCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
            {
                neighbor.gCost = newCostToNeighbor;
                neighbor.hCost = GetDistance(neighbor, end);
                neighbor.parent = current;

                if (!openSet.Contains(neighbor))
                {
                    openSet.Add(neighbor);
                }
            }
        }
    }

    private List<PathNode> RetracePath(PathNode startPathNode, PathNode endPathNode)
    {
        List<PathNode> path = new List<PathNode>();
        PathNode currentPathNode = endPathNode;

        while (currentPathNode != startPathNode)
        {
            path.Add(currentPathNode);
            currentPathNode = currentPathNode.parent;
        }

        path.Reverse();
        return path;
    }

    private float GetDistance(PathNode PathNodeA, PathNode PathNodeB)
    {
        return Mathf.Abs(PathNodeA.transform.position.x - PathNodeB.transform.position.x) + Mathf.Abs(PathNodeA.transform.position.z - PathNodeB.transform.position.z);
    }
}

