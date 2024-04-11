using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIAnimation : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private List<Sprite> sprites;
    [SerializeField] private float timePerImage = 0.02f;

    [SerializeField] private bool isLoop = true;
    private float currentTime;
    private int currentIndex;
    private bool isStopped;
    private void Start() {
        currentTime = 0f;
        isStopped = false;
    }
    private void Update()
    {
        RunAnimation();
    }

    private void RunAnimation()
    {
        if (isStopped) { return; }
        currentTime += Time.deltaTime;
        if (currentTime >= timePerImage)
        {
            if (currentIndex + 1 == sprites.Count)
            {
                if (!isLoop)
                {
                    isStopped = true;
                }
                else
                {
                    isStopped = false;
                }
            }
            currentIndex = (currentIndex + 1) % sprites.Count;
            currentTime = 0f;
            image.sprite = sprites[currentIndex];
        }
    }
}
