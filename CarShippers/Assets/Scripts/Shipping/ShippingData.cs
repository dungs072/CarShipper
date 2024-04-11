using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShippingData : MonoBehaviour
{
    [HideInInspector]
    public PathNode CurrentPathNode;
    public bool IsStartPoint = false;
    public bool IsEndPoint = false;
    [HideInInspector]
    public bool IsTraveled = false;
}
