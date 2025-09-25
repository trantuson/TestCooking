using System.Runtime.CompilerServices;
using UnityEngine;

public class BlockWater : MonoBehaviour
{
    private Vector3 offset;
    private Vector3 originalPosition;
    private bool isDragging = false;
    private bool sticky = false;
    public bool isBlocked = false;

    public bool checkBlock = false;
    private void Start()
    {
        originalPosition = transform.position;
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
            if (hit.collider != null && hit.collider.CompareTag("BlockWater"))
            {
                isDragging = true;
                sticky = false;
                offset = transform.position - mousePos;
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
            if(!sticky)
            {
                transform.position = originalPosition;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Drainage"))
        {
            sticky = true;
            transform.position = collision.transform.position;
            originalPosition = new Vector3(-1.305f, 1.148f, 0);
            isDragging = false;
            isBlocked = true;
        }
        if(collision.CompareTag("OriginBlockWater"))
        {
            sticky = true;
            transform.position = collision.transform.position;
            originalPosition = new Vector3(0.152f, 0.501f, 0);
            isDragging = false;
            isBlocked = false;
            checkBlock = true;
        }
    }
    private Vector3 GetMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}
