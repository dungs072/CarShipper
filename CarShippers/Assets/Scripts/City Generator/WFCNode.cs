using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WFCNode", menuName = "WFC/Node", order = 0)]
[System.Serializable]
public class WFCNode : ScriptableObject 
{
    [Header("For generator")]
    public string Name;
    public GameObject Prefab;
    public WFC_Connection Top;
    public WFC_Connection Bottom;
    public WFC_Connection Left;
    public WFC_Connection Right;

    public List<GameObject> Prefabs;

    public GameObject GetRandomPrefabs()
    {
        return Prefabs[Random.Range(0,Prefabs.Count)];
    }
}
[System.Serializable]
public class WFC_Connection
{
    public List<WFCNode> CompatibleNodes = new List<WFCNode>();
}

