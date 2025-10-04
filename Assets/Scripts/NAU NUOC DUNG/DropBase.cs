using UnityEngine;
using DG.Tweening;

public class DropBase : DraggableBase
{
    [SerializeField] CountOBJ countOBJ;
    [SerializeField] private ClayPotWater clayPotWater;
    [SerializeField] private Transform pointWater;
    public bool isPointTarget = false;
    [SerializeField] private Collider2D ollider2D;
    protected override bool CheckCorrectDropZone()
    {
        return Vector2.Distance(transform.position, pointWater.position) <= 1f;
    }
    protected override void OnDropSuccess()
    {
        if (clayPotWater.isWaterTrue == true)
        {
            countOBJ.obIndex++;
            
            // Bay vào vị trí đích
            transform.DOMove(pointWater.position, 0.3f).SetEase(Ease.OutBack);

            Sequence seq = DOTween.Sequence();

            // 1. Chìm xuống (hạ Y một chút)
            seq.Append(transform.DOLocalMoveY(pointWater.localPosition.y - 0.1f, 0.4f)
                .SetEase(Ease.InOutSine));

            // 2. Nổi lên lại
            seq.Append(transform.DOLocalMoveY(pointWater.localPosition.y + 0.05f, 0.6f)
                .SetEase(Ease.InOutSine));

            // 3. Sau đó trôi qua lại ngang (loop vô hạn)
            seq.AppendCallback(() =>
            {
                transform.DOLocalMoveX(pointWater.localPosition.x + 0.06f, 0.8f)
                    .SetEase(Ease.InOutSine)
                    .SetLoops(-1, LoopType.Yoyo);
            });

            
            // 👉 Không cho kéo thả lại nữa
            GetComponent<Collider2D>().enabled = false;
            this.enabled = false;  // tắt luôn script Pepper (hoặc DraggableBase)
        }
        else OnDropFail();
    }
    protected override void OnDropFail()
    {
        base.OnDropFail();    
    }
}
