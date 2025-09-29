using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;
using System.Collections;

public class Onion : DraggableBase
{
    [SerializeField] private Transform PointWater;
    [SerializeField] private Transform PointCuttingBoard;
    [SerializeField] private Transform destroyTarget;
    [SerializeField] private float snapDistance = 0.5f;
    [SerializeField] private float shakeAmount = 0.05f;

    private Vector3 currentBasePosition;

    public int stage = 0;
    private bool isShaking = false;
    private float shakeTimer = 3f;
    protected override void Start()
    {
        base.Start();
        currentBasePosition = startLocalPosition;
    }

    protected override void Update()
    {
        base.Update();
        if (stage >= 2)
            return;
        if (isShaking)
        {
            if (shakeTimer > 0)
            {
                transform.position = currentBasePosition + (Vector3)Random.insideUnitCircle * shakeAmount;
                shakeTimer -= Time.deltaTime;
            }
            else
            {
                isShaking = false;
                if (destroyTarget != null)
                    Destroy(destroyTarget.gameObject);
            }
        }
    }

    protected override bool CheckCorrectDropZone()
    {
        if (stage == 0)
        {
            return Vector2.Distance(transform.position, PointWater.position) <= snapDistance;
        }
        else if (stage == 1)
        {
            return Vector2.Distance(transform.position, PointCuttingBoard.position) <= snapDistance;
        }
        return false;
    }

    protected override void OnDropSuccess()
    {
        if (stage == 0)
        {
            transform.position = PointWater.position;
            currentBasePosition = PointWater.position;
            stage = 1;
        }
        else if (stage == 1)
        {
            transform.position = PointCuttingBoard.position;
            currentBasePosition = PointCuttingBoard.position;
            stage = 2;
        }
    }

    protected override void OnDropFail()
    {
        if (stage == 0)
            transform.position = transform.parent.TransformPoint(startLocalPosition);
        else if (stage == 1)
            transform.position = PointWater.position;

        currentBasePosition = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            Debug.Log("Va cham");
            StartCoroutine(TimeDelayShake(2f));
        }
    }
    private IEnumerator TimeDelayShake(float time)
    {
        yield return new WaitForSeconds(time);
        Debug.Log("Het 2s");
        isShaking = true;
        shakeTimer = 3f;
    }
}
