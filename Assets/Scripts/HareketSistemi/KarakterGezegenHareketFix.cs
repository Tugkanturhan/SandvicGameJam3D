using UnityEngine;

public class KarakterGezegenHareketFix : MonoBehaviour
{
  public Transform planet;
    public float moveSpeed = 6f;
    public float rotateSpeed = 10f;

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal"); // A D
        float v = Input.GetAxisRaw("Vertical");   // W S

        // 1️⃣ Gravity yönü (merkeze doğru)
        Vector3 up = (transform.position - planet.position).normalized;

        // 2️⃣ Forward = karakterin baktığı yön, gezegen yüzeyine projelenmiş hali
        Vector3 forward = Vector3.ProjectOnPlane(transform.forward, up).normalized;

        // 3️⃣ Right = forward x up (SAĞLAM REFERANS)
        Vector3 right = Vector3.Cross(up, forward).normalized;

        // 4️⃣ Net ve kontrollü hareket
        Vector3 moveDir = (forward * v + right * h).normalized;
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        // 5️⃣ Karakteri gezegene dik tut
        Quaternion targetRot =
            Quaternion.LookRotation(forward, up);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRot,
            rotateSpeed * Time.deltaTime
        );
    }
}
