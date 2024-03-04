using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myLevel : Singleton<myLevel>
{
    public GameObject target;
    public int countTarget;
    public bool hasEnded;
    public bool checkHowManyRopeLeft;
    myTarget myTarget;
    int levelNumber;
    UIGamePlay uIGamePlay;
    private void OnEnable()
    {
        SetTargetToLine();
        hasEnded = false;
        levelNumber = int.Parse(this.gameObject.name.Substring(0,2)); //lay' 2 ki' tu. dau` trong level name
    }

    private void Start()
    {
        myTarget = target.GetComponent<myTarget>();
        uIGamePlay = UIGamePlay.Instance;
    }

    private void SetTargetToLine()
    {
        if (LinesDrawer.Instance == null)
            return;
        LinesDrawer.Instance._target = target.transform;
    }

    private void Update()
    {
        if (hasEnded) return;
        if (myTarget.countTrueEdge >= countTarget)
        {
            hasEnded = true;
        }
    }

    public void CheckEnd()
    {
        hasEnded = true;
        if (myTarget.countTrashEdge > 0 || myTarget.countTrashPolygon > 0) //keo' phai? rac'
        {
            StartCoroutine(TurnOnPanelEnd("Lose", 1, 0f));
        }
        else if ((myTarget.countTrueEdge == countTarget || myTarget.countTruePolygon == countTarget)  && !myTarget.isOver && !myTarget.objStuck)
        {
            //Show UI finish
            StartCoroutine( TurnOnPanelEnd ("Win", 0, 0f));
        }
        else //ko keo' dc gi` hoac. keo' thieu' true object 
        {
            switch (levelNumber)
            {
                case int x when (levelNumber > 15 && levelNumber < 21 || levelNumber > 7 && levelNumber < 11):
                    StartCoroutine(TurnOnPanelEnd("Lose", 1, 0f));
                    break;
                case int x when (levelNumber < 16 || levelNumber > 20):
                    StartCoroutine(TurnOnPanelEnd("Lose", 0, 0f));
                    break;
            }
        }     
    }
    public void LostPanel() //keo' trung' gai hoac. bi guard nhin thay hoac bom
    {
        StartCoroutine(TurnOnPanelEnd("Lose", 0, 0.3f));
        hasEnded = true;
    }

    IEnumerator TurnOnPanelEnd(string winOrLose,int indexImageLose, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if(winOrLose.Contains("Lose"))
        {
            Lose(indexImageLose);
        }
        else
        {
            Win();
            if (uIGamePlay.GetLevelSelecting() + 1 < uIGamePlay.levels.Count)
            {
                PlayerData.AddLevelComplete(uIGamePlay.GetLevelSelecting() + 1);
            }  
            uIGamePlay.LogFirebaseWhenWin();
        }
    }

    //CodeTam
    void Lose(int indexImageLose)
    {
        switch (UIGamePlay.Instance.GetLevelSelecting())
        {
            case 0:
                SoundManager.Instance.PlaySound(SoundManager.SoundType.LoseNoel);
                break;
            case 2: case 3: case 4:
                SoundManager.Instance.PlaySound(SoundManager.SoundType.FailMain);
                break;
            case 1:
                SoundManager.Instance.PlaySound(SoundManager.SoundType.LoseGoblin);
                SoundManager.Instance.PlaySound(SoundManager.SoundType.FailMain);
                break;
            case int x when (UIGamePlay.Instance.GetLevelSelecting() >= 5 && UIGamePlay.Instance.GetLevelSelecting() < 9):
                SoundManager.Instance.PlaySound(SoundManager.SoundType.FarmerCry);
                SoundManager.Instance.PlaySound(SoundManager.SoundType.FailMain);
                break;
            case 9:
                SoundManager.Instance.PlaySound(SoundManager.SoundType.FarmerAngry);
                SoundManager.Instance.PlaySound(SoundManager.SoundType.FailMain);
                break;
            case int x when (UIGamePlay.Instance.GetLevelSelecting() >= 10 && UIGamePlay.Instance.GetLevelSelecting() < 15):
                SoundManager.Instance.PlaySound(SoundManager.SoundType.ThiefLose);
                SoundManager.Instance.PlaySound(SoundManager.SoundType.FailMain);
                break;
            case int x when (UIGamePlay.Instance.GetLevelSelecting() >= 15 && UIGamePlay.Instance.GetLevelSelecting() < 20):
                SoundManager.Instance.PlaySound(SoundManager.SoundType.FarmerCry);
                SoundManager.Instance.PlaySound(SoundManager.SoundType.FailMain);
                break;
            case int x when (UIGamePlay.Instance.GetLevelSelecting() >= 20):
                SoundManager.Instance.PlaySound(SoundManager.SoundType.FailMain);
                break;

        }
        ButtonManager.Instance.TurnOnPanelComplete(true, false, indexImageLose);
    }
    void Win()
    {
        switch(UIGamePlay.Instance.GetLevelSelecting())
        {
            case 0: case 3: case 4:
                SoundManager.Instance.PlaySound(SoundManager.SoundType.SuccessNoel);
            break;
            case 1: case 2:
                SoundManager.Instance.PlaySound(SoundManager.SoundType.SuccessGoblin);
            break;
            case int x when (UIGamePlay.Instance.GetLevelSelecting() >= 5 && UIGamePlay.Instance.GetLevelSelecting() < 10 || UIGamePlay.Instance.GetLevelSelecting() >= 15 && UIGamePlay.Instance.GetLevelSelecting() < 20):
                SoundManager.Instance.PlaySound(SoundManager.SoundType.FarmerLaugh);
                SoundManager.Instance.PlaySound(SoundManager.SoundType.SuccessMain);
            break;
            case int x when (UIGamePlay.Instance.GetLevelSelecting() >= 10 && UIGamePlay.Instance.GetLevelSelecting() < 15):
                SoundManager.Instance.PlaySound(SoundManager.SoundType.ThiefWin);
                SoundManager.Instance.PlaySound(SoundManager.SoundType.SuccessMain);
                break;
            case int x when (UIGamePlay.Instance.GetLevelSelecting() >= 20):
                SoundManager.Instance.PlaySound(SoundManager.SoundType.SuccessMain);
                break;


        }
        ButtonManager.Instance.TurnOnPanelComplete(false, true, 0);
    }
}
