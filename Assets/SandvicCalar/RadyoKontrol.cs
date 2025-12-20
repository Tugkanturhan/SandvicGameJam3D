using UnityEngine;
using UnityEngine.Video;

public class RadyoKontrol : MonoBehaviour
{
    public AudioSource sesKaynagi;
    public VideoPlayer videoOynatici;
    public Transform kasetYuvasi; // Kasetin oturacağı yer

    private void OnTriggerEnter(Collider other)
    {
        // Çarpan nesne bir kaset mi kontrol et
        Kaset takilanKaset = other.GetComponent<Kaset>();

        if (takilanKaset != null)
        {
            KasetiCal(takilanKaset.veri);
            
            // Kaseti yuvaya sabitle (Opsiyonel)
            other.transform.position = kasetYuvasi.position;
            other.transform.rotation = kasetYuvasi.rotation;
            other.GetComponent<Rigidbody>().isKinematic = true; 
        }
    }

    void KasetiCal(KasetVerisi veri)
    {
        // Ses Ayarları
        sesKaynagi.clip = veri.muzik;
        sesKaynagi.Play();

        // Video Ayarları (Eğer kasetin içinde video varsa)
        if (veri.video != null)
        {
            videoOynatici.clip = (VideoClip)veri.video;
            videoOynatici.Play();
        }
        
        Debug.Log(veri.kasetAdi + " oynatılıyor...");
    }
}