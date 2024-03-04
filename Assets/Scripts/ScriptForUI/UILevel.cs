using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILevel : MonoBehaviour
{
    [SerializeField] UIGamePlay uiGame;
    [SerializeField] GameObject btnPrefabs;
    [SerializeField] GameObject gp_panel;
    [SerializeField] Transform btnHolder;
    [SerializeField] Sprite img_complete, img_skip, imp_playable, imp_unplayable;
    [Header("Script")]
    [SerializeField] FirebaseManager firebaseManager;
    //[SerializeField] AdsManager adsManager;
    [SerializeField] GoogleAdMobManager googleAdMobManager;
    private void Start()
    {
        for (int i = 0; i < uiGame.levels.Count; i++) 
        {
            int num = i;
            var btn = Instantiate(btnPrefabs, btnHolder);
            btn.transform.GetComponentInChildren<Text>().text = (num + 1).ToString();
            btn.GetComponent<Button>().onClick.AddListener(() =>
            {
                if(firebaseManager.isShowInterAdsFromGameplay)
                {
                    //adsManager.ShowInterstitialFromGameplay();
                    googleAdMobManager.ShowFullscreenNextLevelInterstitialAd();
                }

                SoundManager.Instance.PlaySound(SoundManager.SoundType.Click);
                SoundManager.Instance.PlaySound(SoundManager.SoundType.BGFarm);
                SoundManager.Instance.StopSpecificSound(SoundManager.SoundType.BGMain);
                uiGame.SetLevel(num);
                gp_panel.SetActive(true);
                this.gameObject.SetActive(false);
            });
            if (num == uiGame.levels.Count - 1)
            {
                gp_panel.SetActive(false);
                this.gameObject.SetActive(false);
            }
        }
        for(int i = uiGame.levels.Count; i < 100; i++)
        {
            int num = i;
            var btn = Instantiate(btnPrefabs, btnHolder);
            btn.transform.GetComponentInChildren<Text>().text = "Coming Soon";
            btn.transform.GetComponentInChildren<Text>().resizeTextMaxSize = 45;
            btn.transform.GetChild(1).GetComponent<Text>().text = "";
            btnHolder.GetChild(num).GetComponent<Button>().interactable = false;
            btnHolder.GetChild(num).GetComponent<Image>().sprite = imp_unplayable;
        }
    }

    private void OnEnable()
    {
        if(btnHolder.transform.childCount > 0) //tranh' truong` hop, luc' bat. game lan` dau button chua spawn
            UnlockLevel();
    }

    public void UnlockLevel()
    {
        for (int i = 0; i < uiGame.levels.Count; i++)
        {
            int num = i;
            if (num <= PlayerData.GetCurrentLevelPlay())
            {
                btnHolder.GetChild(num).GetComponent<Button>().interactable = true;
                btnHolder.GetChild(num).GetComponent<Image>().sprite = imp_playable;
                //VD: them level 1 vao` thi` trong list complete se co' 1 chu' ko phai? 0 nen o? day phai? +1
                if (PlayerData.checkIfContainsCompleteLevel(num + 1))
                {
                    btnHolder.GetChild(num).GetComponent<Image>().sprite = img_complete;
                }
                else if(PlayerData.checkIfContainsSkipLevel(num + 1))
                {
                    btnHolder.GetChild(num).GetComponent<Image>().sprite = img_skip;
                }
            }
            else
            {
                btnHolder.GetChild(num).GetComponent<Button>().interactable = false;
                btnHolder.GetChild(num).GetComponent<Image>().sprite = imp_unplayable;
            }
            
        }
    }

}
