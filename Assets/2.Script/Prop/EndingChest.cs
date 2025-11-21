using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingChest : MonoBehaviour, IQuest
{
    public int QuestID { get; private set; }
    public bool IQuestClear => QuestClear;

    private int FixCredit = 5;
    private int EndingCredit = 5;

    private string FixKey = "Chest";
    private string EndingKey = "Ending";
    private string FixCreditType = "Stone";
    private string EndingCreditType = "Gold";

    private bool QuestClear = false;
    private void Start()
    {
        QuestID = 4;
    }

    private void OnTiggerEnter2D(Collider2D collision)
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
            ButtonManager.Instance.SetCurrentQuest(QuestID);
            ButtonManager.Instance.ButtonUpdate(1);

            UIManager.Instance.QuestUIEdit(EndingKey);
            QuestManager.Instance.QuestCheck(EndingKey, EndingCreditType, EndingCredit, this);

            UIManager.Instance.GuideUIControl(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ButtonManager.Instance.ButtonClear();
        UIManager.Instance.GuideUIControl(false);
    }

    private void LoadEndingChest()
    {
        
    }

    public void SetQuestClear() // 퀘스트가 성공하면 발생하는 이벤트.
    {
        //열린 상자가 되도록 해야함.
    }
}
