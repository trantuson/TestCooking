using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrapDropOnion : MonoBehaviour
{
    [SerializeField] Animator animatorOnion;
    [SerializeField] Animator animatorGround;
    [SerializeField] private Transform pointTarget;
    [SerializeField] private float speed;
    private Vector3 originalPosition;

    private bool isNextLevelTriggered = false;
    private static int currentOnionIndex;

    private Vector3 mousePosition;
    [SerializeField] private float distanceMouse;

    private bool isCollide = false;

    private void OnMouseDown()
    {
        animatorOnion.SetBool("OnionShake", true);
        originalPosition = transform.position;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    private void OnMouseDrag()
    {
        Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (currentMousePosition.y - mousePosition.y > distanceMouse)
        {
            Vector3 newPos = Vector3.MoveTowards(transform.position, pointTarget.position, Time.deltaTime * speed);
            transform.position = newPos;
            animatorOnion.SetBool("OnionScale", true);
        }
    }
    private void OnMouseUp()
    {
        animatorOnion.SetBool("OnionScale", false);
        animatorOnion.SetBool("OnionShake", false);
        if (isCollide == false)
        {
            transform.position = originalPosition;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PosPointOnion"))
        {
            animatorOnion.SetBool("OnionRota", true);
            animatorGround.SetTrigger("Ground");

            isCollide = true;

            currentOnionIndex++;

            if (currentOnionIndex >= 4 && !isNextLevelTriggered)
            {
                isNextLevelTriggered = true;
                StartCoroutine(NextLevelAfterDelay(3f));
            }
        }
    }
    private IEnumerator NextLevelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        FindFirstObjectByType<LevelManager>().NextLevel();
    }
}
