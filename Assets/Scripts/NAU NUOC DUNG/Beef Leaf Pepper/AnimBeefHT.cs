using UnityEngine;

public class AnimBeefHT : MonoBehaviour
{
    [SerializeField] Animator animator;

    public void OnAnimBeefHT()
    {
        animator.SetTrigger("BeefHTY");
    }
    public void AnimaEventBeef()
    {
        animator.SetTrigger("BeefHTY");
    }
    public void AnimatorFalse()
    {
        animator.enabled = false;
    }
}
