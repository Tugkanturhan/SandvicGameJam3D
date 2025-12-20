using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "KasetVerisi", menuName = "Scriptable Objects/KasetVerisi")]
public class KasetVerisi : ScriptableObject
{
    public string kasetAdi;
    public AudioClip muzik;
    public VideoClip video; 
    public Color kasetRengi = Color.white;
}
