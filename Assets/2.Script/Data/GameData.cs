using Firebase.Firestore;
using System;

[FirestoreData]
public class GameData //게임플레이 데이터를 저장한다. (세이브 로드용)
{
    //저장 날짜
    [FirestoreProperty] public string SaveDate {get; set;} = null;

    //슬롯 정보
    [FirestoreProperty] public int SlotIndex {get; set;} = 0;

    //재화 저장
    [FirestoreProperty] public int pray {get; set;} = 0;
    [FirestoreProperty] public int stone {get; set;} = 0;
    [FirestoreProperty] public int gold {get; set;} = 0;

    //아이템 수량 저장 (int 값)
    [FirestoreProperty] public int  MineWorker_N{get; set;} = 0;
    [FirestoreProperty] public int  MineWorker_R{get; set;} = 0;
    [FirestoreProperty] public int  MineWorker_U{get; set;} = 0;

    [FirestoreProperty] public int  Knight_N{get; set;} = 0;
    [FirestoreProperty] public int  Knight_R{get; set;} = 0;
    [FirestoreProperty] public int  Knight_U{get; set;} = 0;

    [FirestoreProperty] public int  Wizard_N{get; set;} = 0;
    [FirestoreProperty] public int  Wizard_R{get; set;} = 0;
    [FirestoreProperty] public int  Wizard_U{get; set;} = 0;

    //진행도 저장 (bool 값)
    [FirestoreProperty] public bool Stair_Main {get; set;} = true;
    [FirestoreProperty] public bool Grave {get; set;} = true;
    [FirestoreProperty] public bool Gate {get; set;} = true;
    [FirestoreProperty] public bool EndingChest {get; set;} = true;

    public  GameData() {}

    public void UpdateSaveData() //현재시간 저장 함수.
    {
        SaveDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
    }
}
