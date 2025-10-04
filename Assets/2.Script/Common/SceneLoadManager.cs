using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : Singleton<SceneLoadManager>
{
    private Vector3 PlayerspawnPoint;

    public void NextSceneLoad(string sceneName, Vector3 spawnPoint) //씬 로드 함수.
    {
        PlayerspawnPoint = spawnPoint;

        SceneManager.sceneLoaded += PlayerLoad;
        SceneManager.sceneLoaded += MapLoad;
        SceneManager.LoadScene(sceneName);
    }

    private void PlayerLoad(Scene scene, LoadSceneMode mode) // 씬 로드후 콜백함수. 플레이어 소환
    {
        SceneManager.sceneLoaded -= PlayerLoad;
        ObjectManager.Instance.PlayerSpawn(PlayerspawnPoint);
    }

    private void MapLoad(Scene scene, LoadSceneMode mode) //씬 로드 후 콜백함수. 맵로드
    {
        SceneManager.sceneLoaded -= MapLoad;
        SaveManager.Instance.LoadMap(scene.name);
    }
}
