using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateUpdate : MonoBehaviour
{
    private BoxCollider2D QuestTrigger;

    private int GateCredit = 5;

    private void Start()
    {
        QuestTrigger = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //기도력 소모해서 문여는 기능 구현. (UI 매니저를 통해 UI 띄우기, 버튼 누르면 기도력 소모해서 문열기.)
        QuestManager.instance.GateQuest(GateCredit);
        if (QuestManager.instance.ClearCheck("Gate"))
            QuestTrigger.enabled = false;
    }
}
