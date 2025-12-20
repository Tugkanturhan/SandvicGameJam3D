using UnityEngine;

public class KameraAyarı : MonoBehaviour
{
    public Transform planet;
    public Transform cam;
    public float moveSpeed = 6f;
    public float rotateSpeed = 10f;

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal"); // A D
        float v = Input.GetAxisRaw("Vertical");   // W S

        // 1️⃣ Gravity (up) yönü
        Vector3 up = (transform.position - planet.position).normalized;

        // 2️⃣ Kamera forward & right'ını gezegen yüzeyine projeleriz
        Vector3 camForward = Vector3.ProjectOnPlane(cam.forward, up).normalized;
        Vector3 camRight   = Vector3.ProjectOnPlane(cam.right, up).normalized;

        // 3️⃣ Oyuncu ALGIsına göre hareket
        Vector3 moveDir = (camForward * v + camRight * h).normalized;
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        // 4️⃣ Karakter bakış yönü = hareket yönü
        if (moveDir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir, up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotateSpeed * Time.deltaTime);
        }
        else
        {
            // Hareket yokken sadece gezegene dik dur
            Quaternion upright = Quaternion.FromToRotation(transform.up, up) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, upright, rotateSpeed * Time.deltaTime);
        }
    }

    
}
