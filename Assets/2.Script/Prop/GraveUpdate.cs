using UnityEngine;

public class GraveUpdate : MonoBehaviour, IQuest
{
    public int QuestID { get; private set; }
    public bool IQuestClear => QuestClear; // 내부 변수 QuestClear를 읽기 위한 읽기 전용 프로퍼티, IQuestClear를 호출하면 QuestClear 값 반환.


    private int SummonID = 1;
    private int FixCredit = 5;
    private int SpawnCredit = 1;

    private string FixKey = "GraveStone";
    private string SpawnKey = "Spawn";
    private string FixCreditType = "Stone";
    private string SpawnCreditType = "Pray";

    private bool QuestClear = false;
    void Start()
    {
        QuestID = 2;

        LoadGrave();
    }

    private void OnTriggerEnter2D(Collider2D other) //수리 후와 수리 전을 구분해야함.
    {
        if (QuestClear == false) //수리가 되기 전 사용함수.
        {
            ButtonManager.Instance.SetCurrentQuest(QuestID);
            ButtonManager.Instance.ButtonUpdate(0);

            UIManager.Instance.QuestUIEdit(FixKey);
            QuestManager.Instance.QuestCheck(FixKey, FixCreditType, FixCredit, this);

            UIManager.Instance.GuideUIControl(true);
        }
        else //수리가 된 후 사용함수.
        {
            ButtonManager.Instance.SetCurrentQuest(SummonID);
            ButtonManager.Instance.ButtonUpdate(1);

            UIManager.Instance.QuestUIEdit(SpawnKey);
            QuestManager.Instance.QuestCheck(SpawnKey, SpawnCreditType, SpawnCredit, this);

            UIManager.Instance.GuideUIControl(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ButtonManager.Instance.ButtonClear();
        UIManager.Instance.GuideUIControl(false);
    }

    private void LoadGrave()
    {
        GameObject Grave_B = gameObject.transform.GetChild(0).gameObject;
        SaveObject Grave_B_Data = Grave_B.transform.GetComponent<SaveObject>();
        Grave_B.SetActive(Grave_B_Data.isRepaired);

        GameObject Grave = gameObject.transform.GetChild(1).gameObject;
        SaveObject Grave_Data = Grave.transform.GetComponent<SaveObject>();
        Grave.SetActive(Grave_Data.isRepaired);
        QuestClear = Grave_Data.isRepaired;
    }

    public void SetQuestClear() // 퀘스트가 성공하면 발생하는 이벤트. (오브젝트 변경, 아이템 뽑기 같은 함수 넣으면 될 듯.)
    {
        if (QuestClear == false) // 퀘스트 완료 전 작동 함수.
        {
            QuestClear = true;

            GameObject Grave_B = gameObject.transform.GetChild(0).gameObject;
            SaveObject Grave_B_Data = Grave_B.transform.GetComponent<SaveObject>();
            Grave_B.SetActive(false);
            Grave_B_Data.isRepaired = false;

            foreach(GameData gameData in SaveManager.Instance.currentGameData)
            gameData.Grave = false;

            GameObject Grave = gameObject.transform.GetChild(1).gameObject;
            SaveObject Grave_Data = Grave.transform.GetComponent<SaveObject>();
            Grave.SetActive(true);
            Grave_Data.isRepaired = true;
        }
    }
}
