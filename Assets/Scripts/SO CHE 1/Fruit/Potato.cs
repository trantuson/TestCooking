using UnityEngine;

public class Potato : DraggableBase
{
    [SerializeField] private Transform[] potato;
    [SerializeField] private Transform[] pointPotatoCuttingBoard;
    protected override bool CheckCorrectDropZone()
    {
        // tinh distance cuttingboard
        return Vector2.Distance(transform.position, pointPotatoCuttingBoard[0].position) <= 0.3f;
    }
    protected override void OnDropSuccess()
    {
        for (int i = 0; i < potato.Length && i < pointPotatoCuttingBoard.Length; i++)
        {
            potato[i].position = pointPotatoCuttingBoard[i].position;
        }
    }
    protected override void OnDropFail()
    {
        base.OnDropFail();
    }
}
