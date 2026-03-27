using System.Collections;
using UnityEngine;

public class EndingManager : MonoBehaviour
{
    // Start is called before the first frame update
    public SpriteRenderer GemChest;

    private float MaxAlpha = 255;

    private void Awake()
    {
        StartCoroutine(EndingActive());
    }

    private IEnumerator EndingActive()
    {
        Color gemchest = GemChest.color;
        float alpha = 0;

        while(true)
        {
            alpha += Time.deltaTime;
            gemchest.a = alpha;
            GemChest.color = gemchest;

            yield return null;

            if(GemChest.color.a >= MaxAlpha)
            {
                //엔딩 멘트 UI 등장 코드 추가 필요
                break;   
            }
        }
    }

}