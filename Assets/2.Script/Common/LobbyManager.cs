using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public TMP_Dropdown resDropdown;
    private List<Resolution> resolutions = new List<Resolution>();
    void Start()
    {
        InitResolution();        
    }

    private void InitResolution()
    {
        Resolution[] allRes = Screen.resolutions;
        resDropdown.options.Clear();

        int currentResIndex = 0;
        Resolution nativeRes = Screen.currentResolution;

        for (int i = 0; i < allRes.Length; i++)
        {
            // 중복된 해상도 제거 (주사율만 다른 경우 등)
            if (i > 0 && allRes[i].width == allRes[i-1].width && allRes[i].height == allRes[i-1].height)
                continue;

            resolutions.Add(allRes[i]);

            string option = allRes[i].width + " x " + allRes[i].height;
            
            // 권장 해상도 표시 로직 (PC는 Native, 모바일은 기기 해상도 기준)
            if (allRes[i].width == nativeRes.width && allRes[i].height == nativeRes.height)
            {
                option += " (Recommended)";
                currentResIndex = resolutions.Count - 1;
            }

            resDropdown.options.Add(new TMP_Dropdown.OptionData(option));
        }

        resDropdown.value = currentResIndex;
        resDropdown.RefreshShownValue();
    }

    public void SetResolution(int index)
    {
        Resolution res = resolutions[index];
        // PC는 FullScreen 모드, 모바일은 기본적으로 FullScreen 처리됨
        Screen.SetResolution(res.width, res.height, true);
        Debug.Log($"해상도 변경: {res.width}x{res.height}");
    }
}
