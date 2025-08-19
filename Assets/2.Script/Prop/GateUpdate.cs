using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateUpdate : MonoBehaviour, IQuest
{
    public int QuestID { get; private set; }
    public bool IQuestClear => QuestClear;


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
        UIManager.Instance.QuestUIEdit(Key);
        QuestManager.Instance.QuestCheck(Key, CreditType, GateCredit, this, QuestTrigger);

        UIManager.Instance.QuestUIControl(true);
    }

    public void SetQuestClear() // 퀘스트가 성공하면 발생하는 이벤트.
    {
        QuestClear = true;
    }
}
