using UnityEngine;
using DG.Tweening;

public class CookingSingle : MonoBehaviour
{
    [SerializeField] private GameObject ingredient;  // Nguyên liệu duy nhất
    [SerializeField] private Transform potTransform; // Vị trí nồi (pointClay)
    [SerializeField] private GameObject oilPrefab;   // Prefab giọt mỡ/bọt (tùy thích)
    [SerializeField] private Transform oilParent;    // Nơi chứa giọt mỡ (có thể để trống)

    private bool isCooking = false;
    private Vector3 originalPos;
    private Vector3 originalScale;

    public void StartCooking()
    {
        if (isCooking) return;
        isCooking = true;

        // Ingredient bay vào nồi
        ingredient.transform.DOMove(potTransform.position, 0.4f)
            .SetEase(Ease.OutSine)
            .OnComplete(() =>
            {
                CookSequence();
            });
    }

    private void CookSequence()
    {
        originalPos = transform.localPosition;
        originalScale = transform.localScale;

        // Sequence hiệu ứng "nồi sôi"
        Sequence boilingSeq = DOTween.Sequence();

        boilingSeq.Append(transform.DOLocalMoveX(originalPos.x + 0.05f, 0.4f).SetEase(Ease.InOutSine));
        boilingSeq.Append(transform.DOLocalMoveX(originalPos.x - 0.05f, 0.4f).SetEase(Ease.InOutSine));
        boilingSeq.SetLoops(-1, LoopType.Yoyo);

        transform.DOScaleY(originalScale.y * 1.05f, 0.8f)
                 .SetLoops(-1, LoopType.Yoyo)
                 .SetEase(Ease.InOutSine);

        // Spawn mỡ/bọt liên tục trong 3 giây
        InvokeRepeating(nameof(SpawnOil), 0f, 0.5f);

        // Sau 3 giây thì coi như nấu xong
        DOVirtual.DelayedCall(3f, () =>
        {
            boilingSeq.Kill();
            transform.DOKill();

            transform.localPosition = originalPos;
            transform.localScale = originalScale;

            // Dừng spawn mỡ
            CancelInvoke(nameof(SpawnOil));

            Debug.Log("Cooking finished!");
        });
    }

    private void SpawnOil()
    {
        if (oilPrefab == null) return;

        // Random vị trí quanh nồi
        Vector3 spawnPos = potTransform.position + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(0f, 0.3f), 0);
        GameObject oil = Instantiate(oilPrefab, spawnPos, Quaternion.identity, oilParent);

        // Tween nhỏ dần rồi biến mất
        oil.transform.localScale = Vector3.zero;
        oil.transform.DOScale(Vector3.one * 0.2f, 0.3f).SetEase(Ease.OutBack);
        oil.transform.DOMoveY(spawnPos.y + 0.2f, 1f).SetEase(Ease.OutSine);
        Destroy(oil, 1.2f);
    }
}
