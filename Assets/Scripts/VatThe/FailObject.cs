using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailObject : MonoBehaviour
{
    public myTarget player;
    Rigidbody2D rg2D;
    bool hasChangeRigid;

    private void Start()
    {
        rg2D = GetComponent<Rigidbody2D>();
        hasChangeRigid = false;
        rg2D.isKinematic = true;
    }

    private void Update()
    {
        if (player.CheckIfGameHasEnd())
        {
            rg2D.bodyType = RigidbodyType2D.Static;
            return;
        }
        if (!LinesDrawer.Instance.cantDraw || hasChangeRigid) return;
        else
        {
            hasChangeRigid = true;
            rg2D.isKinematic = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Player":
                player.countTrashEdge += 1;
                break;
            case "Polygon":
                player.countTrashPolygon += 1;
                break;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Player":
                player.countTrashEdge -= 1;
                break;
            case "Polygon":
                player.countTrashPolygon -= 1;
                break;
        }
    }

}
