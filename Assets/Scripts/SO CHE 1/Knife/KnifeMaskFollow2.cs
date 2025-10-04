using UnityEngine;
using DG.Tweening;

public class KnifeMaskFollow2 : DraggableBase
{
    [Header("Setup")]
    [SerializeField] private Transform PointBoard;
    [SerializeField] private Transform startPoint; // vị trí dao ban đầu
    [SerializeField] private float distanceCheck = 0.3f;

    [Header("Cutting")]
    [SerializeField] private float cutStep = 0.1f;
    [SerializeField] private float maxCutDistance = 1f;
    [SerializeField] private float pickRadius = 0.25f;

    [Header("Knife Variants")]
    [SerializeField] private GameObject knifeHorizontal; // dao nằm ngang
    [SerializeField] private GameObject knifeVertical;   // dao dọc
    [SerializeField] private GameObject knifeMask;       // mask cắt

    // runtime
    private Vector3 placedPosition;
    private bool isPlaced = false;
    private float currentCutOffset = 0f;

    private Transform currentTargetParent = null;
    private GameObject currentTargetChild = null;

    protected override void Start()
    {
        base.Start();
        ResetKnife(); // đảm bảo khởi tạo đúng trạng thái
    }

    protected override void Update()
    {
        base.Update();
        if (!isPlaced) return;

        if (!Input.GetMouseButtonDown(0)) return;
        // 👉 hiệu ứng dao chém lên xuống
        //DoCutAnimation();

        if (currentTargetChild == null) AcquireTargetAtBoard();
        if (currentTargetChild == null) return;

        currentCutOffset += cutStep;
        transform.position = placedPosition + new Vector3(currentCutOffset, 0f, 0f);

        if (currentCutOffset >= maxCutDistance)
        {
            bool handled = false;
            var cutManager = currentTargetParent?.GetComponent<CarrotCutManager>();
            if (cutManager != null)
            {
                currentTargetChild.SetActive(false);
                cutManager.CutObjectSlice();
                handled = true;
            }

            if (!handled)
            {
                currentTargetChild.SetActive(false);
            }

            ResetKnife();
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
        currentCutOffset = 0f;

        // đổi sang dao dọc + bật mask
        knifeHorizontal.SetActive(false);
        knifeVertical.SetActive(true);
        knifeMask.SetActive(true);

        AcquireTargetAtBoard();
    }

    protected override void OnDropFail()
    {
        ResetKnife();
    }

    private void ResetKnife()
    {
        // reset trạng thái dao về ngang
        isPlaced = false;
        currentCutOffset = 0f;
        currentTargetChild = null;
        currentTargetParent = null;

        transform.position = startPoint.position;

        knifeHorizontal.SetActive(true);
        knifeVertical.SetActive(false);
        knifeMask.SetActive(false);
    }

    private void AcquireTargetAtBoard()
    {
        currentTargetChild = null;
        currentTargetParent = null;

        Collider2D[] hits = Physics2D.OverlapCircleAll(placedPosition, pickRadius);
        if (hits == null || hits.Length == 0) return;

        SpriteRenderer chosenSR = null;
        GameObject chosenChild = null;

        foreach (var h in hits)
        {
            if (!h.CompareTag("Cuttable")) continue;

            SpriteRenderer sr = h.GetComponent<SpriteRenderer>();
            if (sr == null) continue;

            if (chosenSR == null || sr.sortingOrder > chosenSR.sortingOrder)
            {
                chosenSR = sr;
                chosenChild = h.gameObject;
            }
        }

        if (chosenChild != null)
        {
            currentTargetChild = chosenChild;
            currentTargetParent = chosenChild.transform.parent;
        }
    }
    // private void DoCutAnimation()
    // {
    //     // nhún xuống rất nhẹ
    //     Vector3 downPos = transform.position + Vector3.down * 0.05f;

    //     transform.DOMove(downPos, 0.12f)
    //         .SetEase(Ease.InOutSine)
    //         .OnComplete(() =>
    //         {
    //             transform.DOMove(placedPosition + new Vector3(currentCutOffset, 0f, 0f), 0.12f)
    //                     .SetEase(Ease.InOutSine);
    //         });
    // }
}
