using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveUpdate : MonoBehaviour
{
    private int FixCredit = 5;
    private int GraveCredit = 5;
    private string Key = "GraveStone";
    void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        UIManager.instance.QuestUIEdit(Key);
        ButtonManager.instance.QuestCheck(Key, FixCredit);

        UIManager.instance.QuestUIControl(true);
    }
}
