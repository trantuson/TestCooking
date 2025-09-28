using System.Runtime.CompilerServices;
using UnityEngine;

public class BlockWater : MonoBehaviour
{
    [SerializeField] private Transform drainagePoint;
    [SerializeField] private float snapDistance = 0.1f;

    private Vector3 originalLocalPosition;
    public bool isSnapped = false;

    private void Start()
    {
        originalLocalPosition = transform.localPosition;
    }

    private void OnMouseDrag()
    {
        // Cho object đi theo chuột
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 10;
        transform.position = mousePos;

        float distance = Vector2.Distance(transform.position, drainagePoint.position);
        Debug.Log("Distance = " + distance);
        if (distance <= snapDistance)
        {
            isSnapped = true;
            transform.position = drainagePoint.position;
        }
        else
        {
            isSnapped = false;
        }
    }

    private void OnMouseUp()
    {
        if (!isSnapped)
        {
            transform.localPosition = originalLocalPosition;
        }
    }
}
