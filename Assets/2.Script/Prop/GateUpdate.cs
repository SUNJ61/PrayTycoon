using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateUpdate : MonoBehaviour
{
    private BoxCollider2D QuestTrigger;

    private int GateCredit = 5;

    private string Key = "Gate";
    private string CreditType = "Gold";

    private void Start()
    {
        QuestTrigger = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        UIManager.instance.QuestUIEdit(Key);
        QuestManager.instance.QuestCheck(Key, CreditType, GateCredit, QuestTrigger);

        UIManager.instance.QuestUIControl(true);
    }
}
