using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonAltar : MonoBehaviour
{
    public List<SpriteRenderer> runes;

    private Color curColor;
    private Color targetColor;

    private float lerpSpeed = 3;

    private int Credit = 5;

    private string Key = "Summon";
    private string CreditType = "Pray";

    private void Awake()
    {
        targetColor = runes[0].color;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        targetColor.a = 1.0f;
        StartCoroutine(RuneUpdate());

        UIManager.instance.SummonUIEdit(Key);
        QuestManager.instance.QuestCheck(Key, CreditType, Credit);
        UIManager.instance.SummonUIControl(true);
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
}
