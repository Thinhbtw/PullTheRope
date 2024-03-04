using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeftRight : MonoBehaviour
{
    Rigidbody2D rg2d;
    [SerializeField] float timeToTurn, speed;
    [Tooltip("1- phai? || -1 trai")]
    [SerializeField] int direction;
    [SerializeField] bool turnOffKinematic;
    myTarget target;
    float timer;
    bool hasTouchRope;
    RectTransform rectTransform;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rg2d = GetComponent<Rigidbody2D>();
        target = myTarget.Instance;
        timer = timeToTurn;
        rg2d.mass = 1;       
        hasTouchRope = false;
        switch(turnOffKinematic)
        {
            case true:
                rg2d.isKinematic = false;
                break;
            case false:
                rg2d.isKinematic = true;
                break;
        }
        switch (direction)
        {
            case 1:
                rectTransform.localRotation = new Quaternion(0f, 0, 0, 0);
                break;
            case -1:
                rectTransform.localRotation = new Quaternion(0f, 180f, 0, 0);
                break;
        }
    }

    private void Update()
    {
        if(target == null)
        {
            target = myTarget.Instance;
        }
        if (target.isOver || target.gameHasEnd)
        {
            rg2d.velocity = Vector2.zero;
            return;
        }
        if(hasTouchRope) return;
        rg2d.velocity = Vector2.right * speed * direction;
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            direction = -direction;
            timer = timeToTurn;
            rg2d.velocity = Vector2.zero;
            switch (direction)
            {
                case 1:
                    rectTransform.localRotation = new Quaternion(0f, 0, 0, 0);
                    break;
                case -1:
                    rectTransform.localRotation = new Quaternion(0f, 180f, 0, 0);
                    break;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        hasTouchRope = true;
        rg2d.mass = 999;
        rg2d.isKinematic = false;
    }
}
