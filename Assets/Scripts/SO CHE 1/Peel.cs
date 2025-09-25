using UnityEngine;

public class Peel : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Sprite peeledSprite;
    [SerializeField] private float peelTime = 3f;

    private float timer = 0f;
    private bool isPeeled = false;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (isPeeled) return;

        if (other.CompareTag("Razor"))
        {
            timer += Time.deltaTime;

            if (timer >= peelTime)
            {
                sr.sprite = peeledSprite; // đổi ảnh
                isPeeled = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Razor"))
        {
            timer = 0f; // thoát dao thì reset
        }
    }
}
