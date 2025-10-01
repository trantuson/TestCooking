using UnityEngine;

public class LeafDrag : MonoBehaviour
{
    public Animator animator;
    private Onion onionParent;
    private Vector3 mousePosition;
    private Vector3 originalPosition;
    [SerializeField] private float distanceMouse = 2f;
    [SerializeField] private float speed = 0.2f;
    Vector3 offset;
    [SerializeField] private Transform pointLeafTarget;
    void Start()
    {
        animator = GetComponent<Animator>();
        onionParent = GetComponentInParent<Onion>();
    }
    private void OnMouseDown()
    {
        mousePosition = GetMousePos();
        offset = transform.position - mousePosition;
        originalPosition = transform.position;
    }
    void OnMouseDrag()
    {
        // if (onionParent.stage >= 2)
        // {
        //     animator.SetBool("LeafScale", true);
        // }
        animator.SetBool("LeafScale", true);
    }
    void OnMouseUp()
    {
        animator.SetBool("LeafScale", false);
    }
    private Vector3 GetMousePos()
    {
        Vector3 pos = Input.mousePosition;
        pos.z = Mathf.Abs(Camera.main.transform.position.z);
        return Camera.main.ScreenToWorldPoint(pos);
    }
    public void StartAnimEvent()
    {
        animator.SetBool("LeafPosition", true);
        Destroy(gameObject,2f);
    }
}
