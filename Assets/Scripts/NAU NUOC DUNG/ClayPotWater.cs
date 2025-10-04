using UnityEngine;

public class ClayPotWater : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public OnOffWater onOffWater;
    public DragDropManager dragDropManager;
    [SerializeField] private GameObject water;

    public bool isWaterTrue = false;
    void Update()
    {
        ClayPotHaveWater();
    }
    private void ClayPotHaveWater()
    {
        if (onOffWater.isOn == true && dragDropManager.isPointTarget == true)
        {
            isWaterTrue = true;
            water.SetActive(true);
            animator.SetTrigger("OnWaterClay");
        }
    }
}
