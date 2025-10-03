using UnityEngine;
using DG.Tweening;

public class DrapDropOBJ : DraggableBase
{
    [SerializeField] CountOBJ countOBJ;
    [SerializeField] private ClayPotWater clayPotWater;
    [SerializeField] private Transform pointClay;
    [SerializeField] private Transform pointPlate;
    private float distToClay;
    private float distToPlate;
    private Tween floatingTween;
    protected override bool CheckCorrectDropZone()
    {
        // Kiểm tra nếu gần nồi hoặc gần dĩa thì cho phép
        distToClay = Vector2.Distance(transform.position, pointClay.position);
        distToPlate = Vector2.Distance(transform.position, pointPlate.position);

        return distToClay <= 1f || distToPlate <= 1f;
    }
    protected override void OnDropSuccess()
    {
        if (distToClay <= 1f) // Nếu gần nồi
        {
            if (clayPotWater.isWaterTrue == true)
            {
                countOBJ.obIndex++;
                Debug.Log("Dropped into Clay Pot: " + countOBJ.obIndex);

                // Bay vào vị trí nồi
                transform.DOMove(pointClay.position, 0.35f)
                        .SetEase(Ease.OutSine)
                        .OnComplete(() =>
                        {
                            StopFloating(); // đảm bảo clear tween cũ

                            // Chỉ tạo hiệu ứng nổi nếu chưa nấu xong
                            if (!Cooking.CookingFinished)
                            {
                                float dipDistance = 0.1f;
                                float dipDuration = 0.8f;

                                floatingTween = transform.DOMoveY(pointClay.position.y - dipDistance, dipDuration)
                                        .SetLoops(-1, LoopType.Yoyo)
                                        .SetEase(Ease.InOutSine);
                            }
                        });
            }
            else
            {
                OnDropFail();
            }
        }
        else if (distToPlate <= 1f) // Nếu gần dĩa
        {
            Debug.Log("Dropped into Plate");

            transform.DOMove(pointPlate.position, 0.35f)
                    .SetEase(Ease.OutBack)
                    .OnComplete(() =>
                    {
                        // Hiệu ứng đặt xuống dĩa nhẹ nhàng
                        transform.DOPunchScale(Vector3.one * 0.05f, 0.2f, 4, 0.4f);
                    });
        }
    }
    protected override void OnDropFail()
    {
        base.OnDropFail();
    }
    public void StopFloating()
    {
        // dừng tween nổi riêng
        if (floatingTween != null && floatingTween.IsActive())
        {
            floatingTween.Kill();
            floatingTween = null;
        }

        // dừng tất cả tween khác gắn vào object này (tránh revive)
        transform.DOKill();

        // reset về vị trí nồi
        transform.position = pointClay.position;
    }
}
