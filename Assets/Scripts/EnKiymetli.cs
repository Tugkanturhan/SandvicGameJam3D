using UnityEngine;

public class EnKiymetli : MonoBehaviour
{
    [Header("Referanslar")]
    public Transform planet;
    public Transform cameraTransform;
    private Animator anim;

    [Header("Hız Ayarları")]
    public float moveSpeed = 25f;
    public float characterTurnSpeed = 15f;
    public float planetTurnSpeed = 80f;
    public float surfaceAlignmentSpeed = 25f;

    private Vector3 upAxis;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        upAxis = (transform.position - planet.position).normalized;

        float h = Input.GetAxis("Horizontal"); // A-D veya Sol-Sağ ok
        float v = Mathf.Max(Input.GetAxis("Vertical"), 0f); // W veya Yukarı ok

        // --- ANIMASYONLARI TETIKLE ---
        if (anim != null)
        {
            float speed = Mathf.Clamp01(v + Mathf.Abs(h));
            anim.SetFloat("Speed", speed);

            // 🛡️ Guard (SADECE basılıyken)
            bool isGuarding = Input.GetMouseButton(1);
            anim.SetBool("isGuarding", isGuarding);

            // ⚡ Deflect (ANLIK)
            if (Input.GetKeyDown(KeyCode.R))
            {
                // Guard açıkken deflect olmaz → kapat
                anim.SetBool("isGuarding", false);

                // Trigger'ı temiz tetikle
                anim.ResetTrigger("deflect");
                anim.SetTrigger("deflect");
            }
        }

        HandlePlanetMovement(h, v);
        HandleCharacterRotation(h, v);
    }

    void HandleCharacterRotation(float h, float v)
    {
        if (Mathf.Abs(h) < 0.01f && v < 0.01f) return;

        Vector3 camForward = Vector3.ProjectOnPlane(cameraTransform.forward, upAxis).normalized;
        Vector3 camRight = Vector3.Cross(upAxis, camForward);
        Vector3 targetDir = (camForward * v) + (camRight * h);

        if (targetDir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(targetDir, upAxis);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, characterTurnSpeed * Time.deltaTime);
        }
    }

void HandlePlanetMovement(float h, float v)
{
    // --- KRİTİK EKLEME ---
    if (anim != null)
    {
        // Karakterin toplam hareket miktarını hesapla (h ve v'nin mutlak değer toplamı)
        float toplamHiz = Mathf.Clamp01(Mathf.Abs(h) + Mathf.Abs(v));
        anim.SetFloat("Speed", toplamHiz); 
    }
    // ---------------------

    if (v > 0.001f)
        planet.RotateAround(planet.position, transform.right, -v * moveSpeed * Time.deltaTime);

    if (Mathf.Abs(h) > 0.001f)
        planet.RotateAround(planet.position, upAxis, -h * planetTurnSpeed * Time.deltaTime);
}

    void LateUpdate()
    {
        Vector3 currentUp = (transform.position - planet.position).normalized;
        Vector3 forward = transform.forward;
        Vector3.OrthoNormalize(ref currentUp, ref forward); 
        Quaternion targetRotation = Quaternion.LookRotation(forward, currentUp);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, surfaceAlignmentSpeed);
    }
}