using UnityEngine;
using UnityEditor;

public class TextureReimportTool : MonoBehaviour
{
    [MenuItem("Tools/Fix Missing Sprite Materials")]
    public static void FixAllSpriteRenderers()
    {
        if (!EditorUtility.DisplayDialog("Fix Missing Materials",
            "Tự động gán Default-SpriteMaterial cho mọi SpriteRenderer bị Missing. Bạn có chắc không?",
            "Yes", "No"))
        {
            return;
        }

        int fixedCount = 0;

        // Quét toàn bộ SpriteRenderer trong scene
        SpriteRenderer[] renderers = GameObject.FindObjectsOfType<SpriteRenderer>(true);
        foreach (var sr in renderers)
        {
            if (sr.sharedMaterial == null) // bị Missing
            {
                sr.sharedMaterial = AssetDatabase.GetBuiltinExtraResource<Material>("Sprites-Default.mat");
                fixedCount++;
                EditorUtility.SetDirty(sr);
            }
        }

        AssetDatabase.SaveAssets();
        EditorUtility.DisplayDialog("Done",
            $"Đã sửa {fixedCount} SpriteRenderer bị thiếu material.",
            "OK");
    }
}
