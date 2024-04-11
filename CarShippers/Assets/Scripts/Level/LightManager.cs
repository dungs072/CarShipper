using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    [SerializeField] private Light myLight;
    [SerializeField] private float nightValue;
    [SerializeField] private float morningValue;
    [SerializeField] private float normalValue;
    private int currentIndex = -1;
    public void SetContinuous()
    {
        currentIndex = (currentIndex + 1) % 3;
        if (currentIndex == 0)
        {
            SetNormalLight();
        }
        else if (currentIndex == 1)
        {
            SetNightLight();
        }
        else if (currentIndex == 2)
        {
            SetMorningLight();
        }
    }
    public void Reset()
    {
        myLight.intensity = normalValue;
    }
    public void SetNormalLight()
    {
        //myLight.intensity = normalValue;
        StartCoroutine(ChangeIncreaseLight(normalValue));
    }
    public void SetMorningLight()
    {
        //myLight.intensity = morningValue;
        StartCoroutine(ChangeIncreaseLight(morningValue));
    }
    public void SetNightLight()
    {
        //myLight.intensity = nightValue;
        StartCoroutine(ChangeIncreaseLight(nightValue));
    }
    private IEnumerator ChangeIncreaseLight(float desiredLight)
    {
        if (myLight.intensity - desiredLight > 0)
        {
            while (myLight.intensity - desiredLight > 0)
            {
                myLight.intensity = myLight.intensity-Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            while (myLight.intensity - desiredLight < 0)
            {
                myLight.intensity = myLight.intensity+Time.deltaTime;
                yield return null;
            }
        }


    }
}
