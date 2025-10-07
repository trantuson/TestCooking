using UnityEngine;

public class Tomato : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite tomatoSprite;

    public bool CanBeScooped { get; private set; } = false;
    private void OnMouseDown()
    {
        spriteRenderer.sprite = tomatoSprite;
        CanBeScooped = true;
    }
}
