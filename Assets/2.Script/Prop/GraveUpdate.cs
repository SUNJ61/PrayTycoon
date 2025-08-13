using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveUpdate : MonoBehaviour
{
    private int FixCredit = 5;
    private int GraveCredit = 5;
    private string Key = "GraveStone";
    private string CreditType = "Stone";
    void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D other) //수리 후와 수리 전을 구분해야함.
    {
        UIManager.Instance.QuestUIEdit(Key);
        QuestManager.Instance.QuestCheck(Key, CreditType, FixCredit);

        UIManager.Instance.QuestUIControl(true);
    }
}
