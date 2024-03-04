using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : Singleton<CountDown>
{
    float timeCount=10;
    public bool hasntCountDown=true, hasTouchSpike = false, guardCaught = false, hasWin = false;
    [SerializeField] GameObject clock;
    UIGamePlay uIGamePlay;
    [SerializeField] RemoveRope removeRope;
    [SerializeField] GameObject uIComplete;

    private void Start()
    {
        uIGamePlay = UIGamePlay.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (uIComplete.activeInHierarchy) return; //loi~ o? man` 37 39 40 neu' thua ma` van~ dang chua tha? tay de ve day thi` sound chay lien tuc 

        switch(uIGamePlay.GetLevelSelecting()) //chi? cho cac' man` 37 39 40 
        {
            case int x when (uIGamePlay.GetLevelSelecting() == 36 || uIGamePlay.GetLevelSelecting() == 38 || uIGamePlay.GetLevelSelecting() == 39):
                if(hasTouchSpike)
                {
                    SoundManager.Instance.StopSpecificSound(SoundManager.SoundType.PullRope);
                    hasntCountDown = true;
                    myLevel.Instance.LostPanel();
                }
                break;
        }
        if (hasntCountDown) return;
        if (removeRope.gameObject.activeInHierarchy)
        {
            if (removeRope.timerCheck <= 0f)
            {
                SoundManager.Instance.StopSpecificSound(SoundManager.SoundType.PullRope);
                hasntCountDown = true;
                myLevel.Instance.CheckEnd();
            }
        }
        switch(hasTouchSpike || guardCaught)
        {
            case true:
                SoundManager.Instance.StopSpecificSound(SoundManager.SoundType.PullRope);
                hasntCountDown = true;
                myLevel.Instance.LostPanel();
                break;
        }
        if (timeCount <= 0)
        {
            SoundManager.Instance.StopSpecificSound(SoundManager.SoundType.PullRope);
            hasntCountDown = true;
            myLevel.Instance.CheckEnd();
        }
        if(timeCount <= 5)
        {
            clock.SetActive(true);
        }
        timeCount -= Time.deltaTime;
        GetComponent<Text>().text = "00:0"+((int)timeCount).ToString();
    }

    public float GetTime()
    {
        return timeCount;
    }

    public void ResetTime()
    {
        hasTouchSpike = guardCaught = false;
        timeCount = 10;
        hasntCountDown = true;
        clock.SetActive(false);
        GetComponent<Text>().text = "00:10";
    }
}
