using UnityEngine;

public class CarrotCutManager : MonoBehaviour
{
    [SerializeField] private GameObject Potato1;
    [SerializeField] private GameObject Potato2;
    public void CutObjectSlice()
    {
        Potato1.SetActive(true);
        Potato2.SetActive(false);
    }
}
