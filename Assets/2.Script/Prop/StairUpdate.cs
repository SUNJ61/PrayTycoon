using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairUpdate : MonoBehaviour, IQuest
{
    public int QuestID { get; private set; }
    public bool IQuestClear => QuestClear; // 내부 변수 QuestClear를 읽기 위한 읽기 전용 프로퍼티 IQuestClear를 등록.

    private BoxCollider2D QuestTrigger;

    private int StairCredit = 5;

    private string Key = "Stair-Main";
    private string CreditType = "Pray";

    private bool QuestClear = false;

    private void Start()
    {
        QuestTrigger = GetComponent<BoxCollider2D>();

        QuestID = 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        UIManager.Instance.QuestUIEdit(Key);
        QuestManager.Instance.QuestCheck(Key, CreditType, StairCredit, this, QuestTrigger);

        UIManager.Instance.QuestUIControl(true);
    }

    public void SetQuestClear() // 퀘스트 성공하면 발생하는 이벤트.
    {
        QuestClear = true;
    }
}
