using UnityEngine;

public class AnimaLeaf : MonoBehaviour
{
    [SerializeField] Animator animator;

    public void OnAnimLeaf()
    {
        animator.SetTrigger("Leaf");
    }
}
