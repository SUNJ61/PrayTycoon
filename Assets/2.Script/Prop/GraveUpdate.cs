using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveUpdate : MonoBehaviour
{
    private int QuestID = 2;
    private int FixCredit = 5;
    private int GraveCredit = 5;

    private string Key = "GraveStone";
    private string CreditType = "Stone";

    private bool QuestClear = false;
    void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D other) //수리 후와 수리 전을 구분해야함.
    {
        if (QuestClear == false) //수리가 되기 전 사용함수.
        {
            UIManager.Instance.QuestUIEdit(Key);
            QuestManager.Instance.QuestCheck(Key, CreditType, FixCredit);

            UIManager.Instance.QuestUIControl(true);
        }
        else //수리가 된 후 사용함수.
        {

        }
    }
}
