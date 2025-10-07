using UnityEngine;
using DG.Tweening;
using System.Collections;

public class WaterJug : DraggableBase
{
    [SerializeField] private Transform pointPan;
    [SerializeField] private SpriteRenderer water;
    [SerializeField] private Sprite waterNgang;
    [SerializeField] private Sprite waterStand; // sprite bình đứng
    [SerializeField] private GameObject waterPan;

    [SerializeField] private GameObject waterStream; // sprite nước chảy

    protected override bool CheckCorrectDropZone()
    {
        // Kiểm tra khoảng cách
        return Vector2.Distance(transform.position, pointPan.position) < 2f;
    }

    protected override void OnDropSuccess()
    {
        StartCoroutine(PourRoutine());
    }

    private IEnumerator PourRoutine()
    {
        // Di chuyển vào vị trí rót
        transform.DOMove(pointPan.position, 0.4f).SetEase(Ease.OutSine);

        water.sprite = waterNgang; // Đổi sprite sang tư thế rót
        water.flipX = true; // Lật sprite sang trái

        yield return new WaitForSeconds(1f);
        waterStream.SetActive(true);
        waterPan.SetActive(true);

        // Giữ tư thế rót trong vài giây
        yield return new WaitForSeconds(2f);

        // Đổi lại sprite đứng
        water.sprite = waterStand;
        waterStream.SetActive(false);

        // Xoay thẳng lại
        transform.DORotate(Vector3.zero, 0.3f).SetEase(Ease.OutSine);

        // Quay về vị trí ban đầu
        Vector3 startWorldPos = transform.parent.TransformPoint(startLocalPosition);
        transform.DOMove(startWorldPos, 0.5f).SetEase(Ease.InOutQuad);

        // Ngắt drag để không rót lại liên tục
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }

    protected override void OnDropFail()
    {
        base.OnDropFail();
    }
}
