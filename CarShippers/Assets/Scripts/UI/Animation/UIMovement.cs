using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class UIMovement : MonoBehaviour
{
    public event Action OnReachDestination;
    [SerializeField] private RectTransform startRectTransform;
    [SerializeField] private RectTransform endRectTransform;
    [SerializeField] private Image imagePrefab;
    [SerializeField] private int numberSpawn;
    [SerializeField] private float movementSpeed = 10f;
    private List<Image> images = new List<Image>();
    private int initNumberSpawn;
    private void Start() {
        initNumberSpawn = numberSpawn;
    }
    
    private void Update()
    {
        if (images.Count == 0) { return; }
        for (int i = images.Count - 1; i >= 0; i--)
        {
            Vector3 direction = (endRectTransform.position - images[i].rectTransform.position).normalized;
            float distance = Vector3.Distance(images[i].rectTransform.position, endRectTransform.position);
            images[i].rectTransform.position += direction * movementSpeed * Time.deltaTime;
            if (distance <= 8f)
            {
                images[i].rectTransform.anchoredPosition = endRectTransform.anchoredPosition;
                Destroy(images[i].gameObject);
                images.RemoveAt(i);
                OnReachDestination?.Invoke();
            }
        }

    }
    public void Reset()
    {
        foreach (var image in images)
        {
            Destroy(image.gameObject);
        }
        images.Clear();
        initNumberSpawn = numberSpawn;
    }
    public void SetNumberSpawn(int value)
    {
        initNumberSpawn = value;
    }
    public void AddNumberSpawn(int value)
    {
        initNumberSpawn+=value;
    }
    public void SpawnUI()
    {
        StartCoroutine(SpawnUICoroutine());
    }
    private IEnumerator SpawnUICoroutine()
    {
        for (int i = 0; i < initNumberSpawn; i++)
        {
            var instance = Instantiate(imagePrefab, startRectTransform);
            images.Add(instance);
            yield return new WaitForSeconds(0.2f);
        }
    }


}
