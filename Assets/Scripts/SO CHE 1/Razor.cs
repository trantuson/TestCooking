//using Unity.VisualScripting;
using UnityEngine;

public class Razor : MonoBehaviour
{
    private Camera cam;
    private Vector3 offset;
    private Vector3 originalPosition;

    private bool isDragging = false;
    private void Start()
    {
        cam = Camera.main;
    }
    private void Update()
    {
        DrapDrop();
    }
    private void DrapDrop()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = GetMouse();
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (hit.collider != null && hit.collider.CompareTag("Razor"))
            {
                isDragging = true;
                offset = transform.position - mousePos;
                originalPosition = transform.position;
            }
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = GetMouse();
            if (isDragging)
            {
                transform.position = mousePos + offset;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            transform.position = originalPosition;
        }
    }
    private Vector3 GetMouse()
    {
        Vector3 mouse = Input.mousePosition;
        mouse.z = 10f;
        return cam.ScreenToWorldPoint(mouse);
    }
}
