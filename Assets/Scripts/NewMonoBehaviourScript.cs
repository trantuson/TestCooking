using DG.Tweening;
using UnityEngine;

public abstract class DraggableBase : MonoBehaviour
{
    protected Vector3 startLocalPosition; // sửa
    protected Transform originalParent; // lưu vị trí parent gốc
    protected bool isDragging;
    private Vector3 offset;
    private int originalSortingOrder;

    protected virtual void Start()
    {
        startLocalPosition = transform.localPosition;
        originalParent = transform.parent; // lưu parent gốc
    }

    protected virtual void Update()
    {
        HandleDragging();
    }

    private void HandleDragging()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (IsMouseOver())
            {
                isDragging = true;
                offset = transform.position - GetMouseWorldPos();
                OnDragStart();

                // đưa lên trên cùng
                var sr = GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    originalSortingOrder = sr.sortingOrder;
                    sr.sortingOrder = 100;
                }
            }
        }

        if (isDragging && Input.GetMouseButton(0))
        {
            Vector3 mousePos = GetMouseWorldPos();
            transform.position = mousePos + offset;
            OnDragging();
        }

        if (isDragging && Input.GetMouseButtonUp(0))
        {
            isDragging = false;

            // trả sortingOrder lại
            var sr = GetComponent<SpriteRenderer>();
            if (sr != null) sr.sortingOrder = originalSortingOrder;

            if (CheckCorrectDropZone())
                OnDropSuccess();
            else
                OnDropFail();
        }
    }

    protected virtual void OnDragStart() { }
    protected virtual void OnDragging() { }
    protected virtual void OnDropSuccess() { }
    protected virtual void OnDropFail()
    {
        transform.position = transform.parent.TransformPoint(startLocalPosition);
    }

    protected abstract bool CheckCorrectDropZone();

    protected Vector3 GetMouseWorldPos()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    private bool IsMouseOver()
    {
        Vector3 mousePos = GetMouseWorldPos();
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
        return hit.collider != null && hit.collider.gameObject == gameObject;
    }
}
