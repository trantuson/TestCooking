using UnityEngine;
using DG.Tweening;
using System.Collections;

public class OIL : DraggableBase
{
    [SerializeField] private Transform pointPan;  // Vị trí cái chảo
    [SerializeField] private GameObject oil2;     // Hiệu ứng dầu nhỏ
    [SerializeField] private GameObject oil3;     // Hiệu ứng dầu to hơn

    private bool isPouring = false;

    protected override bool CheckCorrectDropZone()
    {
        return Vector2.Distance(transform.position, pointPan.position) < 2.0f;
    }

    protected override void OnDropSuccess()
    {
        if (isPouring) return;
        StartCoroutine(PourOilRoutine());
    }

    protected override void OnDropFail()
    {
        base.OnDropFail();

        // Ẩn hiệu ứng khi kéo sai chỗ
        oil2.SetActive(false);
        oil3.SetActive(false);
    }

    private IEnumerator PourOilRoutine()
    {
        isPouring = true;

        // Xoay nghiêng để "rót"
        transform.DOMove(pointPan.position, 0.4f).SetEase(Ease.OutSine);
        transform.DORotate(new Vector3(0, 0, 45f), 0.4f).SetEase(Ease.OutBack);

        yield return new WaitForSeconds(0.4f);

        // Bật hiệu ứng dầu chảy
        oil2.SetActive(true);
        oil3.SetActive(true);

        // Rót trong 1 giây
        yield return new WaitForSeconds(1f);

        // Tắt hiệu ứng
        oil2.SetActive(false);

        // Nghiêng ngược lại (trở về thẳng)
        transform.DORotate(Vector3.zero, 0.4f).SetEase(Ease.OutBack);

        // Quay về vị trí ban đầu
        Vector3 worldStartPos = transform.parent.TransformPoint(startLocalPosition);
        transform.DOMove(worldStartPos, 0.5f).SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                isPouring = false;

                // Tắt kéo thả sau khi đã rót
                GetComponent<Collider2D>().enabled = false;
                this.enabled = false;
            });
    }
}
