using UnityEngine;
using DG.Tweening;
using NUnit.Framework;

public class DragBeefChicken : DraggableBase
{
    [SerializeField] private ClayPotWater clayPotWater;
    [SerializeField] private Transform pointClayChicken;
    [SerializeField] private Transform pointPlateChicken;
    private float distToClay;
    private float distToPlate;

    public bool isClayPot;
    protected override bool CheckCorrectDropZone()
    {
        distToClay = Vector2.Distance(transform.position, pointClayChicken.position);
        distToPlate = Vector2.Distance(transform.position, pointPlateChicken.position);

        return distToClay <= 1f || distToPlate <= 1f;
    }
    protected override void OnDropSuccess()
    {
        if (distToClay <= 1f && clayPotWater.isWaterTrue == true)
        {
            isClayPot = true;
            DropInPot(pointClayChicken.position);
        }
        else if (distToPlate <= 1f)
        {
            DropOnPlate(pointPlateChicken.position);
        }
        else OnDropFail();
    }
    protected override void OnDropFail()
    {
        base.OnDropFail();
    }
    private void DropOnPlate(Vector3 targetPos)
    {
        // Bay tới đĩa
        transform.DOMove(targetPos, 0.4f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            // Kill các tween cũ trước khi nảy để không bị cộng dồn
            transform.DOKill();

            // Nảy một cái
            transform.DOPunchScale(Vector3.one * 0.15f, 0.3f, 4, 0.5f);
        });
    }
    private void DropInPot(Vector3 targetPos)
    {
        // Bay tới nồi
        transform.DOMove(targetPos, 0.4f).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            transform.DOKill(); // kill tween cũ

            // Hiệu ứng chìm xuống rồi nổi lên
            Sequence seq = DOTween.Sequence();
            seq.Append(transform.DOLocalMoveY(transform.localPosition.y - 0.1f, 0.3f).SetEase(Ease.InOutSine));
            seq.Append(transform.DOLocalMoveY(transform.localPosition.y, 0.3f).SetEase(Ease.InOutSine));
        });
    }
}
