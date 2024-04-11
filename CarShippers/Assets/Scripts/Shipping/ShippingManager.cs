using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShippingManager : MonoBehaviour
{
    [Header("Shipping palace")]
    [SerializeField] private ShippingData shippingSignPrefab;
    [SerializeField] private ShippingData startPosPrefab;
    [SerializeField] private int numberShippingSign = 2;
    private List<ShippingData> shippingSigns = new List<ShippingData>();
    private ShippingData startPosInstance;

    // public
    public List<ShippingData> ShippingSigns{get{return shippingSigns;}}
    
    public ShippingData StartPoint{get{return startPosInstance;}}
    private void Start() {
        WFCBuilder.OnGenerateMap+=ClearData;
        WFCBuilder.OnGenerateShipping+=ChooseRandomShippingSign;
    }
    private void OnDestroy() {
        WFCBuilder.OnGenerateMap -= ClearData;
        WFCBuilder.OnGenerateShipping-=ChooseRandomShippingSign;
    }
    public void SetNummberShippingSign(int value)
    {
        numberShippingSign=value;
    }
    public void AddNummberShippingSign(int value)
    {
        numberShippingSign+=value;
    }
    private void ClearData()
    {
        if(startPosInstance!=null)
        {
            Destroy(startPosInstance.gameObject);
        }
        
        foreach (var shippingSign in shippingSigns)
        {
            Destroy(shippingSign.gameObject);
        }
        startPosInstance = null;
        shippingSigns.Clear();
    }

    private void ChooseRandomShippingSign(List<PathNode> possiblePathNodes)
    {
        if(possiblePathNodes.Count == 0){return;}
        startPosInstance = Instantiate(startPosPrefab,possiblePathNodes[0].transform.position+Vector3.up*2f,Quaternion.identity);
        startPosInstance.CurrentPathNode = possiblePathNodes[0];
        possiblePathNodes.RemoveAt(0);
        for(int i = possiblePathNodes.Count-1; i >= 0;i--)
        {
            if(startPosInstance.CurrentPathNode==possiblePathNodes[i])
            {
                possiblePathNodes.RemoveAt(i);
            }
        }
        for (int i = 0; i < numberShippingSign; i++)
        {
            var pathNode = possiblePathNodes[Random.Range(possiblePathNodes.Count / 2, possiblePathNodes.Count)];

            possiblePathNodes.Remove(pathNode);
            var shippingInstance = Instantiate(shippingSignPrefab, pathNode.transform.position+Vector3.up*2f, Quaternion.identity);
            shippingInstance.CurrentPathNode = pathNode;
            shippingSigns.Add(shippingInstance);
        }
    }

}

