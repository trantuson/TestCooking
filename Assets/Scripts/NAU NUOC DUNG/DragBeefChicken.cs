using UnityEngine;
using DG.Tweening;

public class DragBeefChicken : DraggableBase
{
    [SerializeField] private ClayPotWater clayPotWater;
    [SerializeField] private Transform pointClayChicken;
    [SerializeField] private Transform pointPlateChicken;
    private float distToClay;
    private float distToPlate;

    public bool isClayPot;

    private enum DragStage { ToPlate, ToClayPot, Finished }
    private DragStage currentStage = DragStage.ToPlate;

    protected override bool CheckCorrectDropZone()
    {
        distToClay = Vector2.Distance(transform.position, pointClayChicken.position);
        distToPlate = Vector2.Distance(transform.position, pointPlateChicken.position);

        // Chỉ cho phép drop tùy theo stage hiện tại
        if (currentStage == DragStage.ToPlate)
            return distToPlate <= 1f;
        else if (currentStage == DragStage.ToClayPot)
            return distToClay <= 1f;
        else
            return false; // Finished => không kéo nữa
    }

    protected override void OnDropSuccess()
    {
        if (currentStage == DragStage.ToPlate && distToPlate <= 1f)
        {
            DropOnPlate(pointPlateChicken.position);
            // Sau khi đã đặt vào đĩa -> cho phép kéo đến nồi
            currentStage = DragStage.ToClayPot;
        }
        else if (currentStage == DragStage.ToClayPot && distToClay <= 1f && clayPotWater.isWaterTrue)
        {
            isClayPot = true;
            DropInPot(pointClayChicken.position);
            // Khi đã bỏ vào nồi => không cho kéo nữa
            currentStage = DragStage.Finished;
            enabled = false; // tắt drag luôn
        }
        else
        {
            OnDropFail();
        }
    }

    protected override void OnDropFail()
    {
        base.OnDropFail();
    }

    private void DropOnPlate(Vector3 targetPos)
    {
        transform.DOMove(targetPos, 0.4f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            transform.DOKill();
            transform.DOPunchScale(Vector3.one * 0.15f, 0.3f, 4, 0.5f);
        });
    }

    private void DropInPot(Vector3 targetPos)
    {
        transform.DOMove(targetPos, 0.4f).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            transform.DOKill();
            Sequence seq = DOTween.Sequence();
            seq.Append(transform.DOLocalMoveY(transform.localPosition.y - 0.1f, 0.3f).SetEase(Ease.InOutSine));
            seq.Append(transform.DOLocalMoveY(transform.localPosition.y, 0.3f).SetEase(Ease.InOutSine));
        });
    }
}
