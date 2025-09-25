using System.Collections;
using TMPro;
using UnityEngine;

public class OnOffWater : MonoBehaviour
{
    [SerializeField] BlockWater blockWater;

    [SerializeField] private GameObject waterTop;
    [SerializeField] private GameObject water;
    [SerializeField] private GameObject waterFalse;

    private bool isOn = false;

    private void Start()
    {
        waterTop.SetActive(false);
        water.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = GetMouseWorldPosition();
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (hit.collider != null && hit.collider.CompareTag("IsOn"))
            {
                ToggleWater();
            }
        }
        if(blockWater.isBlocked == true && isOn == true)
        {
            water.SetActive(true);
        }
        else if(blockWater.isBlocked == false)
        {
            water.SetActive(false);
        }
        //if(blockWater.isBlocked == false && blockWater.checkBlock == true)
        //{
        //    water.SetActive(false);
        //    waterFalse.SetActive(true);
        //}
    }
    private void ToggleWater()
    {
        isOn = !isOn;
        waterTop.SetActive(isOn);
    }
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = Input.mousePosition;
        vec.z = 10f;
        return Camera.main.ScreenToWorldPoint(vec);
    }
}
