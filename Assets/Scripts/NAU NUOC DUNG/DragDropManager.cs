using UnityEngine;
using DG.Tweening;
using System.Drawing;

public class DragDropManager : DraggableBase
{
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
        transform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 4, 0.3f);
        isPointTarget = true;
        // Bay vào vị trí đích
        transform.DOMove(pointWater.position, 0.3f)
        .SetEase(Ease.OutBack)
        .OnComplete(() =>
        {
            // Sau khi bay vào, giảm scale XY một xíu cho giống như nằm trong nồi
            transform.DOScale(new Vector3(0.8f, 0.8f, transform.localScale.z), 0.2f)
                     .SetEase(Ease.InOutSine);
        });

    // Punch nhẹ để có cảm giác thả xuống
    transform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 4, 0.3f);
    }
    protected override void OnDropFail()
    {
        // Bay về vị trí ban đầu
        transform.DOMove(transform.parent.TransformPoint(startLocalPosition), 0.3f)
             .SetEase(Ease.InOutSine)
             .OnComplete(() =>
             {
                // Scale trở lại kích thước gốc
                transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.InOutSine);
                 // Sau khi bay về chỗ cũ thì lắc ngang
                 transform.DOShakePosition(0.3f, new Vector3(0.2f, 0, 0), 10, 90);
                 // Nếu trong nồi có nước thì disable drag sau khi quay về
                if (clayPotWater.isWaterTrue == true)
                {
                    ollider2D.enabled = false; // tắt collider
                    this.enabled = false;       // hoặc disable luôn script drag
                }
             });
    }
}
