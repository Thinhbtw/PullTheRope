using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class myTarget : Singleton<myTarget>
{
    public int countTrueEdge, countTruePolygon;
    public int countTrashEdge, countTrashPolygon;
    public bool isOver, objStuck, hasBeenCaught, gameHasEnd;
    [SerializeField]EdgeCollider2D edgeCollider;
    [SerializeField] PolygonCollider2D polygon;
    Vector2[] vector2s;
    UpdateLine updateLine;
    UIGamePlay uIGamePlay;
    CountDown countDown;

    private void Start()
    {
        vector2s = new Vector2[0];
        edgeCollider = GetComponent<EdgeCollider2D>();
        edgeCollider.isTrigger = polygon.isTrigger = true;
        polygon.gameObject.transform.position = this.transform.position;
        edgeCollider.edgeRadius = 0.08f;
        edgeCollider.points = polygon.points = vector2s;
        isOver = hasBeenCaught = gameHasEnd = false;
        uIGamePlay = UIGamePlay.Instance;
        countDown = CountDown.Instance;
    }

    private void Update()
    {
        if (UpdateLine.Instance != null && updateLine == null)
        {
            updateLine = UpdateLine.Instance;
            updateLine.gobject_parent = GetComponent<RectTransform>();
        }

        if(updateLine != null)
        {
            switch (uIGamePlay.GetLevelSelecting())
            {
                //lvl 37 39 ko ve~ co' the? thua luon neu' de? dong` lenh. 50 chay. thi` dong` 61 se~ ko chay. va` ko thua dc
                case int x when (uIGamePlay.GetLevelSelecting() == 36 || uIGamePlay.GetLevelSelecting() == 38 || uIGamePlay.GetLevelSelecting() == 39):
                    if (updateLine.GetTimeLeft() < 0f || updateLine.CheckIfGameHasEnd())
                    {
                        gameHasEnd = true;
                        return;
                    }
                    break;
                case int x when (uIGamePlay.GetLevelSelecting() != 36 && uIGamePlay.GetLevelSelecting() != 38 && uIGamePlay.GetLevelSelecting() != 39):
                    if (!updateLine.CheckIfPlayerHasDrawn()) return;
                    else if (updateLine.GetTimeLeft() < 0f || updateLine.CheckIfGameHasEnd())
                    {
                        gameHasEnd = true;
                        return;
                    }
                    break;
            }

        }

        switch(isOver)
        {
            case true:
                countDown.hasTouchSpike = true;
                break;
            case false:
                countDown.hasTouchSpike = false;
                break;
        }

        switch (hasBeenCaught)
        {         
            case true:
                countDown.guardCaught = true;
                break;
            case false:
                countDown.guardCaught = false;
                break;
        }

        if (updateLine !=null && (updateLine.GetTimeLeft() < .5f) || countDown.GetTime() < .5f) //performance
        {
            edgeCollider.points = updateLine.allUpdatedPosition.ToArray();
            polygon.points = updateLine.allUpdatedPosition.ToArray();
        }
    }

    public bool CheckIfGameHasEnd()
    {
        return gameHasEnd;
    }
   
}
