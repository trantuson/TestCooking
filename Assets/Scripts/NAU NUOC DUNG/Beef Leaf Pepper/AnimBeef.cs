using UnityEngine;

public class AnimBeef : MonoBehaviour
{
    [SerializeField] Animator animator;

    public void OnAnimBeef()
    {
        animator.SetTrigger("Beef");
    }
}
