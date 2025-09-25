using System.Collections;
using UnityEngine;

public class MouseDrapDrop : MonoBehaviour
{
    public Transform leaf;           // gán lá
    public float liftHeight = 0.5f;    // chiều cao nhấc
    public float liftDuration = 2f;  // thời gian nhấc lên
    public float stretchFactor = 0.1f; // lá căng
    public float flyDistance = 10f;   // bay ra ngoài
    public float flyDuration = 1f;   // thời gian bay

    private Coroutine liftCoroutine;

    private void OnMouseDown()
    {
        if (liftCoroutine == null)
            liftCoroutine = StartCoroutine(LiftAndFly());
    }

    private void OnMouseUp()
    {
        if (liftCoroutine != null)
        {
            StopCoroutine(liftCoroutine);
            liftCoroutine = null;
        }
    }

    private IEnumerator LiftAndFly()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + new Vector3(0, liftHeight, 0);

        Vector3 startScale = leaf.localScale;
        Vector3 endScale = new Vector3(
            startScale.x,
            startScale.y + liftHeight * stretchFactor,
            startScale.z
        );

        float elapsed = 0f;

        while (elapsed < liftDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / liftDuration;

            transform.position = Vector3.Lerp(startPos, endPos, t);

            if (leaf != null)
                leaf.localScale = Vector3.Lerp(startScale, endScale, t);

            yield return null;
        }

        transform.position = endPos;
        if (leaf != null)
            leaf.localScale = endScale;

        Vector3 flyStart = transform.position;
        
        Vector3 flyEnd = flyStart + new Vector3(Random.Range(-flyDistance, flyDistance), flyDistance, 0);

        elapsed = 0f;
        while (elapsed < flyDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / flyDuration;

            transform.position = Vector3.Lerp(flyStart, flyEnd, t);
            if (leaf != null)
                leaf.position = transform.position;

            yield return null;
        }

        transform.position = flyEnd;
        if (leaf != null)
            leaf.position = flyEnd;
    }
}
