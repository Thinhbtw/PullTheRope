using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGamePlay : Singleton<UIGamePlay>
{
    public List<GameObject> levels;
    [SerializeField] List<Sprite> sprites_Background;
    [SerializeField] List<string> textTutorial;
    [Space(20)]
    public Transform levelSpawn;
    public Text txtLevel, txtTutorial;
    [SerializeField] GameObject txtTutorialGameObject;
    public LineRenderer lineRenderer;
    [SerializeField] Image image_Background;
    myLevel myLevel;
    int myCurLv;
    [Space(10f)]
    [Header("Script")]
    public CountDown timeCount;
    [SerializeField] LinesDrawer linesDrawer;
    [SerializeField] FirebaseManager firebaseManager;
    [SerializeField] AdsManager adsManager;
    [SerializeField] GoogleAdMobManager googleAdMobManager;
    [SerializeField] SoundManager soundManager;

    bool hasWatchIntersAds;

    private void Awake()
    {
        levels = new List<GameObject>(Resources.LoadAll<GameObject>("Level"));
        myCurLv = PlayerData.GetCurrentLevelPlay();
        hasWatchIntersAds = false;
        this.gameObject.SetActive(false);
    }

    public void SetLevel(int numlevel)
    {
        txtLevel.text = $"Level {numlevel + 1}";
        myCurLv = numlevel;
        linesDrawer.ResetLevel();
        if (levelSpawn.childCount > 0)
            Destroy(levelSpawn.GetChild(0).gameObject);
        var temp = Instantiate(levels[numlevel], levelSpawn);
        myLevel = temp.GetComponent<myLevel>();
        image_Background.sprite = sprites_Background[numlevel];

        txtTutorialGameObject.SetActive(true);

        txtTutorial.text = textTutorial[numlevel];
        timeCount.ResetTime();
        lineRenderer.enabled = true;

        soundManager.StopAllSoundExceptBG();
        ChangeSound(numlevel);

        if (!adsManager.hasInternet) return;

        if (firebaseManager.listLevelShowInterAds.Contains(numlevel))
        {
            //AdsManager.Instance.ShowInterstitial();
            googleAdMobManager.ShowFullscreenNextLevelInterstitialAd();
        }

    }

    public void LogFirebaseWhenWin()
    {
        if (!PlayerData.checkIfContainsCompleteLevel(GetLevelSelecting()))
        {
            firebaseManager.PassLevel(GetLevelSelecting() + 1);
        }
    }

    public void ResetLevel()
    {
        txtLevel.text = $"Level {myCurLv + 1}";
        linesDrawer.ResetLevel();
        if (levelSpawn.childCount > 0)
            Destroy(levelSpawn.GetChild(0).gameObject);
        var temp = Instantiate(levels[myCurLv], levelSpawn);
        myLevel = temp.GetComponent<myLevel>();
        image_Background.sprite = sprites_Background[myCurLv];
        txtTutorialGameObject.SetActive(true);
        txtTutorial.text = textTutorial[myCurLv];
        timeCount.ResetTime();
        soundManager.StopSpecificSound(SoundManager.SoundType.PullRope);
        lineRenderer.enabled = true;

        if (!adsManager.hasInternet) return;

        if (firebaseManager.listLevelShowInterAds.Contains(myCurLv))
        {
            // AdsManager.Instance.ShowInterstitial();
            googleAdMobManager.ShowFullscreenNextLevelInterstitialAd();
        }
    }

    public int GetLevelSelecting()
    {
        return myCurLv;
    }

    public void ClickHint()
    {
        if(!levelSpawn.GetChild(0).GetChild(1).gameObject.activeInHierarchy)
        {
            levelSpawn.GetChild(0).GetChild(1).gameObject.SetActive(true);
        }

        firebaseManager.WatchHintAtLevel(myCurLv + 1);
    }

    void ChangeSound(int numLevel)
    {
        switch(numLevel)
        {
            case 7: case 8:
                soundManager.PlaySound(SoundManager.SoundType.Cow);
            break;
            case 9: case 16: case 17: case 18: case 19:
                soundManager.PlaySound(SoundManager.SoundType.Pig);
            break;
        }
    }

}
