using UnityEngine;
using DG.Tweening;

public class PeelableObject : MonoBehaviour
{
    public SpriteRenderer peelRenderer;
    public float cutPercentToFinish = 0.99f; // % pixel cần gọt để coi là xong
    public Color peelParticleColor = Color.yellow; // màu vỏ bắn ra
    [SerializeField] private GameObject setFalse; // got xong goi
    [SerializeField] private GameObject setTrue; // got xon goi
    [HideInInspector] public Texture2D peelTexture;
    private int totalPixels;
    private int clearedPixels;
    public bool isFinished = false;

    private bool textureDirty = false; // flag: có pixel nào vừa được sửa

    void Start()
    {
        // Clone texture để không ảnh hưởng sprite gốc
        Texture2D tex = peelRenderer.sprite.texture;
        peelTexture = new Texture2D(tex.width, tex.height, TextureFormat.RGBA32, false);
        Graphics.CopyTexture(tex, peelTexture);
        peelTexture.Apply();

        peelRenderer.sprite = Sprite.Create(peelTexture, peelRenderer.sprite.rect, new Vector2(0.5f, 0.5f));

        // Tính tổng số pixel ban đầu để biết khi nào gọt xong
        totalPixels = 0;
        foreach (var c in peelTexture.GetPixels())
        {
            if (c.a > 0.9f) totalPixels++;
        }
    }

    void LateUpdate()
    {
        // Chỉ Apply 1 lần mỗi frame nếu có pixel thay đổi
        if (textureDirty)
        {
            peelTexture.Apply();
            textureDirty = false;
        }
    }

    // gọi mỗi khi cắt
    public void CutAtPosition(Vector3 worldPos, float cutRadius)
    {
        if (isFinished) return;

        Vector2 localPos = peelRenderer.transform.InverseTransformPoint(worldPos);

        Sprite sprite = peelRenderer.sprite;
        Rect rect = sprite.rect;
        float px = (localPos.x + sprite.bounds.extents.x) / sprite.bounds.size.x * rect.width;
        float py = (localPos.y + sprite.bounds.extents.y) / sprite.bounds.size.y * rect.height;

        int x = Mathf.RoundToInt(px);
        int y = Mathf.RoundToInt(py);
        int radius = Mathf.RoundToInt(cutRadius);

        for (int i = -radius; i <= radius; i++)
        {
            for (int j = -radius; j <= radius; j++)
            {
                int tx = x + i;
                int ty = y + j;
                if (tx >= 0 && tx < peelTexture.width && ty >= 0 && ty < peelTexture.height)
                {
                    if (i * i + j * j <= radius * radius)
                    {
                        Color c = peelTexture.GetPixel(tx, ty);
                        if (c.a > 0.1f) clearedPixels++;
                        c.a = 0f;
                        peelTexture.SetPixel(tx, ty, c);
                        textureDirty = true; // đánh dấu có thay đổi
                    }
                }
            }
        }

        float percent = (float)clearedPixels / totalPixels;
        if (percent >= cutPercentToFinish)
        {
            isFinished = true;
            OnPeelingFinished();
        }
    }

    private void OnPeelingFinished()
    {
        Debug.Log(name + "đã gọt xong!");
        setFalse.SetActive(false);
        setTrue.SetActive(true);
    }
}


