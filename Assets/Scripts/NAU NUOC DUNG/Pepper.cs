using UnityEngine;
using DG.Tweening;
using System.Collections;

public class Pepper : DraggableBase
{
    [SerializeField] CountOBJ countOBJ;
    [SerializeField] private Transform pointClay;
    [SerializeField] private Transform potPoint;        // Vị trí nồi
    [SerializeField] private Transform spawnPoint;      // Miệng hộp
    [SerializeField] private GameObject pepperGroupPrefab; // Prefab cả nhóm hạt tiêu

    private bool isShaking = false;

    protected override bool CheckCorrectDropZone()
    {
        return Vector2.Distance(transform.position, pointClay.position) <= 2f;
    }

    protected override void OnDropSuccess()
    {
        StartCoroutine(PourPepper());
    }

    protected override void OnDropFail()
    {
        base.OnDropFail();
    }

    private IEnumerator PourPepper()
    {
        isShaking = true;

        // Snap hộp tiêu vào vị trí nồi
        transform.position = pointClay.position;

        // Nghiêng hộp 1 chút
        transform.DORotate(new Vector3(0, 0, -35f), 0.3f).SetEase(Ease.OutBack);

        yield return new WaitForSeconds(0.3f);

        // 👉 Spawn nguyên cụm hạt tiêu
        GameObject pepperGroup = Instantiate(pepperGroupPrefab, potPoint.position, Quaternion.identity);

        // (Optional) hiệu ứng scale cho cả cụm tiêu khi spawn
        pepperGroup.transform.localScale = Vector3.zero;
        pepperGroup.transform.DOScale(0.7f, 0.5f).SetEase(Ease.OutBack);

        // 👉 Thêm vào mảng ingredients của Cooking script
        var cooking = FindFirstObjectByType<Cooking>();
        if (cooking != null)
        {
            countOBJ.obIndex++;
        }

        // Xong rồi trả hộp về
        yield return new WaitForSeconds(0.5f);
        transform.DORotate(Vector3.zero, 0.3f).SetEase(Ease.OutBack);

        // Reset về vị trí ban đầu
        Vector3 worldStartPos = transform.parent.TransformPoint(startLocalPosition);
        transform.DOMove(worldStartPos, 0.5f).SetEase(Ease.InOutQuad);

        isShaking = false;
        // 👉 Chỉ cho đổ 1 lần
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }
}
