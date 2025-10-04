using UnityEngine;

public class AnimPepper : MonoBehaviour
{
    [SerializeField] Animator animator;

    public void OnAnimPepper()
    {
        animator.SetTrigger("Pepper");
    }
}
