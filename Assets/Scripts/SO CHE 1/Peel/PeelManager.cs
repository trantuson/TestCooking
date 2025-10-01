using UnityEngine;

public class PeelManager : MonoBehaviour
{
    [Header("Knife Settings")]
    public float cutRadius = 50f;
    public ParticleSystem peelParticlesPrefab; // gán prefab trong Inspector

    private ParticleSystem peelParticlesInstance; // instance dùng runtime
    private Vector3 offset;
    private PeelableObject currentTarget;

    private Vector3 lastPos;
    private bool isDragging = false;
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;

        // tạo particle pool
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
        if (Input.GetMouseButtonDown(0))
        {
            // Raycast chỉ bắt layer "Knife" (dao)
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 0f, LayerMask.GetMask("Knife"));
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                Vector3 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mp.z = transform.position.z;
                offset = transform.position - mp;

                lastPos = transform.position;
                isDragging = true;
            }
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            Vector3 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mp.z = transform.position.z;
            transform.position = mp + offset;

            if (currentTarget != null)
            {
                CutAlongLine(lastPos, transform.position);
            }

            lastPos = transform.position;
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;
            currentTarget = null;
            transform.position = startPosition;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PeelableObject peel = collision.GetComponent<PeelableObject>();
        if (peel != null) currentTarget = peel;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PeelableObject peel = collision.GetComponent<PeelableObject>();
        if (peel != null && peel == currentTarget) currentTarget = null;
    }

    private void CutAlongLine(Vector3 start, Vector3 end)
    {
        float distance = Vector3.Distance(start, end);
        int steps = Mathf.Clamp(Mathf.CeilToInt(distance / 0.01f), 1, 10);

        for (int i = 0; i <= steps; i++)
        {
            Vector3 pos = Vector3.Lerp(start, end, i / (float)steps);

            if (currentTarget != null)
            {
                currentTarget.CutAtPosition(pos, cutRadius);

                // particle
                if (peelParticlesInstance != null)
                {
                    var emit = new ParticleSystem.EmitParams();
                    emit.position = new Vector3(pos.x, pos.y, transform.position.z);
                    emit.startColor = currentTarget.peelParticleColor;
                    peelParticlesInstance.Emit(emit, 3); // giảm số particle
                }
            }
        }
    }
}

