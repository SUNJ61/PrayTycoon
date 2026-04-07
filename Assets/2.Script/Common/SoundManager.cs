using UnityEngine.Audio;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SoundManager : Singleton<SoundManager>
{
    private List<AudioSource> SoundBoxPool = new List<AudioSource>();
    private List<AudioSource> LoopSoundBox = new List<AudioSource>();
    private GameObject SoundBoxPrefab;
    private int PoolCount = 10;

    public List<SoundData> GameSound;

    public AudioMixer audioMixer;
    public AudioMixerGroup BGM;
    public AudioMixerGroup SFX;

    protected override void OnAwake()
    {
        // 사운드 박스 프리팹 리소스에서 가져오기.

        SoundBoxPooling(); //사운드 박스 오브젝트 풀링
    }
    public void PlaySound(string name) // 게임 배경음 재생
    {
        SoundData data = GameSound.Find(s => s.ClipName == name);

        if(data != null)
        {
            if(data.clip == null) return;
            //사운드 박스 소환, 사운드 플레이 코드 필요 
            AudioSource source = SoundBoxPool.Find(s => !s.gameObject.activeSelf); //폴링된 리스트에서 비활성화 된 사운드 박스 찾기.

            if(source == null) source = CreateNewSoundBox(); //사운드 박스가 부족할 시 추가 생성.
            
            source.gameObject.SetActive(true);
            
            switch(data.SoundType) //사운드 박스 믹서 그룹 할당.
            {
                case "BGM":
                source.outputAudioMixerGroup = BGM;
                    break;

                case "SFX":
                source.outputAudioMixerGroup = SFX;
                    break;
            }

            source.clip = data.clip;
            source.loop = data.loop;
            source.Play();

            if(data.loop == false)
                StartCoroutine(EndSFX(source));
            else
                LoopSoundBox.Add(source);
        }
    }

    public void SetVolume(string name, float sliderValue) //옵션으로 소리 조절시 소리값 조절
    {
        if(sliderValue <= 0.0001f)
            audioMixer.SetFloat(name, -80f);
        else
            audioMixer.SetFloat(name, Mathf.Log10(sliderValue) * 20);
    }

    public void EndBGM() //루프로 돌고있는 BGM 끄기
    {
        //소스에 할당된 BGM, SFX초기화 필요, 할당된 클립 제거 필요, 사운드 박스 비활성화 필요
    }

    private void SoundBoxPooling()
    {
        for(int i = 0; i < PoolCount; i++)
        {
            CreateNewSoundBox();
        }
    }

    private AudioSource CreateNewSoundBox()
    {
        GameObject soundbox = Instantiate(SoundBoxPrefab, transform);
        AudioSource audioSource = soundbox.GetComponent<AudioSource>();
        soundbox.SetActive(false);
        SoundBoxPool.Add(audioSource);

        return audioSource;
    }

    private IEnumerator EndSFX(AudioSource source)
    {
        yield return new WaitForSeconds(source.clip.length);
        source.outputAudioMixerGroup = null;
        source.clip = null;
        source.gameObject.SetActive(false);
    }
}
