using UnityEngine;

public class GezegenDonme : MonoBehaviour
{
    public Transform planet;   // Gezegen
    public Transform player;   // Sabit karakter

    public float moveSpeed = 25f;
    public float turnSpeed = 25f;
    public float smooth = 10f;

    float h;
    float v;

    void Update()
    {
        // 🎮 INPUT
        h = Input.GetAxis("Horizontal");
        v = Mathf.Max(Input.GetAxis("Vertical"), 0f); // 🔥 S YOK

        Vector3 center = planet.position;

        Vector3 upAxis = (player.position - center).normalized;
        Vector3 rightAxis = player.right;

        // 🌍 İLERİ HAREKET (SADECE W)
        if (v > 0.001f)
        {
            planet.RotateAround(
                center,
                rightAxis,
                -v * moveSpeed * Time.deltaTime
            );
        }

        // 🌍 SAĞ / SOL
        if (Mathf.Abs(h) > 0.001f)
        {
            planet.RotateAround(
                center,
                upAxis,
                -h * turnSpeed * Time.deltaTime
            );
        }
    }

    void LateUpdate()
    {
        Vector3 upDir = (player.position - planet.position).normalized;

        // Mevcut ileri yönü koru
        Vector3 forwardOnSurface =
            Vector3.ProjectOnPlane(player.forward, upDir).normalized;

        Quaternion targetRot =
            Quaternion.LookRotation(forwardOnSurface, upDir);

        player.rotation = Quaternion.Slerp(
            player.rotation,
            targetRot,
            Time.deltaTime * smooth
        );
    }

}
