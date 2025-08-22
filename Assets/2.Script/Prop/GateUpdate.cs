using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateUpdate : MonoBehaviour, IQuest
{
    public int QuestID { get; private set; }
    public bool IQuestClear => QuestClear; // 내부 변수 QuestClear를 읽기 위한 읽기 전용 프로퍼티 IQuestClear를 등록.


    private BoxCollider2D QuestTrigger;

    private int GateCredit = 5;

    private string Key = "Gate";
    private string CreditType = "Gold";

    private bool QuestClear = false;

    private void Start()
    {
        QuestTrigger = GetComponent<BoxCollider2D>();

        QuestID = 3;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ButtonManager.Instance.SetCurrentQuest(QuestID);
        UIManager.Instance.QuestUIEdit(Key);
        QuestManager.Instance.QuestCheck(Key, CreditType, GateCredit, this);

        UIManager.Instance.QuestUIControl(true);
    }

    public void SetQuestClear() // 퀘스트가 성공하면 발생하는 이벤트. (오브젝트 변경, 아이템 뽑기 같은 함수 넣으면 될 듯.)
    {
        QuestClear = true;

        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        gameObject.transform.GetChild(1).gameObject.SetActive(true);
        QuestTrigger.enabled = false;
    }
}
