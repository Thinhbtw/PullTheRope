using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopCloseOpen : MonoBehaviour
{
    [SerializeField] ClamCloseOpen closeOpen;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        closeOpen.checkifTouchRope = true;
    }
}
