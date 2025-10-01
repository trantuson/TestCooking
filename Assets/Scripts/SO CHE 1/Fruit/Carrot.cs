using UnityEngine;

public class Carrot : DraggableBase
{
    [SerializeField] private Transform carrot;
    protected override bool CheckCorrectDropZone()
    {
        return Vector2.Distance(transform.position, carrot.position) <= 0.3f;
    }
    protected override void OnDropSuccess()
    {
        transform.position = carrot.position;
    }
    protected override void OnDropFail()
    {
        transform.position = transform.parent.TransformPoint(startLocalPosition);
    }
}
