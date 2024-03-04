using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatFloating : Singleton<BoatFloating>
{
    [SerializeField] Rigidbody2D rg2d;
    [SerializeField] Vector3 defaultRectPos;
    [SerializeField] bool notResetingDefaultPos;
    bool stop;
    public float speed, timeToTurn;
    [Tooltip("right || left || up || down")]
    [SerializeField] string direction;
    float orginTimer;
    void Start()
    {
        defaultRectPos = transform.position;
        orginTimer = timeToTurn;
        stop = false;
        switch(direction)
        {
            case "right":
                rg2d.AddForce(Vector2.right * speed, ForceMode2D.Impulse);
                break;
            case "left":
                rg2d.AddForce(Vector2.left * speed, ForceMode2D.Impulse);
                break;
            case "up":
                rg2d.AddForce(Vector2.up * speed, ForceMode2D.Impulse);
                break;
            case "down":
                rg2d.AddForce(Vector2.down * speed, ForceMode2D.Impulse);
                break;
        }
    }

    private void Update()
    {
        if (notResetingDefaultPos) return;

        if (stop) 
        {
            transform.position = defaultRectPos;
            timeToTurn = orginTimer; 
            stop = false;
        }
        if (timeToTurn <= 0)
        {
            stop = true;
        }
        timeToTurn -= Time.deltaTime;
    }

}
