using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityGenerator : MonoBehaviour
{
    public int mapWidth = 20;
    public int mapHeight = 20;
    public float buildingSpacing = 1.2f;
    public GameObject roadPrefab;
    public GameObject buildingPrefab;

    void Start()
    {
        GenerateCity();
    }

    void GenerateCity()
    {
        // Generate roads
        for (int x = 0; x < mapWidth; x++)
        {
            Instantiate(roadPrefab, new Vector3(x * buildingSpacing, 0, 0), Quaternion.identity);
            Instantiate(roadPrefab, new Vector3(x * buildingSpacing, 0, (mapHeight - 1) * buildingSpacing), Quaternion.identity);
        }

        for (int y = 0; y < mapHeight; y++)
        {
            Instantiate(roadPrefab, new Vector3(0, 0, y * buildingSpacing), Quaternion.Euler(0, 90, 0));
            Instantiate(roadPrefab, new Vector3((mapWidth - 1) * buildingSpacing, 0, y * buildingSpacing), Quaternion.Euler(0, 90, 0));
        }

        // Generate buildings
        for (int x = 1; x < mapWidth - 1; x++)
        {
            for (int y = 1; y < mapHeight - 1; y++)
            {
                if (Random.value > 0.5f)
                {
                    Instantiate(buildingPrefab, new Vector3(x * buildingSpacing, 0, y * buildingSpacing), Quaternion.identity);
                }
            }
        }
    }
}
