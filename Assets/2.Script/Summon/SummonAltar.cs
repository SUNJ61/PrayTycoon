using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonAltar : MonoBehaviour
{

    public List<SpriteRenderer> runes;
    private float lerpSpeed = 3;

    private Color curColor;
    private Color targetColor;

    private string Key = "Summon";

    private void Awake()
    {
        targetColor = runes[0].color;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        targetColor.a = 1.0f;
        StartCoroutine(RuneUpdate());
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
