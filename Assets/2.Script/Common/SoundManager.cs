using UnityEngine.Audio;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    public AudioMixer audioMixer;

    public void PlayBGM(AudioClip clip, bool loop = true) //BGM 재생
    {
        if (bgmSource.clip == clip) return; 

        bgmSource.clip = clip;
        bgmSource.loop = loop;
        bgmSource.Play();
    }

    public void PlaySFX(AudioClip clip) // 위치 지정이 필요없는 SFX 소리 재생
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlaySFX(AudioClip clip, Vector3 position) //위치 지정이 필요한 SFX 소리 재생
    {
        AudioSource.PlayClipAtPoint(clip, position);
    }

    public void SetVolume(string name, float sliderValue) //옵션으로 소리 조절시 소리값 조절
    {
        if(sliderValue <= 0.0001f)
            audioMixer.SetFloat(name, -80f);
        else
            audioMixer.SetFloat(name, Mathf.Log10(sliderValue) * 20);
    }
}
