using UnityEngine;

public class KarakterYon : MonoBehaviour
{
    public Transform planet;
    public Transform cameraTransform;
    public float rotationSpeed = 8f;

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // S tuşu basılıysa dönme YAPMA
        if (v < 0 && Mathf.Abs(h) < 0.01f)
            return;

        Vector3 upAxis = (transform.position - planet.position).normalized;

        Vector3 camForward = Vector3.ProjectOnPlane(cameraTransform.forward, upAxis).normalized;
        Vector3 camRight = Vector3.Cross(upAxis, camForward);

        // W ve A/D için yön hesapla
        Vector3 targetDir = camForward * -Mathf.Max(v, 0) + camRight * -h;

        if (targetDir.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(targetDir, upAxis);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }
}
