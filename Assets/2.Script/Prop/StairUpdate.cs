using UnityEngine;

public class StairUpdate : MonoBehaviour, IQuest
{
    public int QuestID { get; private set; }
    public bool IQuestClear => QuestClear; // 내부 변수 QuestClear를 읽기 위한 읽기 전용 프로퍼티, IQuestClear를 호출하면 QuestClear 값 반환.

    private BoxCollider2D QuestTrigger;

    private int StairCredit = 5;

    private string Key = "Stair-Main";
    private string CreditType = "Pray";

    private bool QuestClear = false;

    private void Start()
    {
        QuestTrigger = GetComponent<BoxCollider2D>();

        QuestID = 0;

        LoadStair();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ButtonManager.Instance.SetCurrentQuest(QuestID);
        ButtonManager.Instance.ButtonUpdate(0);

        UIManager.Instance.QuestUIEdit(Key);
        QuestManager.Instance.QuestCheck(Key, CreditType, StairCredit, this);

        UIManager.Instance.GuideUIControl(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ButtonManager.Instance.ButtonClear();
        UIManager.Instance.GuideUIControl(false);
    }

    private void LoadStair()
    {
        GameObject Stair_B = gameObject.transform.GetChild(0).gameObject;
        SaveObject Stair_B_Data = Stair_B.transform.GetComponent<SaveObject>();
        Stair_B.SetActive(Stair_B_Data.isRepaired);
        QuestTrigger.enabled = Stair_B_Data.isRepaired;

        GameObject Stair = gameObject.transform.GetChild(1).gameObject;
        SaveObject Stair_Data = Stair.transform.GetComponent<SaveObject>();
        Stair.SetActive(Stair_Data.isRepaired);
        QuestClear = Stair_Data.isRepaired;
    }

    public void SetQuestClear() // 퀘스트가 성공하면 발생하는 이벤트. (오브젝트 변경, 아이템 뽑기 같은 함수 넣으면 될 듯.)
    {
        QuestClear = true;

        GameObject Stair_B = gameObject.transform.GetChild(0).gameObject;
        SaveObject Stair_B_Data = Stair_B.transform.GetComponent<SaveObject>();
        Stair_B.SetActive(false);
        Stair_B_Data.isRepaired = false;

        if(SaveManager.Instance.currentGameData[0] == null)
            Debug.Log("게임데이터 초기화 안됨.");

        
        foreach(GameData gameData in SaveManager.Instance.currentGameData) //여기서 null 발생 SaveManager가 없거나 currentGameData가 배열로 선언되어있지 않거나 일듯?
            gameData.Stair_Main = false;

        GameObject Stair = gameObject.transform.GetChild(1).gameObject;
        SaveObject Stair_Data = Stair.transform.GetComponent<SaveObject>();
        Stair.SetActive(true);
        Stair_Data.isRepaired = true;

        QuestTrigger.enabled = false;
    }
}
