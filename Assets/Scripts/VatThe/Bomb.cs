using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    Animator anim;
    bool hasExplo;
    PolygonCollider2D polygon;

    private void Start()
    {
        anim = GetComponent<Animator>();
        polygon = GetComponent<PolygonCollider2D>();
        hasExplo = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasExplo) return;
        if (collision.gameObject.CompareTag("Fail_Obj") || collision.gameObject.CompareTag("Pig"))
        {
            StartCoroutine(PlayExploAnimation(0f));
            collision.gameObject.SetActive(false);
            return;
        }
        else 
        {
            StartCoroutine(PlayExploAnimation(0f));
            Invoke("GameIsOver", 1f);
        }
    }
    
    IEnumerator PlayExploAnimation(float timer)
    {
        yield return new WaitForSeconds(timer);
        anim.Play("bomb");
        if (PlayerData.GetVibrationState())
        {
            Handheld.Vibrate();
        }
        SoundManager.Instance.PlaySound(SoundManager.SoundType.Explosion);
        polygon.enabled = false;
        yield return new WaitForSeconds(0.5f);
        this.gameObject.SetActive(false);
        hasExplo = true;
    }

    void GameIsOver()
    {
        myTarget.Instance.isOver = true;
    }
}
