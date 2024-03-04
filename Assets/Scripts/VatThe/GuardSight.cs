using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuardSight : MonoBehaviour
{
    [SerializeField] float timeToRorate;
    float timer;
    [Tooltip("0: left right || 1: up down")]
    [SerializeField] int direction;
    RectTransform guard;

    [Header("if director == 1")]
    [SerializeField] float maxY;
    [SerializeField] float minY;
    [SerializeField] float speed;
    [SerializeField] Sprite goUp, goDown;
    [SerializeField] Image guardImg;
    [SerializeField] PolygonCollider2D polygonCollider2D;
    bool isGoingDown;
     Vector2[] downSight_Point =
    {
        new Vector2(-53.16969f,113.9152f),
        new Vector2(88.62471f,-206.8779f),
        new Vector2(-192.4426f,-212.6307f)
    };
    Vector2[] upSight_Point =
    {
        new Vector2(12.5882f,52.10255f),
        new Vector2(178.1232f,440.3325f),
        new Vector2(-150.0074f,442.0022f)
    };


    void Start()
    {
        guard = GetComponent<RectTransform>();
        guardImg = GetComponent<Image>();
        timer = timeToRorate;
        isGoingDown = true;
    }

    private void Update()
    {
        switch (direction)
        {
            case 0:
                timer -= Time.deltaTime;
                if (timer <=0f)
                {
                    if (guard.localRotation.y == 0f)
                    {
                        guard.localRotation = new Quaternion(0f, 180f, 0f,0f);
                    }
                    else
                    {
                        guard.localRotation = new Quaternion(0f, 0f, 0f, 0f);
                    }
                    timer = timeToRorate;
                }    
                break;
            case 1:
                switch(isGoingDown)
                {
                    case true:
                        guard.localPosition += new Vector3(0f, 1f, 0) * -speed;
                    break;
                    case false:
                        guard.localPosition += new Vector3(0f, 1f, 0) * speed;
                        break;
                }
                if (guard.localPosition.y >= maxY)
                {
                    isGoingDown = true;
                    guard.sizeDelta = new Vector2(400f, 542f);
                    guardImg.sprite = goDown;
                    polygonCollider2D.points = downSight_Point;
                }
                if(guard.localPosition.y <= minY)
                {
                    isGoingDown = false;
                    guard.sizeDelta = new Vector2(400f, 1084f);
                    guardImg.sprite = goUp;
                    polygonCollider2D.points = upSight_Point;
                }
                break;
        }
    }
}
