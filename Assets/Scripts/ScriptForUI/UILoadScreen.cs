using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UILoadScreen : MonoBehaviour
{
    [SerializeField] Image progress;
    [SerializeField] float duration;
    [SerializeField] public Image Transition;
    [SerializeField] GameObject Home;
    [SerializeField] GameObject soundManager;
    [SerializeField] GoogleAdMobManager googleAdMobManager;
    [SerializeField] AdsManager adsManager;
    [SerializeField] FirebaseManager firebaseManager;
    

    // Start is called before the first frame update
    void Start()
    {
        Home.SetActive(false);
        soundManager.SetActive(false);

        Transition.DOFade(0f, 2f);

        StartCoroutine(ChangeSomeValue(0, 1, duration));
    }
    
    public IEnumerator ChangeSomeValue(float oldValue, float newValue, float duration)
    {
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            progress.fillAmount = Mathf.Lerp(oldValue, newValue, t / duration);
            yield return null;
        }
        progress.fillAmount = newValue;
        StartCoroutine(toTheGame());
    }

    public IEnumerator toTheGame()
    {
        yield return new WaitForSeconds(0.5f);
        googleAdMobManager.ShowAppOpenAd();
        soundManager.SetActive(true);
        Home.SetActive(true);
        gameObject.SetActive(false);
        if(!adsManager.hasInternet)
        {
            firebaseManager.PlayerIsOffline();
        }
        else
        {
            firebaseManager.PlayerIsOnline();
        }
    }

    private void Update()
    {
        if (googleAdMobManager.hasLoadOpenAds())
        {
            StopCoroutine(ChangeSomeValue(0, 1, duration));
            StartCoroutine(toTheGame());
        }
        else if (googleAdMobManager.loadOpenAdFailed)
        {
            StopCoroutine(ChangeSomeValue(0, 1, duration));
            StartCoroutine(toTheGame());
        }
    }
}
