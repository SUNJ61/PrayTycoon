using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;
using Firebase.Auth;
using UnityEngine.SceneManagement;
using System;

public class SaveManager : Singleton<SaveManager>
{
    private MapSaveData currentMapSave = new MapSaveData();
    
    private FirebaseFirestore database;

    public UserSetting currentSettings = new UserSetting(); // 옵션 값 저장
    public GameData[] currentGameData= new GameData[3]; // 게임 데이터 저장

    public int currentLoadIndex = -1;
    public int GuildSlot0_ItemId = -1;
    public int GuildSlot1_ItemId = -1;
    public int GuildSlot2_ItemId = -1;

    public bool LogInState = false;

    protected override void OnAwake()
    {
        database = FirebaseFirestore.DefaultInstance;

        for (int i = 0; i < 3; i++) //게임 첫 실행시 데이터 초기화
            currentGameData[i] = new GameData();
    }

    public void SaveMap() //맵 오브젝트 데이터 저장, 맵(씬)) 넘어가기 전에 호출.
    {
        string sceneName = SceneManager.GetActiveScene().name;

        var list = new List<MapObjectData>();

        foreach (var obj in FindObjectsOfType<SaveObject>(true)) //맵에 존재하는 SaveObject 스크립트가 존재하는 오브젝트의 현재 데이터를 모두 저장. 
            list.Add(obj.GetData());
        
        currentMapSave.sceneObjects[sceneName] = list; //위에서 저장한 데이터를 딕셔너리에 씬별로 저장
    }

