using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairUpdate : MonoBehaviour
{
    private BoxCollider2D QuestTrigger;
    private int StairCredit = 5;
    private string Key = "Stair-Main";
    private string CreditType = "Pray";

    private void Start()
    {
        QuestTrigger = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        UIManager.instance.QuestUIEdit(Key);
        QuestManager.instance.QuestCheck(Key, CreditType, StairCredit, QuestTrigger);

        UIManager.instance.QuestUIControl(true);
    }
}
