using NUnit.Framework;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class KnifeMaskFollow : DraggableBase
{
    [SerializeField] private Transform PointBoard;
    private Vector3 placedPosition;
    [SerializeField] private float distanceCheck = 0.3f;
    private bool isPlaced = false;
    [SerializeField] private GameObject sss;
    [SerializeField] private float cutStep = 0.1f; 
    private float currentCutOffset = 0f;
    [SerializeField] private float maxCutDistance = 1f;

    protected override void Update()
    {
        base.Update();

        if (isPlaced)
        {
            if (Input.GetMouseButtonDown(0)) // click chuột trái
            {
                currentCutOffset += cutStep;
                if (currentCutOffset >= maxCutDistance)
                {
                    sss.SetActive(false);
                    transform.position = startLocalPosition;
                    isPlaced = false;
                    currentCutOffset = 0;
                }
                // click ddeer di chuyen
                transform.position = placedPosition + new Vector3(currentCutOffset, 0, 0);
            }
        }
    }
    protected override bool CheckCorrectDropZone()
    {
        if (isPlaced) return false;
        return Vector2.Distance(transform.position, PointBoard.position) <= distanceCheck;
    }
    protected override void OnDropSuccess()
    {
        transform.position = PointBoard.position;
        placedPosition = transform.position;
        isPlaced = true;
    }
    protected override void OnDropFail()
    {
        transform.position = transform.parent.TransformPoint(startLocalPosition);
    }
}
