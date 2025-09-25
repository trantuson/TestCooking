using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;

public class WashOnion : MonoBehaviour
{
    private Vector3 originalPosition;
    private Vector3 offset;
    private bool isDragging = false;
    private bool isSticky = true;
    private bool isInWater = false;

    [SerializeField] private Transform destroyTarget;

    [SerializeField] private float shakeAmount = 0.05f;
    private void Start()
    {
        originalPosition = transform.position;
    }
    private void Update()
    {
        DrapDrop();
        if(isSticky == true && isInWater == true)
        {
            transform.localPosition = originalPosition + (Vector3)Random.insideUnitSphere * shakeAmount;
        }
    }
    private void DrapDrop()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = GetMouse();
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (hit.collider != null && hit.collider.CompareTag("Onion"))
            {
                offset = mousePos - transform.position;
                isDragging = true;
            }
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = GetMouse();
            if(isDragging)
                transform.position = mousePos - offset;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            transform.position = originalPosition;
        }
    }
    private Vector3 GetMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            isInWater = true;
            isDragging = false;
            originalPosition = new Vector3(-1.98f, 0.14f, 0);
            StartCoroutine(TimeCheck());
        }
        if (collision.CompareTag("CuttingBoard") && isSticky == false)
        {
            isDragging = false;
            originalPosition = new Vector3(-1.75f, -2.3f, 0);
        }
    }
    private IEnumerator TimeCheck()
    {
        yield return new WaitForSeconds(3f);
        if (destroyTarget != null)
        {
            Destroy(destroyTarget.gameObject);
        }
        isSticky = false;
    }
}
