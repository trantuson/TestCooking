using UnityEngine;
using DG.Tweening;

public class DOTweenCleaner : MonoBehaviour
{
    [SerializeField] private float cleanupInterval = 2f; // mỗi 2s dọn rác 1 lần

    void Awake()
    {
        DOTween.Init();
        DOTween.SetTweensCapacity(5000, 200); // đủ lớn cho cả scene
    }
    private void Start()
    {
        InvokeRepeating(nameof(CleanupTweens), cleanupInterval, cleanupInterval);
    }

    private void CleanupTweens()
    {
        // Xóa toàn bộ tween đã chết hoặc target null
        DOTween.KillAll(false);
        DOTween.ClearCachedTweens();
    }

    private void OnDestroy()
    {
        CancelInvoke();
        DOTween.KillAll();
    }
}