    public void LoadMap(string sceneName) //맵 오브젝트 데이터 로드, 씬 넘어온 직후 호출.
    {
        if (currentMapSave.sceneObjects.ContainsKey(sceneName)) // 한번이라도 씬이동, 맵데이터 로드를 했을 경우
        {
            foreach (var obj in FindObjectsOfType<SaveObject>(true))
            {
                var data = currentMapSave.sceneObjects[sceneName].Find(d => d.objectId == obj.ObjectId); //MapSaveData 딕셔너리에 저장된 맵 데이터에서 로드된 씬에 있는 오브젝트의 같은 ID를 찾아 데이터를 저장. 

                if (data != null)
                    obj.LoadFromData(data); //불러와진 씬 오브젝트에 딕셔너리에 저장된 데이터 덮어쓰기.
            }
        }
        else // 씬이동 이후 처음 맵을 로드할 경우. (세이브 파일 로드시)
        {
            if(currentLoadIndex == -1) // 세이브 파일을 로드하지 않았을 경우
                return;
            
            string current_sceneName = SceneManager.GetActiveScene().name;

            List<MapObjectData> list = new List<MapObjectData>();

            foreach (SaveObject obj in FindObjectsOfType<SaveObject>(true)) // 서버에서 로드한 데이터 삽입
            {
                if(current_sceneName == "TycoonMainMap")
                    MainMapIdCheck(obj);
                else if(current_sceneName == "TycoonGrave")
                    GraveMapIdCheck(obj);
                
                list.Add(obj.GetData());
            }
        
            currentMapSave.sceneObjects[current_sceneName] = list;

            foreach (var obj in FindObjectsOfType<SaveObject>(true)) // 로드한 데이터 적용
            {
                var data = currentMapSave.sceneObjects[sceneName].Find(d => d.objectId == obj.ObjectId);

                if (data != null)
                    obj.LoadFromData(data);
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
        DocumentReference docRef = database.Collection("users").Document(uid);
        
        // 객체를 Dictionary나 JSON으로 변환하여 저장
        docRef.SetAsync(currentSettings).ContinueWithOnMainThread(task => {
            if (task.IsCompleted) Debug.Log("설정 저장 완료!");
        });
    }

    public void LoadSettingsFromServer(string uid) // 데이터를 서버에서 불러오기
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
                Debug.LogError("저장된 경로에 파일이 존재하지 않습니다.");
            }
        });
    }

    private void ApplyLoadedSettings()
    {
        if(LobbyManager.Instance != null)
            LobbyManager.Instance.LobbyOptionSet();
    }



    public void SaveGameData(int SlotIndex)
    {
        if(LogInState == false) return;

        var auth = FirebaseAuth.DefaultInstance;

        string uid = auth.CurrentUser.UserId;

        DataInput(SlotIndex);

        DocumentReference docRef = database.Collection("users").Document(uid).Collection("SaveSlots").Document($"Slot{SlotIndex}"); //경로 설정

        docRef.SetAsync(currentGameData[SlotIndex]).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                UIManager.Instance.SaveUpdate(SlotIndex);
            }
            else if (task.IsFaulted)
            {
                Debug.LogError($"{SlotIndex}번 슬롯 저장 실패: {task.Exception}");
            }
        });
    }

    public void LoadGameDataFromServer()
    {
        if(LogInState == false) return;
        
        var auth = FirebaseAuth.DefaultInstance;

        string uid = auth.CurrentUser.UserId;

        database.Collection("users").Document(uid).Collection("SaveSlots")
            .GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("데이터 로드 실패: " + task.Exception);
                    return;
                }

                QuerySnapshot snapshot = task.Result;

                // 데이터 배열 초기화 (로그아웃 후 다시 로그인 하면 이전 데이터가 남을 수 있기 때문.)
                for (int i = 0; i < 3; i++) currentGameData[i] = new GameData();

                // 서버에서 받아온 데이터 채우기
                foreach (DocumentSnapshot doc in snapshot.Documents)
                {
                    // 문서 이름에 Slot이 들어가는가
                    if (doc.Id.StartsWith("Slot"))
                    {
                        // 들어가면 Slot을 제외하고 숫자만 추출 후 인덱스로 저장.
                        int slotIndex = int.Parse(doc.Id.Replace("Slot", ""));

                        if (slotIndex >= 0 && slotIndex < 3)
                            currentGameData[slotIndex] = doc.ConvertTo<GameData>();
                    }
                }

                LobbyManager.Instance.SetLoadDataUI(); // 로드 UI 업데이트
            });
    }

    public void UpdateGameData() // 로드 게임 시 플레이 데이터 업데이트 코드
    {
        CreditManager.Instance.credit["Pray"] = currentGameData[currentLoadIndex].pray;
        CreditManager.Instance.credit["Stone"] = currentGameData[currentLoadIndex].stone;
        CreditManager.Instance.credit["Gold"] = currentGameData[currentLoadIndex].gold;

        Inventory.Instance.AddItem(1, currentGameData[currentLoadIndex].pray);
        Inventory.Instance.AddItem(2, currentGameData[currentLoadIndex].stone);
        Inventory.Instance.AddItem(3, currentGameData[currentLoadIndex].gold);

        UIManager.Instance.CreditUIEdit();

        Inventory.Instance.AddItem(14, currentGameData[currentLoadIndex].MineWorker_N);
        Inventory.Instance.AddItem(24, currentGameData[currentLoadIndex].MineWorker_R);
        Inventory.Instance.AddItem(34, currentGameData[currentLoadIndex].MineWorker_U);

        Inventory.Instance.AddItem(15, currentGameData[currentLoadIndex].Knight_N);
        Inventory.Instance.AddItem(25, currentGameData[currentLoadIndex].Knight_R);
        Inventory.Instance.AddItem(35, currentGameData[currentLoadIndex].Knight_U);

        Inventory.Instance.AddItem(16, currentGameData[currentLoadIndex].Wizard_N);
        Inventory.Instance.AddItem(26, currentGameData[currentLoadIndex].Wizard_R);
        Inventory.Instance.AddItem(36, currentGameData[currentLoadIndex].Wizard_U);

        for(int i = 0; i < 3; i++)
        {
            if(currentGameData[currentLoadIndex].MercenaryIds[i] != -1) //빈 슬롯이 아닐 경우
            {
                CreditManager.Instance.GuildSlotAdd(currentGameData[currentLoadIndex].MercenaryIds[i],i);
                UIManager.Instance.GuildSlotEdit(currentGameData[currentLoadIndex].MercenaryIds[i],i);
            }
        }

        if(currentGameData[currentLoadIndex].EndingChest == false) //엔딩 상호작용이 이미 되었을 경우.
            SceneLoadManager.Instance.EndingSceneLoad();
        
        //지형, 사물 업데이트는 맵 로드시 작동.
    }

    private void DataInput(int SlotIndex) // 저장할 데이터를 최신화
    {
        currentGameData[SlotIndex].SaveDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm"); //저장 시간 업데이트.

        //슬롯정보 저장
        currentGameData[SlotIndex].SlotIndex = SlotIndex;

        //재화 저장
        currentGameData[SlotIndex].pray = CreditManager.Instance.credit["Pray"];
        currentGameData[SlotIndex].stone = CreditManager.Instance.credit["Stone"];
        currentGameData[SlotIndex].gold = CreditManager.Instance.credit["Gold"];

        //소환석 저장
        currentGameData[SlotIndex].MineWorker_N = Inventory.Instance.AmountItem(14);
        currentGameData[SlotIndex].MineWorker_R = Inventory.Instance.AmountItem(24);
        currentGameData[SlotIndex].MineWorker_U = Inventory.Instance.AmountItem(34);

        currentGameData[SlotIndex].Knight_N = Inventory.Instance.AmountItem(15);
        currentGameData[SlotIndex].Knight_R = Inventory.Instance.AmountItem(25);
        currentGameData[SlotIndex].Knight_U = Inventory.Instance.AmountItem(35);

        currentGameData[SlotIndex].Wizard_N = Inventory.Instance.AmountItem(16);
        currentGameData[SlotIndex].Wizard_R = Inventory.Instance.AmountItem(26);
        currentGameData[SlotIndex].Wizard_U = Inventory.Instance.AmountItem(36);

        //길드 용병 저장 추가 필요
        currentGameData[SlotIndex].MercenaryIds[0] = GuildSlot0_ItemId;
        currentGameData[SlotIndex].MercenaryIds[1] = GuildSlot1_ItemId;
        currentGameData[SlotIndex].MercenaryIds[2] = GuildSlot2_ItemId;
    }

    private void MainMapIdCheck(SaveObject obj)
    {
        if(obj.ObjectId == "Stair Broken - Main")
            obj.isRepaired = currentGameData[currentLoadIndex].Stair_Main;

        if(obj.ObjectId == "Stair - Main")
            obj.isRepaired = !currentGameData[currentLoadIndex].Stair_Main;

        if(obj.ObjectId == "Gate Close")
            obj.isRepaired = currentGameData[currentLoadIndex].Gate;

        if(obj.ObjectId == "Gate Open")
            obj.isRepaired = !currentGameData[currentLoadIndex].Gate;

        if(obj.ObjectId == "EndingChest_Close")
            obj.isRepaired = currentGameData[currentLoadIndex].EndingChest;

        if(obj.ObjectId == "EndingChest_Open")
            obj.isRepaired = !currentGameData[currentLoadIndex].EndingChest;
    }

    private void GraveMapIdCheck(SaveObject obj)
    {
        if(obj.ObjectId == "Broken Pillar")
            obj.isRepaired = currentGameData[currentLoadIndex].Grave;

        if(obj.ObjectId == "Pillar")
            obj.isRepaired = !currentGameData[currentLoadIndex].Grave;
    }
}
