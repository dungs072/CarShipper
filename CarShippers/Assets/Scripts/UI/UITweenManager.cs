using UnityEngine;
public class UITweenManager : MonoBehaviour
{
    [SerializeField] private float popUpDuration = 0.5f;
    [SerializeField] private float closePopUpDuration = 0.2f;
    public float duration = 0.5f;
    public static UITweenManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void CloseSettingPanel()
    {
        LeanTween.scale(UIManager.Instance.SettingPanel, Vector3.zero, closePopUpDuration)
           .setEase(LeanTweenType.easeInBack)
           .setOnComplete(() =>
           {
               UIManager.Instance.SettingPanel.SetActive(false);
               UIManager.Instance.ToggleBlurSettingPanel(false);

           }).setIgnoreTimeScale(true);
    }
    public void SettingPanelAppear()
    {
        UIManager.Instance.SettingPanel.transform.localScale = Vector3.zero;
        LeanTween.scale(UIManager.Instance.SettingPanel, Vector3.one, popUpDuration)
            .setEase(LeanTweenType.easeOutBack)
            .setOnStart(() =>
            {
                UIManager.Instance.SettingPanel.SetActive(true);
                UIManager.Instance.ToggleBlurSettingPanel(true);
            }).setIgnoreTimeScale(true);
    }

    public void ClosePausePanel()
    {

        LeanTween.scale(UIManager.Instance.PausePanel, Vector3.zero, closePopUpDuration)
            .setEase(LeanTweenType.easeInBack)
            .setOnComplete(() =>
            {
                UIManager.Instance.PausePanel.SetActive(false);
                UIManager.Instance.ToggleBlurPanel(false);

            }).setIgnoreTimeScale(true);
    }
    public void PausePanelAppear()
    {
        UIManager.Instance.PausePanel.transform.localScale = Vector3.zero;
        LeanTween.scale(UIManager.Instance.PausePanel, Vector3.one, popUpDuration)
            .setEase(LeanTweenType.easeOutBack)
            .setOnStart(() =>
            {
                UIManager.Instance.PausePanel.SetActive(true);
                UIManager.Instance.ToggleBlurPanel(true);
            }).setIgnoreTimeScale(true);
    }

    public void LosePanelAppear()
    {
        UIManager.Instance.LosePanel.transform.localScale = Vector3.zero;
        LeanTween.scale(UIManager.Instance.LosePanel, Vector3.one, popUpDuration)
            .setEase(LeanTweenType.easeOutBack)
            .setOnStart(() =>
            {
                UIManager.Instance.LosePanel.SetActive(true);
                UIManager.Instance.ToggleBlurPanel(true);
            }).setIgnoreTimeScale(true);
    }
    public void CloseLosePanel()
    {
        LeanTween.scale(UIManager.Instance.LosePanel, Vector3.zero, closePopUpDuration)
            .setEase(LeanTweenType.easeInBack)
            .setOnComplete(() =>
            {
                UIManager.Instance.LosePanel.SetActive(false);
                UIManager.Instance.ToggleBlurPanel(false);
            }).setIgnoreTimeScale(true);
    }

    public void MainMenuPanelAppear()
    {
        LeanTween.reset();
        LeanTween.moveLocal(UIManager.Instance.MainMenuPanel, new Vector3(0, -130, 0), 2f).
                                    setDelay(.1f).setEase(LeanTweenType.easeOutElastic).setOnComplete(
                                        () =>
                                        {
                                            UIManager.Instance.MainMenuPanel.SetActive(true);
                                        }
                                    );
    }
    public void CloseMainMenuPanel()
    {
        LeanTween.moveLocal(UIManager.Instance.MainMenuPanel, new Vector3(1519, -130, 0), 2f).
                                    setDelay(.1f).setEase(LeanTweenType.easeOutElastic).setOnComplete(
                                        () =>
                                        {
                                            UIManager.Instance.MainMenuPanel.SetActive(true);
                                        }
                                    );
    }

    public void ShowNotification()
    {
        LeanTween.reset();
        LeanTween.moveLocal(UIManager.Instance.Notification, new Vector3(0, 263, 0), 2f).
                                    setDelay(.1f).setEase(LeanTweenType.easeOutElastic).setOnComplete(
                                        () =>
                                        {
                                            UIManager.Instance.Notification.SetActive(true);
                                            CloseNotification();
                                        }
                                    );
    }

    public void CloseNotification()
    {
        LeanTween.moveLocal(UIManager.Instance.Notification, new Vector3(-1289, 263, 0), 0.5f).
                                    setDelay(.1f).setEase(LeanTweenType.easeOutElastic).setOnComplete(
                                        () =>
                                        {
                                            UIManager.Instance.Notification.transform.localPosition = new Vector3(1275, 263, 0);
                                            UIManager.Instance.Notification.SetActive(true);
                                        }
                                    );
    }


    public void HighestLevelAppear()
    {
        LeanTween.reset();
        LeanTween.moveLocal(UIManager.Instance.HighestLevelPanel, new Vector3(0, 398, 0), 2f).
                                    setDelay(.1f).setEase(LeanTweenType.easeOutElastic).setOnComplete(
                                        () =>
                                        {
                                            UIManager.Instance.HighestLevelPanel.SetActive(true);
                                        }
                                    );
    }
    public void CloseHighestLevelPanel()
    {
        LeanTween.moveLocal(UIManager.Instance.HighestLevelPanel, new Vector3(0, 737, 0), 2f).
                                    setDelay(.1f).setEase(LeanTweenType.easeOutElastic).setOnComplete(
                                        () =>
                                        {
                                            UIManager.Instance.HighestLevelPanel.SetActive(true);
                                        }
                                    );
    }

    public void CloseDirectionPanel()
    {
        LeanTween.scale(UIManager.Instance.DirectionPanel, Vector3.zero, closePopUpDuration)
           .setEase(LeanTweenType.easeInBack)
           .setOnComplete(() =>
           {
               UIManager.Instance.DirectionPanel.SetActive(false);
               UIManager.Instance.ToggleBlurPanel(false);

           }).setIgnoreTimeScale(true);
    }
    public void DirectionPanelAppear()
    {
        UIManager.Instance.DirectionPanel.transform.localScale = Vector3.zero;
        LeanTween.scale(UIManager.Instance.DirectionPanel, Vector3.one, popUpDuration)
            .setEase(LeanTweenType.easeOutBack)
            .setOnStart(() =>
            {
                UIManager.Instance.DirectionPanel.SetActive(true);
                UIManager.Instance.ToggleBlurPanel(true);
            }).setIgnoreTimeScale(true);
    }

}
