
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Audio source")]
    [SerializeField] private AudioSource introGameSource;
    [SerializeField] private AudioSource playGameSource;
    [SerializeField] private AudioSource mainGameSource;
    [SerializeField] private AudioSource carSource;
    [Header("Audio clips")]
    [SerializeField] private AudioClip buttonClick;
    [SerializeField] private AudioClip coinSound;
    [SerializeField] private AudioClip engineInitSound;
    [SerializeField] private AudioClip finishLevelSound;
    [SerializeField] private AudioClip confirmationSound;
    [SerializeField] private AudioClip loseGameSound;
    [SerializeField] private AudioClip buySound;
    [Header("UI")]
    [SerializeField] private UIMovement uIMovement;
    private void Start() {
        OnIntroGame();
        uIMovement.OnReachDestination+=PlayCoinSound;
        CarController.OnRunFinishPath+=PlayFinishLevelSound;
        CarController.OnCarRunning+=PlayCarRunSound;
        CarController.OnRunFinishPath+=StopCarRunSound;
        PlayerController.OnReachShippingPoint+=PlayConfirmationSound;
        LevelManager.OnLoseGame+=PlayLoseGameSound;
    }
    private void OnDestroy() {
        uIMovement.OnReachDestination-=PlayCoinSound;
        CarController.OnRunFinishPath-=PlayFinishLevelSound;
        CarController.OnCarRunning-=PlayCarRunSound;
        CarController.OnRunFinishPath-=StopCarRunSound;
        PlayerController.OnReachShippingPoint-=PlayConfirmationSound;
        LevelManager.OnLoseGame-=PlayLoseGameSound;
    }
    #region UI
    
    public void OnIntroGame()
    {
        playGameSource.Stop();
        introGameSource.Play();
    }
    public void OnGamePlay()
    {
        introGameSource.Stop();
        playGameSource.Play();
    }
    public void PlayCoinSound()
    {
        mainGameSource.PlayOneShot(coinSound);
    }
    public void PlayEngineInitCarSound()
    {
        carSource.PlayOneShot(engineInitSound);
    }
    public void PlayCarRunSound()
    {
        if(carSource.isPlaying){return;}
        carSource.Play();
    }
    public void StopCarRunSound()
    {
        carSource.Stop();
    }
    public void PlayFinishLevelSound()
    {
        mainGameSource.PlayOneShot(finishLevelSound);
    }
    public void PlayConfirmationSound()
    {
        mainGameSource.PlayOneShot(confirmationSound);
    }
    public void PlayLoseGameSound()
    {
        mainGameSource.PlayOneShot(loseGameSound);
    }

    public void PlayButtonClick()
    {
        mainGameSource.PlayOneShot(buttonClick);
    }
    public void PlayBuySound()
    {
        mainGameSource.PlayOneShot(buySound);
    }
    public void StopInGameSound()
    {
        mainGameSource.Stop();
        carSource.Stop();
    }
    #endregion
    #region Setting
    public void SetSoundVolume(float volume)
    {
        introGameSource.volume = volume;
        playGameSource.volume = volume;
        
    }
    public void SetSoundFXVolume(float volume)
    {
        mainGameSource.volume = volume;
        carSource.volume = volume;
        
    }
    public float GetSoundVolume()
    {
        return introGameSource.volume;
    }

    #endregion
}
