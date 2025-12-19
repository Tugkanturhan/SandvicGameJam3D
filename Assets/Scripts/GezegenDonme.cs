using UnityEngine;

public class GezegenDonme : MonoBehaviour
{
[Header("Referanslar")]
    public Transform planet; 

    [Header("Hız Ayarları")]
    public float moveSpeed = 60f;      // Dünyanın kayma hızı
    public float turnSpeed = 10f;      // Karakterin bakış yönü hızı
    public float lerpTime = 10f;       // Hareketin yumuşaklığı (Smooth)

    private float currentH, currentV;

    void Update()
    {
        // 1. Girişleri al ve yumuşat (Smooth Input)
        float targetH = Input.GetAxis("Horizontal");
        float targetV = Input.GetAxis("Vertical");

        currentH = Mathf.Lerp(currentH, targetH, Time.deltaTime * lerpTime);
        currentV = Mathf.Lerp(currentV, targetV, Time.deltaTime * lerpTime);

        // 2. DÜNYAYI DÖNDÜR (Karakterin altında kayar)
        if (Mathf.Abs(currentH) > 0.01f || Mathf.Abs(currentV) > 0.01f)
        {
            // Gezegeni karakterin sağ ve ileri eksenine göre döndürüyoruz.
            // Bu sayede hangi yöne bakarsan bak "W" her zaman ileri götürür.
            planet.Rotate(transform.right, currentV * moveSpeed * Time.deltaTime, Space.World);
            planet.Rotate(transform.up, -currentH * moveSpeed * Time.deltaTime, Space.World);

            // 3. KARAKTERİ DÖNDÜR (Görsel Bakış Yönü)
            // Karakter sadece yerel Y ekseninde döner.
            HandleCharacterRotation(targetH, targetV);
        }
    }

    void HandleCharacterRotation(float h, float v)
    {
        // Hareket vektörünü hesapla
        Vector3 inputDir = new Vector3(h, 0, v).normalized;

        if (inputDir.magnitude > 0.1f)
        {
            // Karakterin yerel Y açısını hesapla (Atan2 açıyı radyan olarak verir)
            float targetAngle = Mathf.Atan2(h, v) * Mathf.Rad2Deg;
            
            // Mevcut yerel rotasyonu yumuşakça hedef açıya çevir
            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * turnSpeed);
        }
    }

    // Karakterin her zaman gezegenin merkezine göre dik durmasını sağlayan FixedUpdate
    void FixedUpdate()
    {
        if (planet != null)
        {
            Vector3 upDir = (transform.position - planet.position).normalized;
            Quaternion toRotation = Quaternion.FromToRotation(transform.up, upDir) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 0.1f);
        }
    }
}
