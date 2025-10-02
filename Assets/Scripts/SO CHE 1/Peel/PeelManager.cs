using UnityEngine;

public class PeelManager : MonoBehaviour
{
    [Header("Knife Settings")]
    public float cutRadius = 50f;
    public float returnSpeed = 10f;
    public ParticleSystem peelParticlesPrefab;

    private ParticleSystem peelParticlesInstance;
    private Vector3 offset;
    private PeelableObject currentTarget;

    private Vector3 lastPos;
    private bool isDragging = false;
    private Vector3 startLocalPosition;

    void Start()
    {
        startLocalPosition = transform.localPosition;

        if (peelParticlesPrefab != null)
        {
            peelParticlesInstance = Instantiate(peelParticlesPrefab);
            peelParticlesInstance.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

            var main = peelParticlesInstance.main;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
        }
    }

    void Update()
    {
        Vector3 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mp.z = transform.position.z;

        // --- Bắt đầu kéo dao ---
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(mp, Vector2.zero, 0f, LayerMask.GetMask("Knife"));
            // chỉ bắt khi click trực tiếp vào dao
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                offset = transform.position - mp;
                lastPos = transform.position;
                isDragging = true;
            }
        }

        // --- Khi đang kéo ---
        if (Input.GetMouseButton(0) && isDragging)
        {
            transform.position = mp + offset;

            if (currentTarget != null && !currentTarget.isFinished)
            {
                CutAlongLine(lastPos, transform.position);

                if (currentTarget.isFinished)
                {
                    // Gọt xong -> dừng kéo, dao auto quay về
                    isDragging = false;
                    currentTarget = null;
                }
            }

            lastPos = transform.position;
        }

        // --- Khi thả chuột ---
        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;
            currentTarget = null;
        }

        // --- Khi không kéo -> dao quay về ---
        if (!isDragging)
        {
            transform.localPosition = Vector3.MoveTowards(
                transform.localPosition,
                startLocalPosition,
                returnSpeed * Time.deltaTime
            );
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PeelableObject peel = collision.GetComponent<PeelableObject>();
        if (peel != null && !peel.isFinished) 
            currentTarget = peel;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PeelableObject peel = collision.GetComponent<PeelableObject>();
        if (peel != null && peel == currentTarget) 
            currentTarget = null;
    }

    private void CutAlongLine(Vector3 start, Vector3 end)
    {
        float distance = Vector3.Distance(start, end);
        int steps = Mathf.Clamp(Mathf.CeilToInt(distance / 0.01f), 1, 10);

        for (int i = 0; i <= steps; i++)
        {
            Vector3 pos = Vector3.Lerp(start, end, i / (float)steps);

            if (currentTarget != null && !currentTarget.isFinished)
            {
                currentTarget.CutAtPosition(pos, cutRadius);

                if (peelParticlesInstance != null)
                {
                    var emit = new ParticleSystem.EmitParams();
                    emit.position = new Vector3(pos.x, pos.y, transform.position.z);
                    emit.startColor = currentTarget.peelParticleColor;
                    peelParticlesInstance.Emit(emit, 3);
                }
            }
        }
    }
}
