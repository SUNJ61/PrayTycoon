using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonAltar : MonoBehaviour, IQuest
{
    public int QuestID { get; private set; }
    public bool IQuestClear => QuestClear; // 내부 변수 QuestClear를 읽기 위한 읽기 전용 프로퍼티 IQuestClear를 등록.

    public List<SpriteRenderer> runes;

    private Color curColor;
    private Color targetColor;

    private float lerpSpeed = 3;

    private int Credit = 5;

    private string Key = "Summon";
    private string CreditType = "Pray";

    private bool QuestClear = false;
    private void Awake()
    {
        targetColor = runes[0].color;

        QuestID = 1;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        targetColor.a = 1.0f;
        StartCoroutine(RuneUpdate());

        UIManager.Instance.SummonUIEdit(Key);
        QuestManager.Instance.QuestCheck(Key, CreditType, Credit, this);
        UIManager.Instance.SummonUIControl(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        targetColor.a = 0.0f;
        StartCoroutine(RuneUpdate());
    }

    private IEnumerator RuneUpdate()
    {
        while (true)
        {
            curColor = Color.Lerp(curColor, targetColor, lerpSpeed * Time.deltaTime);

            foreach (var r in runes)
            {
                r.color = curColor;
            }

            yield return null;

            if (Vector4.Distance(curColor, targetColor) < 0.02f)
                break;
        }
    }

    public void SetQuestClear() // 퀘스트 성공하면 발생하는 이벤트.
    {
        QuestClear = true;
    }
}
