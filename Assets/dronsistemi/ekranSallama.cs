using UnityEngine;

public class ekranSallama : MonoBehaviour
{
    [Header("Yürüme Sarsıntısı (Head Bob)")]
    public float yürümeHızı = 14f; 
    public float sarsıntıMiktarı = 0.05f; 
    public Transform kamera; // Main Camera'yı buraya sürükle

    [Header("Kontrol Kaybı (Screen Shake)")]
    public float shakeŞiddeti = 0f; // Kodla bunu artıracağız
    
    private float timer = 0f;
    private Vector3 orjinalPos;

    void Start()
    {
        // Kameranın başlangıç yerini kaydet
        orjinalPos = kamera.localPosition;
    }

    void Update()
    {
        HandleHeadBob();
        HandleScreenShake();
    }

    // 1. Yürüme Efekti (W-A-S-D basınca sallanır)
    void HandleHeadBob()
    {
        float yatay = Input.GetAxis("Horizontal");
        float dikey = Input.GetAxis("Vertical");

        if (Mathf.Abs(yatay) > 0.1f || Mathf.Abs(dikey) > 0.1f)
        {
            // Yürüme ritmi (Sinüs dalgası)
            timer += Time.deltaTime * yürümeHızı;
            float yeniY = orjinalPos.y + Mathf.Sin(timer) * sarsıntıMiktarı;
            kamera.localPosition = new Vector3(kamera.localPosition.x, yeniY, kamera.localPosition.z);
        }
        else
        {
            // Durunca kamerayı yavaşça merkeze çek
            timer = 0;
            kamera.localPosition = Vector3.Lerp(kamera.localPosition, orjinalPos, Time.deltaTime * 5f);
        }
    }

    // 2. Kontrol Kaybı Efekti (Ekran Titremesi)
    void HandleScreenShake()
    {
        if (shakeŞiddeti > 0)
        {
            Vector3 randomSallama = orjinalPos + Random.insideUnitSphere * shakeŞiddeti;
            kamera.localPosition = new Vector3(randomSallama.x, kamera.localPosition.y, randomSallama.z);
        }
    }

    // Gezegen etkilerinden bu fonksiyonu çağırabilirsin
    public void SarsıntıyıAyarla(float miktar)
    {
        shakeŞiddeti = miktar;
    }
}