using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonManager : Singleton<ButtonManager>
{
    [Header("Panel")]
    [SerializeField] GameObject setting_Panel;
    [SerializeField] GameObject gameplay_Panel;
    [SerializeField] GameObject home_Panel;
    [SerializeField] GameObject end_Panel, win_panel, lose_panel, level_panel;
    [SerializeField] RectTransform panel_InUISetting;

    [Header("Script")]
    [SerializeField] LinesDrawer linesDrawer;
    [SerializeField] UIGamePlay uiGameplay;
    //[SerializeField] AdsManager adsManager;
    [SerializeField] GoogleAdMobManager googleAdMobManager;
    [SerializeField] FirebaseManager firebaseManager;
    [SerializeField] SoundManager soundManager;
    public bool clickSkip, clickHint;
    bool ifClickLevelOnGamePlay;


    #region UISETTING
    [Header("Button")]
    [Header("----- UI SETTING ------")]
    [SerializeField] Button btnIncre_Sound;
    [SerializeField] Button btnDecres_Sound, btnNotifi, btnVibrate, btnCloseSetting;

    [Header("Slider")]
    [Space(10)]
    [SerializeField] Slider slider;

    [Header("ImageOnOff")]
    [Space(10)]
    [SerializeField] Sprite TurnOn;
    [SerializeField] Sprite TurnOff;
    [SerializeField] Image Notifi_Image, Virate_Image;
    bool isVirateOn, isNotifiOn;
    [HideInInspector]public float volumeValue;
    public bool nowChangingVolume;

    #endregion

    #region UIHOME
    [Header("Button")]
    [Header("------ UI HOME ------")]
    [SerializeField] Button btnSetting;
    [SerializeField] Button btnPlay, btnLevel;

    #endregion

    #region UIGAMEPLAY
    [Header("Button")]
    [Header("------ UI GAMEPLAY ------")]
    [SerializeField] Button btnHome;
    [SerializeField] Button btnHint;
    [SerializeField] Button btnLevel2;
    bool hasClickNextLevel;

    #endregion

    #region UICOMPLETE
    [Header("Button")]
    [Header("------ UI COMPLETE ------")]
    [SerializeField] Button btnHome2;
    [SerializeField] Button btnHome3, btnSkip, btnNextLV;
    [SerializeField] List<Button> btnReplay;
    public int indexOfLoseImg; //chinh? anh? ben UI Complete

    #endregion

    #region UILEVEL
    [Header("Button")]
    [Header("------ UI LEVEL ------")]
    [SerializeField] Button btnBackFromLevel;
    #endregion


    // Start is called before the first frame update


    void Start()
    {
        #region UISETTING
        ifClickLevelOnGamePlay = nowChangingVolume = hasClickNextLevel = false;
        isVirateOn = isNotifiOn = true;

        btnIncre_Sound.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Click);
            slider.value += 0.1f;
            volumeValue = slider.value;
        });

        btnDecres_Sound.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Click);
            slider.value -= 0.1f;
            volumeValue = slider.value;
        });

        btnNotifi.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Click);
            isNotifiOn = !isNotifiOn;
            if (isNotifiOn)
            {
                Notifi_Image.sprite = TurnOn;
                PlayerData.TurnOnNotifi(true);
            }
            else
            {
                Notifi_Image.sprite = TurnOff;
                PlayerData.TurnOnNotifi(false);
            }
        });

        btnVibrate.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Click);
            isVirateOn = !isVirateOn;
            if (isVirateOn)
            {
                Handheld.Vibrate();
                Virate_Image.sprite = TurnOn;
                PlayerData.TurnOnVibration(true);
            }
            else
            {
                Virate_Image.sprite = TurnOff;
                PlayerData.TurnOnVibration(false);
            }
        });

        slider.onValueChanged.AddListener(delegate { SetVolumeValue(); });

        btnCloseSetting.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Click);
            setting_Panel.SetActive(false);
            panel_InUISetting.localScale = Vector3.zero;
        });

        slider.value = PlayerData.GetVolumeValue();
        if (PlayerData.GetVibrationState())
        {
            Virate_Image.sprite = TurnOn;
        }
        else
        {
            Virate_Image.sprite = TurnOff;
        }
        if (PlayerData.GetNotifiState())
        {
            Notifi_Image.sprite = TurnOn;
        }
        else
        {
            Notifi_Image.sprite = TurnOff;
        }

        #endregion

        #region UIHOME

        btnSetting.onClick.AddListener(() =>
        {
            btnSetting.transform.DOScale(new Vector3(0.9f,0.9f), .3f).SetEase(Ease.InOutSine).SetLoops(2, LoopType.Yoyo);
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Click);
            setting_Panel.SetActive(true);
        });

        btnPlay.onClick.AddListener(() =>
        {
            if(firebaseManager.isShowInterAdsFromGameplay)
            {
                //adsManager.ShowInterstitialFromGameplay();
                googleAdMobManager.ShowFullscreenNextLevelInterstitialAd();
            }
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Click);
            if (PlayerData.GetCurrentLevelPlay() == uiGameplay.levels.Count)
            {
                uiGameplay.SetLevel(PlayerData.GetCurrentLevelPlay() - 1);
            }
            else 
                uiGameplay.SetLevel(PlayerData.GetCurrentLevelPlay());
            soundManager.PlaySound(SoundManager.SoundType.BGFarm);
            soundManager.StopSpecificSound(SoundManager.SoundType.BGMain);
            home_Panel.SetActive(false);
            gameplay_Panel.gameObject.SetActive(true);
        });



        btnLevel.onClick.AddListener(() =>
        {
            if (firebaseManager.isShowInterAdsFromHome)
            {
                //adsManager.ShowInterstitialFromHome();
                googleAdMobManager.ShowFullscreenNextLevelInterstitialAd();
            }

            SoundManager.Instance.PlaySound(SoundManager.SoundType.Click);
            SoundManager.Instance.StopAllSoundExceptBG();
            level_panel.SetActive(true);
            home_Panel.SetActive(false);
        });
        #endregion

        #region UIGAMEPLAY

        btnHome.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Click);
            ReturnToUIHome();
        });

        btnHint.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Click);
            googleAdMobManager.ShowRewardedAd();
            //adsManager.ShowRewardedAd();
            clickHint = true;
        });

        btnLevel2.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Click);
            SoundManager.Instance.StopAllSoundExceptBG();
            ifClickLevelOnGamePlay = true;
            uiGameplay.gameObject.SetActive(false);
            level_panel.gameObject.SetActive(true);
        });

        #endregion

        #region UICOMPLETE

        btnHome3.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Click);
            end_Panel.SetActive(false);
            ReturnToUIHome();
        });

        btnHome2.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Click);
            end_Panel.SetActive(false);
            ReturnToUIHome();
        });

        foreach(var btn in btnReplay)
        {
            btn.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlaySound(SoundManager.SoundType.Click);
                uiGameplay.ResetLevel();
                UIComplete.Instance.PanelFadeOut();
            });
        }

        btnNextLV.onClick.AddListener(() =>
        {
            if (!hasClickNextLevel)
            {
                hasClickNextLevel = true;
                Invoke("DelayButton", 0.5f);

                SoundManager.Instance.PlaySound(SoundManager.SoundType.Click);
                if (uiGameplay.GetLevelSelecting() + 1 >= uiGameplay.levels.Count)
                {
                    ReturnToUIHome();
                    return;
                }
                UIComplete.Instance.PanelFadeOut();
                uiGameplay.SetLevel(uiGameplay.GetLevelSelecting() + 1);



            }
        });

        btnSkip.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Click);
            googleAdMobManager.ShowRewardedAd();
            //adsManager.ShowRewardedAd();
            clickSkip = true;
        });
        #endregion

        #region UILEVEL
        btnBackFromLevel.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Click);
            if(ifClickLevelOnGamePlay)
            {
                uiGameplay.SetLevel(uiGameplay.GetLevelSelecting());
                gameplay_Panel.gameObject.SetActive(true);
            }
            else
            {
                ReturnToUIHome();
            }
            level_panel.SetActive(false);
            ifClickLevelOnGamePlay = false;
        });
        #endregion
    }

    private void ReturnToUIHome()
    {
        gameplay_Panel.SetActive(false);
        home_Panel.SetActive(true);
        end_Panel.SetActive(false);
        soundManager.StopAllSoundExceptBG();
        soundManager.StopSpecificSound(SoundManager.SoundType.BGFarm);
        soundManager.PlaySound(SoundManager.SoundType.BGMain);
        linesDrawer.ResetLevel();
    }

    void SetVolumeValue()
    {
        volumeValue = slider.value;
        PlayerData.SetVolumeValue(volumeValue);
        soundManager.ChangingAudioVolume(volumeValue);
        nowChangingVolume = true;
    }

    public void TurnOnPanelComplete(bool TurnOnLose, bool TurnOnWin, int indexOfImg)
    {
        indexOfLoseImg = indexOfImg;
        lose_panel.SetActive(TurnOnLose);
        win_panel.SetActive(TurnOnWin);
        end_Panel.SetActive(true);
    }

    void DelayButton()
    {
        hasClickNextLevel = false;
    }

    private void Update()
    {
        if (googleAdMobManager.adsHint)
        {
            googleAdMobManager.adsSkip = googleAdMobManager.adsHint = false;
            uiGameplay.ClickHint();
        }
        if (googleAdMobManager.adsSkip)
        {
            googleAdMobManager.adsSkip = googleAdMobManager.adsHint = false;
            if (uiGameplay.GetLevelSelecting() + 1 >= uiGameplay.levels.Count)
            {
                PlayerData.AddLevelSkip(uiGameplay.GetLevelSelecting() + 1);
                ReturnToUIHome();
                return;
            }
            UIComplete.Instance.PanelFadeOut();
            PlayerData.AddLevelSkip(uiGameplay.GetLevelSelecting() + 1);
            uiGameplay.SetLevel(uiGameplay.GetLevelSelecting() + 1);

            if (!PlayerData.checkIfContainsSkipLevel(uiGameplay.GetLevelSelecting() - 1))
            {
                firebaseManager.SkipLevel(uiGameplay.GetLevelSelecting());
            }
        }
    }
}
