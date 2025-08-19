using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveUpdate : MonoBehaviour, IQuest
{
    public int QuestID { get; private set; }
    public bool IQuestClear => QuestClear; // 내부 변수 QuestClear를 읽기 위한 읽기 전용 프로퍼티 IQuestClear를 등록.


    private int FixCredit = 5;
    private int GraveCredit = 5;

    private string Key = "GraveStone";
    private string CreditType = "Stone";

    private bool QuestClear = false;
    void Start()
    {
        QuestID = 2;
    }

    private void OnTriggerEnter2D(Collider2D other) //수리 후와 수리 전을 구분해야함.
    {
        if (QuestClear == false) //수리가 되기 전 사용함수.
        {
            UIManager.Instance.QuestUIEdit(Key);
            QuestManager.Instance.QuestCheck(Key, CreditType, FixCredit, this);

            UIManager.Instance.QuestUIControl(true);
        }
        else //수리가 된 후 사용함수.
        {

        }
    }

    public void SetQuestClear() // 퀘스트 성공하면 발생하는 이벤트.
    {
        QuestClear = true;
    }
}
