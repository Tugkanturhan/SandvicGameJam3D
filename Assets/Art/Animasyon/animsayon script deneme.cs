using UnityEngine;

public class animsayonscriptdeneme : MonoBehaviour
{
private Animator anim;
    
    // Değişken isimlerinin Animator'daki Parameters kısmıyla BİREBİR aynı olması lazım
    [Header("Animasyon Parametre İsimleri")]
    public string guardBool = "isGuarding";
    public string deflectTrigger = "deflect";

    void Awake() {
        anim = GetComponent<Animator>();
    }

    void Update() {
        HandleAnimations();
    }

    void HandleAnimations() {
        // GARD ALMA: Sağ tık basılı tutulduğu sürece
        if (Input.GetMouseButton(1)) {
            anim.SetBool(guardBool, true);
        } else {
            anim.SetBool(guardBool, false);
        }

        // SAVUŞTURMA: "E" tuşuna basıldığı an (Tek seferlik)
        if (Input.GetKeyDown(KeyCode.E)) {
            anim.SetTrigger(deflectTrigger);
        }
        
        // KOŞMA/YÜRÜME (Eğer Animator'da Float varsa)
        // Mevcut treadmill scriptindeki hızı buraya bağlayabiliriz
        // float speed = GetComponent<Rigidbody>().velocity.magnitude;
        // anim.SetFloat("Speed", speed);
    }
}
