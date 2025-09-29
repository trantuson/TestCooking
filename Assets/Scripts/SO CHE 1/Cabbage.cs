using UnityEngine;

public class Cabbage : DraggableBase
{
    [SerializeField] private Transform CabbagePoint;
    protected override bool CheckCorrectDropZone()
    {
        return Vector2.Distance(transform.position, CabbagePoint.position) <= 0.3f;
    }
    protected override void OnDropSuccess()
    {
        transform.position = CabbagePoint.position;
    }
    protected override void OnDropFail()
    {
        transform.position = transform.parent.TransformPoint(startLocalPosition);
    }
}
