using UnityEngine;
using DG.Tweening;

public class OnionPeelManager : MonoBehaviour
{
    [SerializeField] GameObject lan1;
    [SerializeField] GameObject lan2;
    private int clickCount = 0;
    // Hàm này bạn gọi khi bấm nút
    private void OnMouseDown()
    {
        clickCount++;

        if (clickCount == 5)
        {
            // Bật Lan1
            lan1.SetActive(true);

            // Hiệu ứng scale nảy lên
            lan1.transform.localScale = Vector3.zero;
            lan1.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        }

        if (clickCount == 10)
        {
            lan2.SetActive(false);
        }
    }
}
