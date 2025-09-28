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
    private bool wasSnapped = false;

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
        HandleWaterState();
    }
    private void ToggleWater()
    {
        isOn = !isOn;
        waterTop.SetActive(isOn);
    }
    private void HandleWaterState()
    {
        if (isOn && blockWater.isSnapped)
        {
            // Vòi bật + nắp đóng -> nước chảy
            water.SetActive(true);
            waterFalse.SetActive(false);
            wasSnapped = true;
        }
        else if (!blockWater.isSnapped)
        {
            // Vòi bật + nắp mở
            if (wasSnapped) // chỉ khi trước đó có đóng nắp
            {
                StartCoroutine(ShowWaterFalse());
                wasSnapped = false;
            }

            water.SetActive(false);
        }
    }
    private IEnumerator ShowWaterFalse()
    {
        waterFalse.SetActive(true);
        yield return new WaitForSeconds(1.5f); // hiệu ứng nước rút 0.5s
        waterFalse.SetActive(false);
    }
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = Input.mousePosition;
        vec.z = 10f;
        return Camera.main.ScreenToWorldPoint(vec);
    }
}
