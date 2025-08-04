using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairUpdate : MonoBehaviour
{
    private BoxCollider2D QuestTrigger;
    private int StairCredit = 5;

    private void Start()
    {
        QuestTrigger = GetComponent<BoxCollider2D>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        UIManager.instance.QuestUIControl("Stair-Main", true);

        //기도력 소모해서 계단 수리 기능 구현. (UI 매니저를 통해 UI 띄우기, 버튼 누르면 기도력 소모해서 수리하기.)
        QuestManager.instance.StairQuest(StairCredit);

        //해당 기능은 버튼으로 계단을 수리했을 때만 발동하도록 해야함. (지금은 트리거에 들어와 있어 버튼과 무관하게 작동 중.)
        if (QuestManager.instance.ClearCheck("Stair-Main"))
            QuestTrigger.enabled = false;
    }
}
