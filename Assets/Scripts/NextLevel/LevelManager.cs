using DG.Tweening;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Transform levelContainer; // Gán object chứa các màn (ví dụ: LevelContainer)
    [SerializeField] private float moveDuration = 1f;  // Thời gian tween giữa các màn
    [SerializeField] private float levelWidth = 10f;   // Khoảng cách giữa mỗi màn
    private int currentLevelIndex = 0;

    private Vector3 originalLocalPos;

    private void Start()
    {
        if (levelContainer == null)
        {
            Debug.LogError("LevelManager: levelContainer chưa được gán trong Inspector!");
            return;
        }

        originalLocalPos = levelContainer.localPosition;
    }

    public void NextLevel()
    {
        if (levelContainer == null) return;

        currentLevelIndex++;

        // Vị trí mới tính theo local (mỗi màn cách nhau levelWidth)
        Vector3 targetPos = new Vector3(-currentLevelIndex * levelWidth, 0, 0);

        // Tween chuyển màn
        levelContainer.DOLocalMove(targetPos, moveDuration)
            .SetEase(Ease.InOutQuad)
            .OnStart(() => Debug.Log($"▶ Chuyển sang Level {currentLevelIndex}"))
            .OnComplete(() => Debug.Log("✅ Hoàn thành tween chuyển màn"));

        // Hiệu ứng zoom nhẹ camera (nếu có camera chính)
        if (Camera.main != null)
        {
            Camera.main.DOOrthoSize(3f, 0.5f)
                .OnComplete(() => Camera.main.DOOrthoSize(5f, 0.5f));
        }
        else
        {
            Debug.LogWarning("⚠️ Không tìm thấy Camera chính (MainCamera). Bỏ qua hiệu ứng zoom.");
        }
    }

    public void ResetGame()
    {
        currentLevelIndex = 0;

        if (levelContainer != null)
            levelContainer.localPosition = originalLocalPos;

        Debug.Log("🔄 Reset về màn đầu tiên");
    }
}
