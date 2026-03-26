using UnityEngine;

public class EndingChest : MonoBehaviour, IQuest
{
    public int QuestID { get; private set; }
    public bool IQuestClear => QuestClear;

    private int FixCredit = 5;
    private int FixID = 0;
    //private int EndingCredit = 5;
    private int EndingID = 3;

    private string FixKey = "Chest";
    private string EndingKey = "Ending";
    private string FixCreditType = "Stone";
    //private string EndingCreditType = "Gold";

    private bool QuestClear = false;
    private void Start()
    {
        QuestID = 4;

        LoadEndingChest();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (QuestClear == false) //수리가 되기 전 사용함수.
        {
            ButtonManager.Instance.SetCurrentQuest(QuestID);
            ButtonManager.Instance.ButtonUpdate(FixID);

            UIManager.Instance.QuestUIEdit(FixKey);
            QuestManager.Instance.QuestCheck(FixKey, FixCreditType, FixCredit, this);

            UIManager.Instance.GuideUIControl(true);
        }
        else //수리가 된 후 사용함수.
        {
            ButtonManager.Instance.SetCurrentQuest(QuestID);
            ButtonManager.Instance.ButtonUpdate(EndingID);

            UIManager.Instance.QuestUIEdit(EndingKey);
            //QuestManager.Instance.QuestCheck(EndingKey, EndingCreditType, EndingCredit, this); //필요 없어보임. 엔딩 버튼 함수는 지정되서 연결되어있음.

            UIManager.Instance.GuideUIControl(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ButtonManager.Instance.ButtonClear();
        UIManager.Instance.GuideUIControl(false);
    }

    private void LoadEndingChest() //저장된 상자 오브젝트 불러옴.
    {
        GameObject EndingChest_C = gameObject.transform.GetChild(0).gameObject;
        SaveObject EndingChest_C_Data = EndingChest_C.GetComponent<SaveObject>();
        EndingChest_C.SetActive(EndingChest_C_Data.isRepaired);

        GameObject EndingChest_O = gameObject.transform.GetChild(1).gameObject;
        SaveObject EndingChest_O_Data = EndingChest_O.GetComponent<SaveObject>();
        EndingChest_O.SetActive(EndingChest_O_Data.isRepaired);
    }

    public void SetQuestClear() // 퀘스트가 성공하면 발생하는 이벤트. (상자 열림)
    {
        QuestClear = true;

        GameObject EndingChest_C = gameObject.transform.GetChild(0).gameObject;
        SaveObject EndingChest_C_Data = EndingChest_C.GetComponent<SaveObject>();
        EndingChest_C.SetActive(false);
        EndingChest_C_Data.isRepaired = false;

        foreach(GameData gameData in SaveManager.Instance.currentGameData) // 서버에 전달할 데이터 업데이트
            gameData.EndingChest = false;

        GameObject EndingChest_O = gameObject.transform.GetChild(1).gameObject;
        SaveObject EndingChest_O_Data = EndingChest_O.GetComponent<SaveObject>();
        EndingChest_O.SetActive(true);
        EndingChest_O_Data.isRepaired = true;
    }
}
