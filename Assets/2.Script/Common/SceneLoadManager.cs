using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : Singleton<SceneLoadManager>
{
    private Vector3 PlayerspawnPoint;
    private readonly HashSet<string> WhiteList = new HashSet<string> //로비로 이동해도 삭제하면 안돼는 매니저들
    {
      "SceneLoadManager",
      "SaveManager",
      "FirebaseManager",
      "Firebase Services",
      "SoundManager"
    };

    public void StartGame(string sceneName) // 새 게임 시작시 씬 로드
    {
        SoundManager.Instance.PlaySound("Button");
        SoundManager.Instance.EndBGM();
        SoundManager.Instance.PlaySound("Main");

        SceneManager.sceneLoaded += OptionDataLoad;
        SceneManager.sceneLoaded += SaveLoadUIUpdate;

        SceneManager.LoadScene(sceneName);
    }

    public void LoadGame(string sceneName) // 저장된 게임 시작시 씬 로드
    {
        SceneManager.sceneLoaded += OptionDataLoad;
        SceneManager.sceneLoaded += SaveLoadUIUpdate;
        SceneManager.sceneLoaded += GameDataLoad;
        SceneManager.sceneLoaded += MapLoad;

        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame(string sceneName) // 게임 종료시 씬 로드
    {
        SoundManager.Instance.EndBGM();

        SceneManager.sceneLoaded += OptionDataLoad;
        SceneManager.sceneLoaded += SaveLoadUIUpdate;

        GameObject[] ManagerObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None); // DontDestroyOnLoad로 등록된 오브젝트 모두 찾기.
        foreach (GameObject ManagerObject in ManagerObjects)
        {
            if(ManagerObject.transform.parent == null && ManagerObject.scene.name == "DontDestroyOnLoad") // 부모가 없고 DontDestroyOnLoad씬에 있는 오브젝트만 남김
            {
                if(!WhiteList.Contains(ManagerObject.name)) // 화이트 리스트에 없으면 매니저 삭제
                    Destroy(ManagerObject);
            }
        }

        SaveManager.Instance.currentLoadIndex = -1; // 로비로 이동시 로드 인덱스 초기화
        SceneManager.LoadScene(sceneName); //씬로드
    }

    public void NextSceneLoad(string sceneName, Vector3 spawnPoint) // 인게임 씬 전환 함수.
    {
        PlayerspawnPoint = spawnPoint;

        SceneManager.sceneLoaded += PlayerLoad;
        SceneManager.sceneLoaded += MapLoad;
        SceneManager.sceneLoaded += DataClear;

        SceneManager.LoadScene(sceneName); //씬로드
    }

    public void EndingSceneLoad() //3초 뒤 엔딩 씬 이동
    {
        StartCoroutine(EndingStart());
    }

    private void OptionDataLoad(Scene scene, LoadSceneMode mode) // 씬 이동 시 설정 데이터 이동 (로비 -> 메인 / 메인 -> 로비) + 로그인시 데이터 적용
    {
        SceneManager.sceneLoaded -= OptionDataLoad;

        if(Object.FindAnyObjectByType<UIManager>() != null) // 메인 게임 씬 UI매니저
        {
            UIManager.Instance.resDropdown.value = SaveManager.Instance.currentSettings.ResolutionIndex;
            UIManager.Instance.BGMSlider.value = SaveManager.Instance.currentSettings.Volume_BGM;
            UIManager.Instance.SFXSlider.value = SaveManager.Instance.currentSettings.Volume_SFX;
            UIManager.Instance.FullScreenToggle.isOn = SaveManager.Instance.currentSettings.isFullScreen;

            ButtonManager.Instance.ExitBTN.onClick.AddListener(() => ExitGame("TycoonLobby")); //종료 버튼 연결
        }
        else if(Object.FindAnyObjectByType<LobbyManager>() != null) // 로비 씬 UI 매니저
        {
            LobbyManager.Instance.resDropdown.value = SaveManager.Instance.currentSettings.ResolutionIndex;
            LobbyManager.Instance.BGMSlider.value = SaveManager.Instance.currentSettings.Volume_BGM;
            LobbyManager.Instance.SFXSlider.value = SaveManager.Instance.currentSettings.Volume_SFX;
            LobbyManager.Instance.FullScreenToggle.isOn = SaveManager.Instance.currentSettings.isFullScreen;

            SaveManager.Instance.SetLobbyUI(); //로비 UI 변경
        }
    }

    private void GameDataLoad(Scene scene, LoadSceneMode mode) // 메인 게임 시작시 슬롯데이터에 따라 데이터 로드.
    {
        SceneManager.sceneLoaded -= GameDataLoad;

        SaveManager.Instance.UpdateGameData(); //게임데이터 적용
    }

    private void SaveLoadUIUpdate(Scene scene, LoadSceneMode mode) //씬 로드마다 세이브 로드 UI 업데이트 함수 (로그인 시 GameData 업데이트 필요.)
    {
        SceneManager.sceneLoaded -= SaveLoadUIUpdate;

        if(SaveManager.Instance.LogInState == false) //비로그인 시 세이브 슬롯 업데이트 하지 않음.
            return;

        if(Object.FindAnyObjectByType<UIManager>() != null) //세이브 슬롯 업데이트
        {
            UIManager.Instance.SaveSlotUpdate();
        }
        else if(Object.FindAnyObjectByType<LobbyManager>() != null) // 로드 슬롯 업데이트
        {
            LobbyManager.Instance.SetLoadDataUI(); // 로드 UI 업데이트
        }
    }

    private void PlayerLoad(Scene scene, LoadSceneMode mode) // 씬 로드후 콜백함수. 플레이어 소환
    {
        SceneManager.sceneLoaded -= PlayerLoad;
        ObjectManager.Instance.PlayerSpawn(PlayerspawnPoint);
    }

    private void MapLoad(Scene scene, LoadSceneMode mode) // 씬 로드 후 콜백함수. 맵로드
    {
        SceneManager.sceneLoaded -= MapLoad;
        
        SoundManager.Instance.EndBGM();
        if(scene.name == "TycoonMainMap")
            SoundManager.Instance.PlaySound("Main");
        else
            SoundManager.Instance.PlaySound("Room");

        SaveManager.Instance.LoadMap(scene.name);
    }

    private void DataClear(Scene scene, LoadSceneMode mode) // 씬 로드 후 필요 없는 데이터 삭제.
    {
        SceneManager.sceneLoaded -= DataClear;
        QuestManager.Instance.ResetData();
    }

    private IEnumerator EndingStart() // 3초뒤 엔딩씬으로 이동
    {
        yield return new WaitForSeconds(3.0f);

        SoundManager.Instance.EndBGM();
        SoundManager.Instance.PlaySound("Ending");

        GameObject[] ManagerObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None); // DontDestroyOnLoad로 등록된 오브젝트 모두 찾기.
        foreach (GameObject ManagerObject in ManagerObjects)
        {
            if(ManagerObject.transform.parent == null && ManagerObject.scene.name == "DontDestroyOnLoad") // 부모가 없고 DontDestroyOnLoad씬에 있는 오브젝트만 남김
            {
                if(!WhiteList.Contains(ManagerObject.name)) // 화이트 리스트에 없으면 매니저 삭제
                    Destroy(ManagerObject);
            }
        }

        SaveManager.Instance.currentLoadIndex = -1; // 로비로 이동시 로드 인덱스 초기화

        yield return null;

        SceneManager.LoadScene("TycoonEnding");
    }
}
