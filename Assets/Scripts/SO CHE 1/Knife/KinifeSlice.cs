using UnityEngine;

public class KnifeSlice : MonoBehaviour
{
    [SerializeField] private GameObject wholeObject;   // củ nguyên
    [SerializeField] private GameObject slicedPrefab;  // prefab hình cắt khúc
    [SerializeField] private GameObject debrisPrefab;  // prefab vụn (nếu có)
    [SerializeField] private string cuttableTag = "CutSlice";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(cuttableTag))
        {
            // Ẩn object nguyên
            wholeObject.SetActive(false);

            // Spawn object cắt khúc tại đúng vị trí
            Instantiate(slicedPrefab, collision.transform.position, Quaternion.identity);

            // Nếu có vụn thì spawn thêm
            if (debrisPrefab != null)
            {
                Instantiate(debrisPrefab, collision.transform.position, Quaternion.identity);
            }
        }
    }
}

