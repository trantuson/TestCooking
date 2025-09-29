using UnityEngine;

public class LeafDrag : MonoBehaviour
{
    private Animator animator;
    private Onion onionParent;
    private Vector3 mousePosition;
    private Vector3 originalPosition;
    [SerializeField] private float distanceMouse = 2f;
    [SerializeField] private float speed = 0.2f;

    [SerializeField] private Transform pointLeafTarget;
    void Start()
    {
        animator = GetComponent<Animator>();
        onionParent = GetComponentInChildren<Onion>();
    }
    private void OnMouseDown()
    {
        originalPosition = transform.position;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    void OnMouseDrag()
    {
        if (onionParent.stage >= 2)
        {
            Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (currentMousePosition.y - mousePosition.y > distanceMouse)
            {
                Vector3 newPos = Vector3.MoveTowards(transform.position, pointLeafTarget.position, Time.deltaTime * speed);
                transform.position = newPos;
                animator.SetBool("LeafScale", true);
            }
        }
    }
    void OnMouseUp()
    {
        animator.SetBool("LeafScale", false);
        if (DistanceTarget() <= 1f)
        {
            Debug.Log("Da cham Distance");
        }
    }
    private float DistanceTarget()
    {
        return Vector2.Distance(transform.position, pointLeafTarget.position);
    }
}
