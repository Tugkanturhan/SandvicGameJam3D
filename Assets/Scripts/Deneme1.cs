using UnityEngine;

public class PlanetTreadmillController : MonoBehaviour
{
    [Header("Referanslar")]
    public Transform planet;
    public Transform cameraTransform;

    [Header("Hız Ayarları")]
    public float moveSpeed = 25f;          
    public float characterTurnSpeed = 15f; // Yumuşak bakış geçişi
    public float planetTurnSpeed = 80f;    // A/D basınca gezegen hızı (Artırıldı)
    public float surfaceAlignmentSpeed = 25f; // Titremeyi önlemek için artırıldı

    private Vector3 upAxis;

    void Update()
    {
        // 1. Eksenleri hesapla
        upAxis = (transform.position - planet.position).normalized;

        HandlePlanetMovement();
        HandleCharacterRotation();
    }

    void HandleCharacterRotation()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Mathf.Max(Input.GetAxis("Vertical"), 0f);

        if (Mathf.Abs(h) < 0.01f && v < 0.01f) return;

        // Kameraya göre yönü belirle
        Vector3 camForward = Vector3.ProjectOnPlane(cameraTransform.forward, upAxis).normalized;
        Vector3 camRight = Vector3.Cross(upAxis, camForward);
        Vector3 targetDir = (camForward * v) + (camRight * h);

        if (targetDir.sqrMagnitude > 0.001f)
        {
            // TİTREME ÇÖZÜMÜ: Sadece Y ekseninde (kendi etrafında) döndür
            Quaternion targetRot = Quaternion.LookRotation(targetDir, upAxis);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, characterTurnSpeed * Time.deltaTime);
        }
    }

    void HandlePlanetMovement()
    {
        float v = Mathf.Max(Input.GetAxis("Vertical"), 0f); 
        float h = Input.GetAxis("Horizontal");

        // W ile İleri
        if (v > 0.001f)
        {
            planet.RotateAround(planet.position, transform.right, -v * moveSpeed * Time.deltaTime);
        }

        // A-D ile Keskin Dönüş
        if (Mathf.Abs(h) > 0.001f)
        {
            planet.RotateAround(planet.position, upAxis, -h * planetTurnSpeed * Time.deltaTime);
        }
    }

    // TİTREMEYİ ÖNLEMEK İÇİN: Tüm rotasyon bittikten sonra en son dikliği koru
    void LateUpdate()
    {
        Vector3 currentUp = (transform.position - planet.position).normalized;
        
        // Karakterin ileri yönünü gezegen yüzeyine projekte et
        Vector3 forward = transform.forward;
        Vector3.OrthoNormalize(ref currentUp, ref forward); 
        
        // Doğrudan Quaternion.LookRotation kullanarak yönü sabitle
        Quaternion targetRotation = Quaternion.LookRotation(forward, currentUp);
        
        // Çok yüksek bir hızla veya direkt eşitleyerek titremeyi bitir
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, surfaceAlignmentSpeed);
    }
}