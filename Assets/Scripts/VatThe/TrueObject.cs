using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 

public class TrueObject : MonoBehaviour
{
    public myTarget player;
    Rigidbody2D rg2D;
    bool hasBeenSeen = false;
    public bool onABoat;


    private void Start()
    {
        rg2D = GetComponent<Rigidbody2D>();
        rg2D.isKinematic = true;
        if (onABoat)
        {
            rg2D.mass = 1;
            rg2D.isKinematic = false;
        }
    }

    private void Update()
    {
        if (player.CheckIfGameHasEnd())
        {
            rg2D.bodyType = RigidbodyType2D.Static;
            return;
        }
        if (onABoat) return;
        if (!LinesDrawer.Instance.cantDraw) return;
        else
        {
            rg2D.isKinematic = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Player":
                player.countTrueEdge += 1;
                break;
            case "Polygon":
                player.countTruePolygon += 1;
                break;
            case "Guard":
                player.hasBeenCaught = true;
                if (!CountDown.Instance.hasntCountDown)
                {
                    SoundManager.Instance.PlaySound(SoundManager.SoundType.ThiefGotCaught);
                }
                break;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Guard":
                if (!CountDown.Instance.hasntCountDown && !hasBeenSeen)
                {
                    hasBeenSeen = true;
                    SoundManager.Instance.PlaySound(SoundManager.SoundType.ThiefGotCaught);
                }
                break;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Player":
                player.countTrueEdge -= 1;
                break;
            case "Polygon":
                player.countTruePolygon -= 1;
                break;
            case "Guard":
                player.hasBeenCaught = false;
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (player.isOver) return;
        if(collision.gameObject.CompareTag("Spike"))
        {
            SoundManager.Instance.PlaySound(SoundManager.SoundType.HitSpike);
            player.isOver = true;
        }
        if(onABoat)
        {
            rg2D.isKinematic = false;
            rg2D.mass = 999;
            this.gameObject.GetComponent<BoatFloating>().enabled = false;
        }
    }

}
