using UnityEngine;
public class KnifeMaskFollow : DraggableBase
{
    [Header("Setup")]
    [SerializeField] private Transform PointBoard;
    [SerializeField] private float distanceCheck = 0.3f;

    [Header("Cutting")]
    [SerializeField] private float cutStep = 0.1f;
    [SerializeField] private float maxCutDistance = 1f;
    [SerializeField] private float pickRadius = 0.25f; // bán kính tìm object tại board

    // runtime
    private Vector3 placedPosition;
    private bool isPlaced = false;
    private float currentCutOffset = 0f;

    // target runtime
    private Transform currentTargetParent = null;
    private GameObject currentTargetChild = null;

    protected override void Update()
    {
        base.Update();

        if (!isPlaced) return; // nếu chưa đặt dao lên board thì không xử lý click cắt

        // chỉ xử lý click khi dao đang đặt
        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }
        // nếu chưa có child mục tiêu thì tìm
        if (currentTargetChild == null)
        {
            AcquireTargetAtBoard();
        }

        // nếu vẫn không có gì để cắt thì bỏ qua
        if (currentTargetChild == null)
        {
            // không có target để cắt
            return;
        }

        // tiến vết cắt
        currentCutOffset += cutStep;
        transform.position = placedPosition + new Vector3(currentCutOffset, 0f, 0f);
        
        if (currentCutOffset >= maxCutDistance)
        {
            bool handled = false;

            // Nếu object cha có CarrotCutManager (hoặc PotatoCutManager...), gọi luôn cắt khúc
            var cutManager = currentTargetParent?.GetComponent<CarrotCutManager>();
            if (cutManager != null)
            {
                currentTargetChild.SetActive(false);
                cutManager.CutObjectSlice();
                handled = true;
            }

            if (!handled)
            {
                // fallback: ẩn child top nhất như logic cũ
                currentTargetChild.SetActive(false);
            }

            // reset dao về vị trí board
            currentCutOffset = 0f;
            transform.position = placedPosition;

            // clear target
            currentTargetChild = null;
            currentTargetParent = null;

            return;
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

        // Lần đặt đầu tiên: tìm target sẵn sàng để cắt
        AcquireTargetAtBoard();
    }

    protected override void OnDropFail()
    {
        // quay về vị trí ban đầu
        transform.position = transform.parent != null ? transform.parent.TransformPoint(startLocalPosition) : startLocalPosition;
        isPlaced = false;
        currentCutOffset = 0f;
        currentTargetChild = null;
        currentTargetParent = null;
    }

    private void AcquireTargetAtBoard()
    {
        currentTargetChild = null;
        currentTargetParent = null;

        Collider2D[] hits = Physics2D.OverlapCircleAll(placedPosition, pickRadius);
        Debug.Log("Found hits: " + hits.Length);
        if (hits == null || hits.Length == 0) return;

        SpriteRenderer chosenSR = null;
        GameObject chosenChild = null;

        foreach (var h in hits)
        {
            // chỉ lấy object có tag "Cuttable"
            if (!h.CompareTag("Cuttable")) continue;

            SpriteRenderer sr = h.GetComponent<SpriteRenderer>();
            if (sr == null) continue;

            // chọn child có sortingOrder cao nhất (lớp trên cùng)
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

            Debug.Log("[KnifeMaskFollow] Target locked: " + currentTargetChild.name + " (parent: " + currentTargetParent.name + ")");
        }
    }

}