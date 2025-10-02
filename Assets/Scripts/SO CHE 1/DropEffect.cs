using UnityEngine;
using DG.Tweening;

public class DropEffect : MonoBehaviour
{
    [Header("Drop Settings")]
    public float scaleUp = 1.2f;
    public float scaleDuration = 0.2f;
    public float rotateAngle = 10f; // độ nghiêng tối đa
    public float rotateDuration = 0.15f;

    private Vector3 originalScale;
    private Vector3 originalRotation;

    void Awake()
    {
        originalScale = transform.localScale;
        originalRotation = transform.eulerAngles;
    }

    /// <summary>
    /// Gọi khi thả xuống thành công
    /// </summary>
    public void PlayDropEffect()
    {
        transform.DOKill();

        // scale
        transform.localScale = originalScale * scaleUp;
        transform.DOScale(originalScale, scaleDuration).SetEase(Ease.OutBack);

        // xoay nghiêng -> rồi về lại
        transform.DORotate(originalRotation + new Vector3(0, 0, Random.Range(-rotateAngle, rotateAngle)), rotateDuration)
                 .SetLoops(2, LoopType.Yoyo)
                 .SetEase(Ease.OutQuad);
    }
}
