using UnityEngine;

public class GateUpdate : MonoBehaviour, IQuest
{
    public int QuestID { get; private set; }
    public bool IQuestClear => QuestClear; // 내부 변수 QuestClear를 읽기 위한 읽기 전용 프로퍼티, IQuestClear를 호출하면 QuestClear 값 반환.


    private BoxCollider2D QuestTrigger;

    private int GateCredit = 5;

    private string Key = "Gate";
    private string CreditType = "Gold";

    private bool QuestClear = false;

    private void Start()
    {
        QuestTrigger = GetComponent<BoxCollider2D>();

        QuestID = 3;

        LoadGate();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ButtonManager.Instance.SetCurrentQuest(QuestID);
        ButtonManager.Instance.ButtonUpdate(0);

        UIManager.Instance.QuestUIEdit(Key);
        QuestManager.Instance.QuestCheck(Key, CreditType, GateCredit, this);

        UIManager.Instance.GuideUIControl(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ButtonManager.Instance.ButtonClear();
        UIManager.Instance.GuideUIControl(false);
    }

    private void LoadGate()
    {
        GameObject Gate_C = gameObject.transform.GetChild(0).gameObject;
        SaveObject Gate_C_Data = Gate_C.transform.GetComponent<SaveObject>();
        Gate_C.SetActive(Gate_C_Data.isRepaired);
        QuestTrigger.enabled = Gate_C_Data.isRepaired;

        GameObject Gate_O = gameObject.transform.GetChild(1).gameObject;
        SaveObject Gate_O_Data = Gate_O.transform.GetComponent<SaveObject>();
        Gate_O.SetActive(Gate_O_Data.isRepaired);
        QuestClear = Gate_O_Data.isRepaired;
    }

    public void SetQuestClear() // 퀘스트가 성공하면 발생하는 이벤트. (오브젝트 변경, 아이템 뽑기 같은 함수 넣으면 될 듯.)
    {
        QuestClear = true;

        GameObject Gate_C = gameObject.transform.GetChild(0).gameObject;
        SaveObject Gate_C_Data = Gate_C.transform.GetComponent<SaveObject>();
        Gate_C.SetActive(false);
        Gate_C_Data.isRepaired = false;

        GameObject Gate_O = gameObject.transform.GetChild(1).gameObject;
        SaveObject Gate_O_Data = Gate_O.transform.GetComponent<SaveObject>();
        Gate_O.SetActive(true);
        Gate_O_Data.isRepaired = true;

        QuestTrigger.enabled = false;
    }
}
