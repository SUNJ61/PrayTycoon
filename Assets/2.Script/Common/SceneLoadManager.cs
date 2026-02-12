using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : Singleton<SceneLoadManager>
{
    private Vector3 PlayerspawnPoint;

    public void StartGame(string sceneName) // 새 게임 시작시 씬 로드
    {
        SceneManager.sceneLoaded += OptionDataLoad;

        SceneManager.LoadScene(sceneName);
    }

    public void LoadGame(string sceneName) // 저장된 게임 시작시 씬 로드
    {
        
    }

    public void ExitGame(string sceneName) // 게임 종료시 씬 로드 (로비 씬 이동 + 게임저장 필요.)
    {
        SceneManager.sceneLoaded += OptionDataLoad;

        SceneManager.LoadScene(sceneName);
    }

    public void NextSceneLoad(string sceneName, Vector3 spawnPoint) // 인게임 씬 전환 함수.
    {
        PlayerspawnPoint = spawnPoint;

        SceneManager.sceneLoaded += PlayerLoad;
        SceneManager.sceneLoaded += MapLoad;
        SceneManager.sceneLoaded += DataClear;

        SceneManager.LoadScene(sceneName);
    }

    private void OptionDataLoad(Scene scene, LoadSceneMode mode) // 씬 이동 시 설정 데이터 이동 (로비 -> 메인 / 메인 -> 로비) + 로그인시 데이터 적용
    {
        SceneManager.sceneLoaded -= OptionDataLoad;

        if(Object.FindAnyObjectByType<UIManager>() != null) // 메인 게임 씬 UI매니저
        {
            UIManager.Instance.resDropdown.value = SaveManager.Instance.ResIndex;
            UIManager.Instance.BGMSlider.value = SaveManager.Instance.VolumeBGM;
            UIManager.Instance.SFXSlider.value = SaveManager.Instance.VolumeSFX;
            UIManager.Instance.FullScreenToggle.isOn = SaveManager.Instance.isFullScreen;
        }
        else if(Object.FindAnyObjectByType<LobbyManager>() != null) // 로비 씬 UI 매니저
        {
            LobbyManager.Instance.resDropdown.value = SaveManager.Instance.ResIndex;
            LobbyManager.Instance.BGMSlider.value = SaveManager.Instance.VolumeBGM;
            LobbyManager.Instance.SFXSlider.value = SaveManager.Instance.VolumeSFX;
            LobbyManager.Instance.FullScreenToggle.isOn = SaveManager.Instance.isFullScreen;
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
        SaveManager.Instance.LoadMap(scene.name);
    }

    private void DataClear(Scene scene, LoadSceneMode mode) // 씬 로드 후 필요 없는 데이터 삭제.
    {
        SceneManager.sceneLoaded -= DataClear;
        QuestManager.Instance.ResetData();
    }
}
