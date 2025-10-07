using UnityEngine;
using DG.Tweening;
using System.Collections;

public class Spoon : MonoBehaviour
{
    [Header("Points")]
    [SerializeField] private Transform pointTomato;
    [SerializeField] private Transform pointPAN;

    [Header("Tomato Objects")]
    [SerializeField] private GameObject tomato;
    [SerializeField] private GameObject tomato2;
    [SerializeField] private GameObject tomatoPAN;

    [Header("Settings")]
    [SerializeField] private float checkRadius = 1.0f;

    private bool isDragging = false;
    private bool pickedTomato = false;
    private bool isReturning = false;
    private Vector3 startPos;
    private Coroutine returnRoutine;
    private Collider2D col;

    private void Start()
    {
        startPos = transform.position;
        tomato.SetActive(false);
        tomato2.SetActive(false);
        col = GetComponent<Collider2D>();
    }

    private void OnMouseDown()
    {
        if (isReturning) return;
        isDragging = true;
        pickedTomato = false;
        transform.DOKill();
        if (returnRoutine != null)
            StopCoroutine(returnRoutine);
    }

    private void OnMouseDrag()
    {
        if (isReturning) return;

        // Di chuyển theo chuột
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        transform.position = mousePos;

        float distTomato = Vector2.Distance(transform.position, pointTomato.position);
        float distPan = Vector2.Distance(transform.position, pointPAN.position);

        Tomato tomatoScript = pointTomato.GetComponent<Tomato>();
        bool canScoop = (tomatoScript != null && tomatoScript.CanBeScooped);

        // Gần cà chua => xúc
        if (!pickedTomato && distTomato < checkRadius && canScoop)
        {
            pickedTomato = true;
            tomato.SetActive(true);

            transform.DOKill();
            transform.DORotate(new Vector3(0, 0, 25f), 0.3f).SetEase(Ease.OutSine);
        }

        // Khi đã xúc được và tới gần nồi => đổ
        if (pickedTomato && distPan < checkRadius)
        {
            pickedTomato = false;
            tomato.SetActive(false);
            tomato2.SetActive(true);
            tomatoPAN.SetActive(true);

            // Ngừng drag để tự động đổ
            isDragging = false;
            isReturning = true;

            transform.DOKill();

            transform.DOMove(pointPAN.position, 0.4f)
                .SetEase(Ease.OutSine)
                .OnComplete(() =>
                {
                    // Khi đã đến nồi -> nghiêng để đổ
                    transform.DORotate(new Vector3(0, 0, 45f), 0.3f).SetEase(Ease.OutBack);

                    // Bắt đầu đổ sau khi nghiêng
                    if (returnRoutine == null)
                        returnRoutine = StartCoroutine(PourAndReturn());
                });
        }
    }

    private void OnMouseUp()
    {
        if (isReturning) return;

        // Nếu thả ra mà chưa đổ => reset
        if (isDragging && returnRoutine == null)
        {
            ResetAll();
        }

        isDragging = false;
    }

    private IEnumerator PourAndReturn()
    {
        // Giữ trong 2s giả lập đổ
        yield return new WaitForSeconds(2f);

        // Trả về vị trí ban đầu
        transform.DOKill();
        transform.DOMove(startPos, 0.5f).SetEase(Ease.InOutQuad);
        transform.DORotate(Vector3.zero, 0.4f).SetEase(Ease.InOutSine);
        // Reset & tắt tất cả
        ResetAll();

        // Tắt collider và script (để không kéo lại được)
        if (col != null) col.enabled = false;
        this.enabled = false;

        isReturning = false;
        returnRoutine = null;
    }

    private void ResetAll()
    {
        tomato.SetActive(false);
        tomato2.SetActive(false);

        transform.DOKill();
        transform.DOMove(startPos, 0.3f);
        transform.DORotate(Vector3.zero, 0.3f);

        pickedTomato = false;
        isDragging = false;

        if (returnRoutine != null)
        {
            StopCoroutine(returnRoutine);
            returnRoutine = null;
        }
    }
}
