using UnityEngine;
using UnityEngine.Video;

public class SadvicRadyo : MonoBehaviour
{
    [Header("Referanslar")]
    public AudioSource sesKaynagi;
    public VideoPlayer videoOynatici;
    public GameObject radyoEkrani;
    public Animator radyoAnim;

    private bool oyuncuYakinMi = false;
    public bool kasetTakiliMi = false;

    void Update()
    {
        // Eğer oyuncu yakınsa VE E tuşuna bastıysa VE kaset takılı değilse
        if (oyuncuYakinMi && Input.GetKeyDown(KeyCode.E) && !kasetTakiliMi)
        {
            KasetKontrolEt();
        }
    }

    // Oyuncu alana girdiğinde
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Karakterinin Tag'i "Player" olmalı
        {
            oyuncuYakinMi = true;
            Debug.Log("Radyonun yanındasın, E'ye basabilirsin.");
        }
    }

    // Oyuncu alandan çıktığında
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            oyuncuYakinMi = false;
        }
    }

    void KasetKontrolEt()
    {
        Kaset eldekiKaset = Object.FindFirstObjectByType<Kaset>(); 
        if (eldekiKaset != null)
        {
            if (radyoAnim != null) radyoAnim.SetTrigger("KasetTakildi");
            
            VeriyiIsle(eldekiKaset.veri);
            Destroy(eldekiKaset.gameObject); 
            kasetTakiliMi = true;
        }
        else 
        {
            Debug.Log("Elinizde kaset yok!");
        }
    }

    void VeriyiIsle(KasetVerisi veri)
    {
        sesKaynagi.clip = veri.muzik;
        sesKaynagi.Play();

        if (veri.video != null && videoOynatici != null)
        {
            if (radyoEkrani != null) radyoEkrani.SetActive(true);
            videoOynatici.clip = (VideoClip)veri.video;
            videoOynatici.Play();
        }
    }
}