using UnityEngine;

public class FruitCut : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; // sprite cần cắt
    public float cutRadius = 0.1f;        // bán kính vùng cắt

    private Texture2D texture;            // texture runtime
    private int texWidth, texHeight;






    void Start()
    {
        // Copy texture từ sprite để sửa runtime
        Texture2D src = spriteRenderer.sprite.texture;
        texWidth = src.width;
        texHeight = src.height;

        texture = new Texture2D(texWidth, texHeight, TextureFormat.RGBA32, false);
        texture.SetPixels(src.GetPixels());
        texture.Apply();

        // Gán texture mới vào sprite
        spriteRenderer.sprite = Sprite.Create(texture,
            new Rect(0, 0, texWidth, texHeight),
            new Vector2(0.5f, 0.5f));
    }

    // Hàm cut tại vị trí world
    //public void CutAt(Vector2 worldPos)
    //{
    //    Vector2 localPos = transform.InverseTransformPoint(worldPos); // world -> local
    //    int px = Mathf.RoundToInt((localPos.x + 0.5f) * texWidth);
    //    int py = Mathf.RoundToInt((localPos.y + 0.5f) * texHeight);

    //    int r = Mathf.RoundToInt(cutRadius * texWidth);

    //    for (int x = -r; x <= r; x++)
    //    {
    //        for (int y = -r; y <= r; y++)
    //        {
    //            int cx = px + x;
    //            int cy = py + y;

    //            if (cx < 0 || cx >= texWidth || cy < 0 || cy >= texHeight)
    //                continue;

    //            if (Mathf.Sqrt(x * x + y * y) <= r)
    //            {
    //                Color c = texture.GetPixel(cx, cy);
    //                c.a = 0f; // xóa alpha
    //                texture.SetPixel(cx, cy, c);
    //            }
    //        }
    //    }

    //    texture.Apply();
    //}


    //public void CutBetween(Vector2 startWorld, Vector2 endWorld)
    //{
    //    Vector2 startLocal = transform.InverseTransformPoint(startWorld);
    //    Vector2 endLocal = transform.InverseTransformPoint(endWorld);

    //    int startX = Mathf.RoundToInt((startLocal.x + 0.5f) * texWidth);
    //    int startY = Mathf.RoundToInt((startLocal.y + 0.5f) * texHeight);
    //    int endX = Mathf.RoundToInt((endLocal.x + 0.5f) * texWidth);
    //    int endY = Mathf.RoundToInt((endLocal.y + 0.5f) * texHeight);

    //    int r = Mathf.RoundToInt(cutRadius * texWidth);

    //    // Tính vector đường cắt
    //    Vector2 dir = new Vector2(endX - startX, endY - startY);
    //    float len = dir.magnitude;
    //    dir.Normalize();

    //    // Duyệt toàn bộ pixel, kiểm tra khoảng cách tới đường thẳng
    //    for (int x = 0; x < texWidth; x++)
    //    {
    //        for (int y = 0; y < texHeight; y++)
    //        {
    //            Vector2 p = new Vector2(x, y);
    //            Vector2 toPixel = p - new Vector2(startX, startY);
    //            float proj = Vector2.Dot(toPixel, dir); // chiếu lên đường
    //            Vector2 closest;

    //            if (proj < 0) closest = new Vector2(startX, startY);
    //            else if (proj > len) closest = new Vector2(endX, endY);
    //            else closest = new Vector2(startX, startY) + dir * proj;

    //            float dist = Vector2.Distance(p, closest);
    //            if (dist <= r)
    //            {
    //                Color c = texture.GetPixel(x, y);
    //                c.a = 0f;
    //                texture.SetPixel(x, y, c);
    //            }
    //        }
    //    }

    //    texture.Apply();
    //}

    public void CutHorizontalAt(Vector2 worldPos)
    {
        // Chuyển từ world -> local của sprite
        Vector2 localPos = transform.InverseTransformPoint(worldPos);

        // Tính tọa độ pixel trên texture
        int py = Mathf.RoundToInt((localPos.y + 0.5f) * texHeight);

        int r = Mathf.RoundToInt(cutRadius * texHeight); // độ dày đường cắt

        for (int x = 0; x < texWidth; x++)
        {
            for (int y = 0; y < texHeight; y++)
            {
                if (Mathf.Abs(y - py) <= r)
                {
                    Color c = texture.GetPixel(x, y);
                    c.a = 0f; // xóa pixel
                    texture.SetPixel(x, y, c);
                }
            }
        }

        texture.Apply();
    }
}
