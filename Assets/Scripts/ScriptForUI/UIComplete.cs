using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class UIComplete : Singleton<UIComplete>
{
    [SerializeField] float fadeTime = 1f;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] RectTransform rectTransform;
    [SerializeField] UIGamePlay uiGamePlay;
    [SerializeField] Image loseImage, winImage;
    [SerializeField] ButtonManager btnManager;
    [Serializable]
    public class EndImage
    {
        public Sprite imageWin;
        public Vector2 scaleImageWin;
        public Vector3 scaleObjectWin;
        [Space(10f)]
        public Sprite imageLose;
        public Vector2 scaleImageLose;
        public Vector3 scaleObjectLose;
        [Space(10f)]
        public Sprite imageLose2;
        public Vector2 scaleImageLose2;
        public Vector3 scaleObjectLose2;
    }

    public List<EndImage> endImageList;

    void OnEnable()
    {
        if(btnManager.indexOfLoseImg == 0)
        {
            loseImage.sprite = endImageList[uiGamePlay.GetLevelSelecting()].imageLose;
            loseImage.rectTransform.sizeDelta = endImageList[uiGamePlay.GetLevelSelecting()].scaleImageLose;
            loseImage.rectTransform.localScale = endImageList[uiGamePlay.GetLevelSelecting()].scaleObjectLose;
        }
        else
        {
            loseImage.sprite = endImageList[uiGamePlay.GetLevelSelecting()].imageLose2;
            loseImage.rectTransform.sizeDelta = endImageList[uiGamePlay.GetLevelSelecting()].scaleImageLose2;
            loseImage.rectTransform.localScale = endImageList[uiGamePlay.GetLevelSelecting()].scaleObjectLose2;
        }

        winImage.sprite = endImageList[uiGamePlay.GetLevelSelecting()].imageWin;
        winImage.rectTransform.sizeDelta = endImageList[uiGamePlay.GetLevelSelecting()].scaleImageWin;
        winImage.rectTransform.localScale = endImageList[uiGamePlay.GetLevelSelecting()].scaleObjectWin;

        PanelFadeIn();
    }

    public void PanelFadeIn()
    {
        canvasGroup.alpha = 0f;
        rectTransform.transform.localPosition = new Vector3(0f, -1000f, 0f);
        rectTransform.DOAnchorPos(new Vector2(0f, 0f), fadeTime, false).SetEase(Ease.OutElastic);
        canvasGroup.DOFade(1, fadeTime);
    }

    public void PanelFadeOut()
    {
        canvasGroup.alpha = 1f;
        rectTransform.transform.localPosition = new Vector3(0f, 0f, 0f);
        rectTransform.DOAnchorPos(new Vector2(0f, -1000f), fadeTime, false).SetEase(Ease.InOutQuint);
        canvasGroup.DOFade(0, fadeTime);
        StartCoroutine(ResetAnimation());
    }

    IEnumerator ResetAnimation()
    {
        yield return new WaitForSeconds(fadeTime);
        this.gameObject.SetActive(false);
    }
}
