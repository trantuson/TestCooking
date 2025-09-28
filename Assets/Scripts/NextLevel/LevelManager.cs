using DG.Tweening;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Transform levelContainer; // drag LevelContainer vào
    [SerializeField] private float moveDuration = 1f;  // thời gian di chuyển giữa màn
    [SerializeField] private float levelWidth = 10f;   // khoảng cách giữa các màn
    private int currentLevelIndex = 0;

    public void NextLevel()
    {
        currentLevelIndex++;
        Vector3 targetPos = new Vector3(-currentLevelIndex * levelWidth, 0, 0);

        levelContainer.DOMove(targetPos, moveDuration)
            .SetEase(Ease.InOutQuad);
        Camera.main.DOOrthoSize(3f, 1f).OnComplete(() =>
        {
            Camera.main.DOOrthoSize(5f, 1f);
        });
    }
    public void ResetGame()
    {
        currentLevelIndex = 0;
        levelContainer.position = Vector3.zero;
    }
}
