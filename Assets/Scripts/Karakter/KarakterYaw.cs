using UnityEngine;

public class KarakterYaw : MonoBehaviour
{
    public Transform planet;
    public Transform cameraTransform;
    public float yawSpeed = 180f; // 🔥 NET KONTROL

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        if (Mathf.Abs(h) < 0.01f) return;

        Vector3 upAxis = (transform.position - planet.position).normalized;

        // Karakteri kendi up ekseni etrafında döndür
        transform.Rotate(
            upAxis,
            h * yawSpeed * Time.deltaTime,
            Space.World
        );
    }
}
