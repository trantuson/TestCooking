using UnityEngine;

public class Carrot : DraggableBase
{
    [SerializeField] private Transform carrot;
    private DropEffect dropEffect;
    protected override bool CheckCorrectDropZone()
    {
        return Vector2.Distance(transform.position, carrot.position) <= 0.5f;
    }
    protected override void OnDropSuccess()
    {
        transform.position = carrot.position;
        // gọi hiệu ứng đặt xuống
        if (dropEffect) dropEffect.PlayDropEffect();
    }
    protected override void OnDropFail()
    {
        transform.position = transform.parent.TransformPoint(startLocalPosition);
    }
}
