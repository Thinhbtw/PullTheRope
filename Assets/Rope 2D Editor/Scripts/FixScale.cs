using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixScale : MonoBehaviour
{
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    private void OnEnable()
    {
        float myHeight = Screen.height;
        float myWidth = Screen.width;

        float val = 2 - (myHeight / myWidth);
        float temp = val * 90;
        float target = 212 - temp;
        transform.localScale = new Vector3(target, target, target);
    }
}
