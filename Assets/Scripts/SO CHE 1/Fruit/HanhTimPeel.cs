using UnityEngine;

public class OnionPeelManager : MonoBehaviour
{
    [SerializeField] private SpriteRenderer onionRenderer;
    [SerializeField] private Sprite[] peelStages; // [0] nguyên, [n] bóc sạch

    private int currentStage = 0;
    private Vector3 lastMousePos;
    private bool dragging = false;

    void Update()
    {
        // CLICK: mỗi lần click tăng stage
        if (Input.GetMouseButtonDown(0))
        {
            NextPeelStage();
        }

        // DRAG: nếu kéo xuống thì tăng stage
        if (Input.GetMouseButtonDown(0))
        {
            dragging = true;
            lastMousePos = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            dragging = false;
        }
        if (dragging)
        {
            Vector3 delta = Input.mousePosition - lastMousePos;
            if (delta.y < -30f) // kéo xuống đủ xa
            {
                NextPeelStage();
                lastMousePos = Input.mousePosition;
            }
        }
    }

    private void NextPeelStage()
    {
        if (currentStage < peelStages.Length - 1)
        {
            currentStage++;
            onionRenderer.sprite = peelStages[currentStage];
        }
    }
}
