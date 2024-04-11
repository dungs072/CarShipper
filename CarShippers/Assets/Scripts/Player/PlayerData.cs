using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            DontDestroyOnLoad(Instance);
        }
    }
    // if not it returns -1
    public int GetHighestLevel()
    {
        if (!PlayerPrefs.HasKey("highest_level"))
        {
            return -1;
        }
        return PlayerPrefs.GetInt("highest_level");
    }
    public void SetHighestLevel(int level)
    {
        if (!PlayerPrefs.HasKey("highest_level"))
        {
            PlayerPrefs.SetInt("highest_level",level);
            UIManager.Instance.SetHighestLevelText(level);
        }
        else
        {
            int currentHighestLevel = PlayerPrefs.GetInt("highest_level");
            if(level>currentHighestLevel)
            {
                PlayerPrefs.SetInt("highest_level",level);
                UIManager.Instance.SetHighestLevelText(level);
            }
        } 
    }
    public void SetSoundVolume(float  soundVolume)
    {
        PlayerPrefs.SetFloat("sound_vol", soundVolume);
    }
    public void SetSFXVolume(float sfxVolume)
    {
        PlayerPrefs.SetFloat("sfx_vol",sfxVolume);
    }
    public float GetSoundVolume()
    {
        if(PlayerPrefs.HasKey("sound_vol"))
        {
            return PlayerPrefs.GetFloat("sound_vol");
        }
        else return -1;
    }
    public float GetSFXVolume()
    {
        if(PlayerPrefs.HasKey("sfx_vol"))
        {
            return PlayerPrefs.GetFloat("sfx_vol");
        }
        else return -1;
    }
}
