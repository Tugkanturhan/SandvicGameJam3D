using UnityEngine;

public class KarakterDondurme : MonoBehaviour
{
public Transform planet; 
    public float rotateSpeed = 50f;     // Dünyanın dönüş hızı
    public float characterTurnSpeed = 10f; // Karakterin sağa sola dönme hızı

    void Update()
    {
        float h = Input.GetAxis("Horizontal"); // A - D
        float v = Input.GetAxis("Vertical");   // W - S

        // 1. DÜNYAYI DÖNDÜR (Karakterin altında kayar)
        if (Mathf.Abs(h) > 0.1f || Mathf.Abs(v) > 0.1f)
        {
            // Dünyayı karakterin "ileri" ve "sağ" yönlerine göre zıt yönde döndür
            planet.Rotate(transform.right, v * rotateSpeed * Time.deltaTime, Space.World);
            planet.Rotate(transform.up, -h * rotateSpeed * Time.deltaTime, Space.World);

            // 2. KARAKTERİ DÖNDÜR (Baktığı yöne yumuşakça döner)
            // Karakter sadece kendi Y ekseninde (sağa-sola) döner
            Vector3 hareketYonu = new Vector3(h, 0, v).normalized;
            if (hareketYonu != Vector3.zero)
            {
                // Karakterin yerel rotasyonunu ayarla
                Quaternion hedefRotasyon = Quaternion.LookRotation(transform.forward + transform.right * h + transform.forward * v);
                // Not: Karakter sabit durduğu için sadece görsel olarak döndürüyoruz
                transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0, Mathf.Atan2(h, v) * Mathf.Rad2Deg, 0), Time.deltaTime * characterTurnSpeed);
            }
        }
    }

    void FixedUpdate()
    {
        // Karakterin her zaman gezegenin tam tepesinde dik durmasını sağlar
        AlignToPlanet();
    }

    void AlignToPlanet()
    {
        Vector3 upDirection = (transform.position - planet.position).normalized;
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, upDirection) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 20f * Time.deltaTime);
    }
}
