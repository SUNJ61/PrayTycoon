using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;
using UnityEngine.SceneManagement;

public class SaveManager : Singleton<SaveManager>
{
    private MapSaveData currentSave = new MapSaveData();
    
    private FirebaseFirestore database;

    public UserSetting currentSettings = new UserSetting(); //옵션 값 저장

    public bool LogInState = false;

    protected override void OnAwake()
    {
        database = FirebaseFirestore.DefaultInstance;
    }

    public void SaveMap() //맵 오브젝트 데이터 저장, 씬 넘어가기 전에 호출.
    {
        string sceneName = SceneManager.GetActiveScene().name;

        var list = new List<MapObjectData>();

        foreach (var obj in FindObjectsOfType<SaveObject>(true)) //맵에 존재하는 SaveObject 스크립트가 존재하는 오브젝트의 현재 데이터를 모두 저장. 
            list.Add(obj.GetData());
        
        currentSave.sceneObjects[sceneName] = list; //위에서 저장한 데이터를 딕셔너리에 씬별로 저장
    }

    public void LoadMap(string sceneName) //맵 오브젝트 데이터 로드, 씬 넘어온 직후 호출.
    {
        if (currentSave.sceneObjects.ContainsKey(sceneName))
        {
            foreach (var obj in FindObjectsOfType<SaveObject>(true))
            {
                var data = currentSave.sceneObjects[sceneName].Find(d => d.objectId == obj.ObjectId); //MapSaveData 딕셔너리에 저장된 맵 데이터에서 로드된 씬에 있는 오브젝트의 같은 ID를 찾아 데이터를 저장. 

                if (data != null)
                    obj.LoadFromData(data); //불러와진 씬 오브젝트에 딕셔너리에 저장된 데이터 덮어쓰기.
            }
        }
    }

    public void SetLobbyUI() //로그인 유무 확인후 UI 업데이트 (로비씬 호출 시 마다 작동해야함)
    {
        if(LogInState == true)
            LobbyManager.Instance.SetLogInUI();
        else
            LobbyManager.Instance.SetLogOutUI();
    }

    public void SaveSettingsToServer(string uid)
    {
        if(string.IsNullOrEmpty(uid))
        {
            return;
        }

        // "users" 컬렉션 -> "유저UID" 문서 -> "settings" 필드에 저장
        DocumentReference docRef = database.Collection("users").Document(uid); // 해당 코드에서 null이 뜬다. 오류 해결 필요.
        
        // 객체를 Dictionary나 JSON으로 변환하여 저장
        docRef.SetAsync(currentSettings).ContinueWithOnMainThread(task => {
            if (task.IsCompleted) Debug.Log("설정 저장 완료!");
        });
    }

    // --- 데이터를 서버에서 불러오기 ---
    public void LoadSettingsFromServer(string uid)
    {
        DocumentReference docRef = database.Collection("users").Document(uid);

        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task => {
            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                // 데이터를 클래스 형태로 역직렬화
                currentSettings = snapshot.ConvertTo<UserSetting>();
                ApplyLoadedSettings(); // 불러온 설정 적용
            }
            else
            {
                
            }
        });
    }

    private void ApplyLoadedSettings()
    {
        if(LobbyManager.Instance != null)
            LobbyManager.Instance.LobbyOptionSet();
    }
}
