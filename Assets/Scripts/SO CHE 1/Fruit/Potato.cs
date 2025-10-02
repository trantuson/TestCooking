using UnityEngine;
using DG.Tweening;

public class Potato : DraggableBase
{
    [SerializeField] private Transform potato;   // vị trí drop
    private DropEffect dropEffect;
    private Quaternion startRotation;            // xoay ban đầu (dọc)

    [Header("Rotation Settings")]
    public Vector3 droppedEuler = new Vector3(0, 0, 90f); // xoay ngang khi drop
    public float rotateDuration = 0.25f;

    void Awake()
    {
        dropEffect = GetComponent<DropEffect>();
        startRotation = transform.rotation; // lưu xoay ban đầu
    }

    protected override bool CheckCorrectDropZone()
    {
        return Vector2.Distance(transform.position, potato.position) <= 0.5f;
    }

    protected override void OnDropSuccess()
    {
        transform.position = potato.position;

        // Xoay ngang
        transform.DORotate(droppedEuler, rotateDuration).SetEase(Ease.OutBack);
    }

    protected override void OnDropFail()
    {
        // Trả về vị trí + rotation ban đầu
        transform.position = transform.parent.TransformPoint(startLocalPosition);
        transform.rotation = startRotation;
    }
}
