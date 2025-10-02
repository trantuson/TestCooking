using UnityEngine;

public class CheckCaoOnion : MonoBehaviour
{
    Animator animator;
    [SerializeField] private GameObject onionDaGotXong;
    [SerializeField] private GameObject destroyNao;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Nao"))
        {
            animator.SetTrigger("Peel");
            onionDaGotXong.SetActive(true);
            Destroy(destroyNao,4f);
        }
    }
}
