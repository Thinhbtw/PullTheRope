using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClamCloseOpen : MonoBehaviour
{
    [SerializeField] float setTimer;
    float timer;
    bool clamOpen; 
    public bool checkifTouchRope;

    void Start()
    {
        timer = setTimer;
        clamOpen = true;
        checkifTouchRope = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (checkifTouchRope) return;
        timer -= Time.deltaTime;
        if(timer<=0)
        {
            switch (clamOpen)
            {
                case true:
                    this.transform.GetChild(0).gameObject.SetActive(false);
                    this.transform.GetChild(1).gameObject.SetActive(true);
                    clamOpen = !clamOpen;
                    timer = setTimer;
                break;
                case false:
                    this.transform.GetChild(0).gameObject.SetActive(true);
                    this.transform.GetChild(1).gameObject.SetActive(false);
                    clamOpen = !clamOpen;
                    timer = setTimer;
                    break;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        checkifTouchRope = true;
    }
}
