using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager Instance;

    public GameObject OptionUI;
    public GameObject LogInUI;
    public GameObject SignInUI;
    public GameObject LogInMenuBTN; 
    public GameObject SignInMenuBTN;
    public GameObject LogOutBTN;

    public Button LogInBTN;
    public Button SignInBTN;

    public TMP_Dropdown resDropdown;
    public Slider BGMSlider;
    public Slider SFXSlider;
    public Toggle FullScreenToggle;

    public TMP_InputField LogInIdInput;
    public TMP_InputField LogInPwInput;
    public TMP_Text LogInErrorText;

    public TMP_InputField SignInIdInput;
    public TMP_InputField SignInPwInput;
    public TMP_Text SignInErrorText;

    private readonly List<int> standardWidths = new List<int> { 1280, 1600, 1920, 2560, 3840 };
    private List<Resolution> filteredResolutions = new List<Resolution>();

    private Coroutine ErrorCorutine;
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        if(SaveManager.Instance != null)
            SaveManager.Instance.SetLobbyUI(); //로비 UI 변경
        
        LobbyBTNSet();
        InitResolution();
        LobbyOptionSet();

        BGMSlider.onValueChanged.AddListener(BGMsetting);
        SFXSlider.onValueChanged.AddListener(SFXsetting);
    }

    private void LobbyBTNSet()
    {
        Button LogOutbtn = LogOutBTN.GetComponent<Button>();

        if (FirebaseManager.Instance != null)
        {
            LogInBTN.onClick.RemoveAllListeners();
            SignInBTN.onClick.RemoveAllListeners();
            LogOutbtn.onClick.RemoveAllListeners();

            LogInBTN.onClick.AddListener(FirebaseManager.Instance.Login);
            SignInBTN.onClick.AddListener(FirebaseManager.Instance.SignIn);
            LogOutbtn.onClick.AddListener(FirebaseManager.Instance.LogOut);
        }
    }
    private void InitResolution()
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

        filteredResolutions.Reverse(); // 리스트 역순으로 뒤집기 (저해상도 먼저 등록됨)

        // 드롭다운 UI 업데이트
        foreach (var res in filteredResolutions)
        {
            string option = $"{res.width} x {res.height}";
            if (res.width == Screen.currentResolution.width) option += " (권장)";
            resDropdown.options.Add(new TMP_Dropdown.OptionData(option));
        }

        resDropdown.onValueChanged.AddListener(OnResolutionChanged);
        
        resDropdown.RefreshShownValue();
    }

    private void OnResolutionChanged(int index)
    {
        // filteredResolutions 리스트에서 선택된 인덱스의 해상도 정보를 가져옴
        Resolution selectedRes = filteredResolutions[index];
    
        // 실제 해상도 변경 적용
        Screen.SetResolution(selectedRes.width, selectedRes.height, Screen.fullScreen);
    
        // 변경된 인덱스를 SaveManager 등에 저장
        SaveManager.Instance.currentSettings.ResolutionIndex = index;
        Debug.Log($"해상도 변경: {selectedRes.width}x{selectedRes.height}");
    }

    public void LobbyOptionSet() //기본 설정으로 초기화
    {
        BGMSlider.value = SaveManager.Instance.currentSettings.Volume_BGM;
        SFXSlider.value = SaveManager.Instance.currentSettings.Volume_SFX;
        resDropdown.value = SaveManager.Instance.currentSettings.ResolutionIndex;
        FullScreenToggle.isOn = SaveManager.Instance.currentSettings.isFullScreen;
    }

    public void SetResolution(int index)
    {
        if (index < 0 || index >= filteredResolutions.Count) return;

        Resolution selectedRes = filteredResolutions[index];
        
        Screen.SetResolution(selectedRes.width, selectedRes.height, Screen.fullScreen);

        SaveManager.Instance.currentSettings.ResolutionIndex = index;
        
        
        Debug.Log($"선택된 해상도: {selectedRes.width}x{selectedRes.height}");
    }

    public void SetFullScreen(bool isFull)
    {
        Screen.fullScreen = isFull;

        SaveManager.Instance.currentSettings.isFullScreen = isFull;

        Debug.Log($"전체 화면: {isFull}");
    }

    public void BGMsetting(float value)
    {
        SaveManager.Instance.currentSettings.Volume_BGM = value;
    }

    public void SFXsetting(float value)
    {
        SaveManager.Instance.currentSettings.Volume_SFX = value;
    }

    public void LobbyOptionUI(bool isActive)
    {
        if(isActive == false && SaveManager.Instance.LogInState == true)
        {
            string uid = Firebase.Auth.FirebaseAuth.DefaultInstance.CurrentUser.UserId;
            SaveManager.Instance.SaveSettingsToServer(uid);
            Debug.Log("설정 저장됨");
        }

        OptionUI.SetActive(isActive);
    }

    public void LobbyLogInUI(bool isActive)
    {
        LogInIdInput.text = "Id...";
        LogInPwInput.text = "Pw...";
        LogInUI.SetActive(isActive);
        Debug.Log("입력됨 : " + isActive);
    }

    public void LobbySignInUI(bool isActive)
    {
        SignInIdInput.text = "Id...";
        SignInPwInput.text = "Pw...";
        SignInUI.SetActive(isActive);
        Debug.Log("입력됨 : " + isActive);
    }

    public void SetLogInUI() //로그인 했을 시
    {
        LogInMenuBTN.SetActive(false);
        SignInMenuBTN.SetActive(false);
        LogOutBTN.SetActive(true);

        SaveManager.Instance.LogInState = true;
    }

    public void SetLogOutUI() //로그아웃 했을 시
    {
        LogInMenuBTN.SetActive(true);
        SignInMenuBTN.SetActive(true);
        LogOutBTN.SetActive(false);

        SaveManager.Instance.LogInState = false;
    }

    public void CloseBTN(GameObject BTN)
    {
        GameObject parentObj = BTN.transform.parent.gameObject;

        if(parentObj != null)
        {
            parentObj.SetActive(false);
        }
    }

    public void ShowErrorText(GameObject ErrorObj,float delay = 2.0f)
    {
        if (ErrorCorutine != null)
            StopCoroutine(ErrorCorutine);

        ErrorCorutine = StartCoroutine(ShowError(ErrorObj, delay));
    }

    private IEnumerator ShowError(GameObject ErrorObj, float delay)
    {
        ErrorObj.SetActive(true);

        yield return new WaitForSeconds(delay);

        ErrorObj.SetActive(false);
    }

    /*
    해야할 것
    1. 옵션세팅을 로드 매니저로 전달하여 메인 게임 실행시 옵션이 유지되도록 하기. (완)
    2. 로드 매니저로 메인 게임 화면으로 넘어가기. -> 씬로드 매니저를 로비씬으로 바꾸기 (완)
    3. firebaase서버 연동하기. (완)
    4. 로그인 시스템 만들기. (완)
    5. 유저 id 기반으로 설정 데이터를 저장하고 로그인시 불러오는 기능 만들기.
    6. 유저 id 기반으로 게임 세이브, 로드 기능 만들기.
    */
}
