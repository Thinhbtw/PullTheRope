using UnityEngine;
using DG.Tweening;

public enum AnimationType
{
    MoveX,
    MoveY,
    MoveXAndY,
    Rotate,
    Scale,
    Shake,
}

public class AnimationController : MonoBehaviour
{
    [SerializeField] bool needToResetOnEnable;
    public AnimationType type;
    [SerializeField] float AnimDuration;
    [SerializeField] Ease animEase;
    [SerializeField] int loop;
    [SerializeField] LoopType loopType;

    [Header("Move")]
    [SerializeField] float moveY;
    [SerializeField] float moveX;

    [Header("Rotate")]
    [SerializeField] RotateMode rotateMode;
    [SerializeField] Vector3 numRotate;

    [Header("Scale")]
    [SerializeField] Vector3 numScale;

    [Header("Shake")]
    [SerializeField] float duration;
    [SerializeField] float strength;
    [SerializeField] int vibration;
    [SerializeField] float randomness;
    [SerializeField] bool snapping;

    void Start()
    {
        TypeOfAnimation();
    }

    private void OnEnable()
    {
        if(needToResetOnEnable)
        {
            TypeOfAnimation();
        }
    }

    private void TypeOfAnimation()
    {
        switch (type)
        {
            case AnimationType.MoveX:
                {
                    this.transform.DOMoveX(moveX, AnimDuration).SetEase(animEase).SetLoops(loop, loopType);
                    break;
                }
            case AnimationType.MoveY:
                {
                    this.transform.DOMoveY(moveY, AnimDuration).SetEase(animEase).SetLoops(loop, loopType);
                    break;
                }
            case AnimationType.Rotate:
                {
                    this.transform.DORotate(numRotate, AnimDuration, rotateMode).SetEase(animEase).SetLoops(loop, loopType);
                    break;
                }
            case AnimationType.Scale:
                {
                    this.transform.DOScale(numScale, AnimDuration).SetEase(animEase).SetLoops(loop, loopType);
                    break;
                }
            case AnimationType.Shake:
                {
                    this.transform.DOShakePosition(duration, strength, vibration, randomness, snapping).SetEase(animEase).SetLoops(loop, loopType);
                    break;
                }
        }
    }
}
