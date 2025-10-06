using UnityEngine;
using System.Linq;

public class LevelChecker : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;

    [Header("Các object cần bật khi hoàn thành")]
    [SerializeField] private GameObject[] mustBeActiveObjects;

    [Header("Các object cần tắt khi hoàn thành (ví dụ rác, công cụ, vỏ...)")]
    [SerializeField] private GameObject[] mustBeInactiveObjects;

    private bool isCompleted;

    private void Update()
    {
        if (isCompleted) return;

        // Kiểm tra object cần bật: tất cả đều active
        bool allActiveDone = mustBeActiveObjects != null && mustBeActiveObjects.All(obj => obj != null && obj.activeSelf);

        // Kiểm tra object cần tắt: tất cả đều null hoặc inactive
        bool allInactiveDone = mustBeInactiveObjects != null && mustBeInactiveObjects.All(obj => obj == null || !obj.activeSelf);

        if (allActiveDone && allInactiveDone)
        {
            // Gọi chỉ 1 lần duy nhất
            isCompleted = true;

            Debug.Log("✅ Màn đã hoàn thành → Qua màn kế tiếp!");
            levelManager.NextLevel();
        }
    }
}
