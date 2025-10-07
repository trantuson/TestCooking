using UnityEngine;
using DG.Tweening;

public class Butter : DraggableBase
{
    [SerializeField] private Transform pointPan;

    override protected bool CheckCorrectDropZone()
    {
        return Vector2.Distance(transform.position, pointPan.position) < 2.0f;
    }
    override protected void OnDropSuccess()
    {
        transform.DOKill(); // Kill tween cũ nếu có

        transform.DOMove(pointPan.position, 0.5f).SetEase(Ease.OutSine)
            .OnComplete(() =>
            {
                transform.DOScale(new Vector3(0.5f, 0.5f, 1f), 0.3f)
                         .SetEase(Ease.InOutSine);
                GetComponent<Collider2D>().enabled = false;
                this.enabled = false;
            });
    }
    override protected void OnDropFail()
    {
        base.OnDropFail();
    }
}
