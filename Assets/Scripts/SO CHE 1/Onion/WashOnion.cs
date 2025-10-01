using System.Collections;
using UnityEngine;

public class Onion : DraggableBase
{
    [SerializeField] private Transform[] dropPoints;   // danh sách các điểm có thể thả
    [SerializeField] private Transform destroyTarget;
    [SerializeField] private float snapDistance = 0.5f;
    [SerializeField] private float shakeAmount = 0.05f;

    private bool hasTriggeredShake = false;
    private Vector3 currentBasePosition;

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
                hasTriggeredShake = false;
            }
        }
    }

    protected override bool CheckCorrectDropZone()
    {
        // Kiểm tra xem có điểm nào trong dropPoints nằm trong khoảng snap không
        foreach (Transform point in dropPoints)
        {
            if (Vector2.Distance(transform.position, point.position) <= snapDistance)
            {
                return true;
            }
        }
        return false;
    }

    protected override void OnDropSuccess()
    {
        // Snap tới điểm gần nhất
        Transform nearest = null;
        float minDist = Mathf.Infinity;

        foreach (Transform point in dropPoints)
        {
            float dist = Vector2.Distance(transform.position, point.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = point;
            }
        }

        if (nearest != null)
        {
            transform.position = nearest.position;
            currentBasePosition = nearest.position;
        }
    }

    protected override void OnDropFail()
    {
        // nếu không đúng điểm nào thì quay lại chỗ cũ nhất (currentBasePosition)
        transform.position = currentBasePosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Water") && !hasTriggeredShake)
        {
            Debug.Log("Va cham");
            hasTriggeredShake = true;
            StartCoroutine(TimeDelayShake(2f));
        }
    }

    private IEnumerator TimeDelayShake(float time)
    {
        yield return new WaitForSeconds(time);
        Debug.Log("Het 2s");
        isShaking = true;
        shakeTimer = 2f;
    }
}
