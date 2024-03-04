using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfStuck : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Pole"))
        {
            myTarget.Instance.objStuck = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Pole"))
        {
            myTarget.Instance.objStuck = false;
        }
    }
}
