using UnityEngine;

public class DropBeefHT : DraggableBase
{
    [SerializeField] private Transform pointLate;
    [SerializeField] private GameObject obFalse;
    [SerializeField] private GameObject obTrue;

    protected override bool CheckCorrectDropZone()
    {
        return Vector2.Distance(transform.position, pointLate.position) <= 1f;
    }
    protected override void OnDropSuccess()
    {
        transform.position = pointLate.position;
        obTrue.transform.position = pointLate.position;
        obTrue.SetActive(true);
        obFalse.SetActive(false);
    }
    protected override void OnDropFail()
    {
        base.OnDropFail();
    }
}
