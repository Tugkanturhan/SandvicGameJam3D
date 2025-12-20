using UnityEngine;

public class RandomObj : MonoBehaviour
{
public Transform planet;           
    public GameObject[] objeler;      
    public int objeSayisi = 20;        

    void Start()
    {
        // KODUN ÇALIŞIP ÇALIŞMADIĞINI ANLAMAK İÇİN BURAYA MESAJ EKLEDİK
        Debug.Log("Sistem Başlatıldı! Obje dağıtımı başlıyor...");

        if (planet == null) {
            Debug.LogError("HATA: Planet seçilmedi! Lütfen Inspector'dan Gezegen objesini sürükleyin.");
            return; 
        }

        if (objeler.Length == 0) {
            Debug.LogError("HATA: Objeler listesi boş! Lütfen prefabları sürükleyin.");
            return;
        }

        ObjeleriDagit();
    }

    void ObjeleriDagit()
    {
        float yaricap = planet.localScale.x * 1f; // Yarıçapı biraz büyük tutalım garanti olsun

        for (int i = 0; i < objeSayisi; i++)
        {
            Vector3 rastgeleYon = Random.onUnitSphere;
            Vector3 pozisyon = planet.position + (rastgeleYon * yaricap);

            GameObject yeniObje = Instantiate(objeler[Random.Range(0, objeler.Length)], pozisyon, Quaternion.identity);
            
            // Hiyerarşide Gezegenin altında görebilmen için:
            yeniObje.transform.SetParent(planet);
            
            Debug.Log(i + ". obje oluşturuldu: " + yeniObje.name);
        }
    }
}
