using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    public GameObject OptionUI;

    public TMP_Dropdown resDropdown;
    public Slider BGMSlider;
    public Slider SFXSlider;
    public Toggle FullScreenToggle;

    private readonly List<int> standardWidths = new List<int> { 1280, 1600, 1920, 2560, 3840 };
    private List<Resolution> filteredResolutions = new List<Resolution>();
    void Start()
    {
        InitResolution();

        LobbyOptionSet();

        BGMSlider.onValueChanged.AddListener(BGMsetting);
        SFXSlider.onValueChanged.AddListener(SFXsetting);
    }

    public void InitResolution() //정렬순서 바꾸기 필요. 권장 해상도를 가장 위로 올 수 있도록 해야함.
    {
        Resolution[] allResolutions = Screen.resolutions;
        resDropdown.options.Clear();

        // 현재 모니터의 종횡비 계산
        float targetAspect = (float)Screen.currentResolution.width / Screen.currentResolution.height;

        for (int i = 0; i < allResolutions.Length; i++)
        {
            float currentAspect = (float)allResolutions[i].width / allResolutions[i].height;

            //필터링, 위에서 계산한 비율과 비슷한 값의 표준 해상도만 선택.
            if (Mathf.Abs(currentAspect - targetAspect) < 0.01f)
            {
                if (standardWidths.Contains(allResolutions[i].width))
                {
                    // 중복 제거 (주사율 차이 무시)
                    if (filteredResolutions.Exists(r => r.width == allResolutions[i].width))
                        continue;

                    filteredResolutions.Add(allResolutions[i]);
                }
            }
        }

        //만약 필터링된게 하나도 없다면(특수 해상도), 현재 해상도는 무조건 추가
        if (filteredResolutions.Count == 0)
        {
            filteredResolutions.Add(Screen.currentResolution);
        }

        // 드롭다운 UI 업데이트
        foreach (var res in filteredResolutions)
        {
            string option = $"{res.width} x {res.height}";
            if (res.width == Screen.currentResolution.width) option += " (권장)";
            resDropdown.options.Add(new TMP_Dropdown.OptionData(option));
        }
        
        resDropdown.RefreshShownValue();
    }

    private void LobbyOptionSet() //기본 설정으로 초기화
    {
        BGMSlider.value = SaveManager.Instance.VolumeBGM;
        SFXSlider.value = SaveManager.Instance.VolumeSFX;
        resDropdown.value = SaveManager.Instance.ResIndex;
        FullScreenToggle.isOn = SaveManager.Instance.isFullScreen;   
    }

    public void SetResolution(int index)
    {
        if (index < 0 || index >= filteredResolutions.Count) return;

        Resolution selectedRes = filteredResolutions[index];
        
        Screen.SetResolution(selectedRes.width, selectedRes.height, Screen.fullScreen);

        SaveManager.Instance.ResIndex = index;
        
        Debug.Log($"선택된 해상도: {selectedRes.width}x{selectedRes.height}");
    }

    public void SetFullScreen(bool isFull)
    {
        Screen.fullScreen = isFull;

        SaveManager.Instance.isFullScreen = isFull;

        Debug.Log($"전체 화면: {isFull}");
    }

    public void BGMsetting(float value)
    {
        SaveManager.Instance.VolumeBGM = value;
    }

    public void SFXsetting(float value)
    {
        SaveManager.Instance.VolumeSFX = value;
    }

    public void LobbyOptionUI(bool isActive)
    {
        OptionUI.SetActive(isActive);
    }

    /*
    해야할 것
    1. 옵션세팅을 로드 매니저로 전달하여 메인 게임 실행시 옵션이 유지되도록 하기.
    2. 로드 매니저로 메인 게임 화면으로 넘어가기. -> 씬로드 매니저를 로비씬으로 바꾸기 (완)
    3. 데이터 저장과 저장된 데이터를 서버에서 가져와 해당 데이터로 로드하는 것. -> 세이브 매니저를 로비씬으로 옮기고 저장 기능 만들기
    */
}
