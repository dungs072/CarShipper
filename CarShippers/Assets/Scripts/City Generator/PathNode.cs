using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode : MonoBehaviour
{
    [Header("For Path finding")]
    public bool CanGoForward;
    public bool CanGoBackward;
    public bool CanTurnLeft;
    public bool CanTurnRight;

    public LineRenderer ForwardLine;
    public LineRenderer BackwardLine;
    public LineRenderer LeftLine;
    public LineRenderer RightLine;
    [HideInInspector]
    public PathNode Forward;
    [HideInInspector]
    public PathNode Backward;
    [HideInInspector]
    public PathNode Left;
    [HideInInspector]
    public PathNode Right;

    [HideInInspector]
    public bool IsReviewed = false;
    [HideInInspector]
    public int x;
    [HideInInspector]
    public int y;

    [HideInInspector]
    public float fCost;
    [HideInInspector]
    public float gCost;
    [HideInInspector]
    public float hCost;

    [HideInInspector]
    public PathNode parent;

    public void DrawLines()
    {
        if (Forward != null)
        {
            ForwardLine.positionCount = 2;
            ForwardLine.SetPosition(0, transform.position + Vector3.up);
            ForwardLine.SetPosition(1, Forward.transform.position + Vector3.up);
        }
        if (Backward != null)
        {
            BackwardLine.positionCount = 2;
            BackwardLine.SetPosition(0, transform.position + Vector3.up);
            BackwardLine.SetPosition(1, Backward.transform.position + Vector3.up);
        }
        if (Left != null)
        {
            LeftLine.positionCount = 2;
            LeftLine.SetPosition(0, transform.position + Vector3.up);
            LeftLine.SetPosition(1, Left.transform.position + Vector3.up);
        }
        if (Right != null)
        {
            RightLine.positionCount = 2;
            RightLine.SetPosition(0, transform.position + Vector3.up);
            RightLine.SetPosition(1, Right.transform.position + Vector3.up);
        }


    }

    public void DrawPath()
    {
        if (parent == null) { return; }
        ForwardLine.positionCount = 2;
        ForwardLine.SetPosition(0, transform.position + Vector3.up);
        ForwardLine.SetPosition(1, parent.transform.position + Vector3.up);
    }
}
[Serializable]
public class TileNode
{
    public Vector3Int Coordinate;
}
