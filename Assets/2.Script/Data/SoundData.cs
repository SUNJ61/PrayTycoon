using UnityEngine;

[CreateAssetMenu(fileName = "NewSoundData", menuName = "Sound/SoundData")]
public class SoundData : ScriptableObject
{
    public string SoundType;
    public string ClipName;
    public AudioClip clip;
    [Range(0, 1)] public float Voulume = 0.5f;
    public bool loop;
}
