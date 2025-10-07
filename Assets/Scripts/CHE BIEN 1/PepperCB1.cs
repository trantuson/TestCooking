using UnityEngine;
using DG.Tweening;
using System.Collections;

public class PepperCB1 : DraggableBase
{
    [SerializeField] private Transform pointClay;
    [SerializeField] private GameObject pepperGroupPrefab; // Prefab cả nhóm hạt tiêu

    private Vector3 worldStartPos; //Lưu vị trí gốc theo world
    protected override void Start()
    {
        base.Start();
        worldStartPos = transform.position; // lưu vị trí ban đầu
    }
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
        // Snap hộp tiêu vào vị trí nồi
        transform.position = pointClay.position;

        // Nghiêng hộp 1 chút
        transform.DORotate(new Vector3(0, 0, -35f), 0.3f).SetEase(Ease.OutBack);

        yield return new WaitForSeconds(0.3f);

        pepperGroupPrefab.SetActive(true);

        // Xong rồi trả hộp về
        yield return new WaitForSeconds(0.5f);
        transform.DORotate(Vector3.zero, 0.3f).SetEase(Ease.OutBack);
        transform.DOMove(worldStartPos, 0.6f).SetEase(Ease.InOutQuad);

        // 👉 Chỉ cho đổ 1 lần
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }
}
