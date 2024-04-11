using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] private float soundVolume = 1f;
    [SerializeField] private float sfxVolume = 1f;
    [SerializeField] private SoundManager soundManager;
    private void Start() {
        
        var soundVol = PlayerData.Instance.GetSoundVolume();
        var sfxVol = PlayerData.Instance.GetSFXVolume();
        if(soundVol<0)
        {
            UIManager.Instance.SetSoundSlider(soundVolume);
        }
        else
        {
            UIManager.Instance.SetSoundSlider(soundVol);
        }
        if(sfxVol<0)
        {
            UIManager.Instance.SetSoundFXSlider(sfxVolume);
        }
        else
        {
            UIManager.Instance.SetSoundFXSlider(sfxVol);
        }
        
    }

    public void SetSound()
    {
        soundManager.SetSoundVolume(UIManager.Instance.GetSoundSliderValue());
        PlayerData.Instance.SetSoundVolume(UIManager.Instance.GetSoundSliderValue());
    }
    public void SetSFX()
    {
        soundManager.SetSoundFXVolume(UIManager.Instance.GetSoundFXValue());
        PlayerData.Instance.SetSFXVolume(UIManager.Instance.GetSoundFXValue());
    }
}
