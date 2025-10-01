using UnityEngine;

public class DestroyEventOnion : MonoBehaviour
{
    [SerializeField] LeafDrag leafDrag;

    public void StartAnimEvent()
    {
        leafDrag.animator.SetBool("LeafPosition", true);
        Destroy(leafDrag.gameObject);
    }
}
