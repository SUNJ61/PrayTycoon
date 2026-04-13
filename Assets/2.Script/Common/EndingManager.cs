using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndingManager : MonoBehaviour
{
    // Start is called before the first frame update
    public SpriteRenderer GemChest;
    public GameObject EndingUI;

    public TextMeshProUGUI[] EndingTexts;
    public Image LobbyBTNImg;
    public TextMeshProUGUI LobbyBTNText;

    private Button LobbyBTN;
    private float MaxAlpha = 1.0f;
    private float DelayBetweenText = 0.5f;

    WaitForSeconds delay = new WaitForSeconds(0.01f);

    private void Awake()
    {
        LobbyBTN = LobbyBTNImg.gameObject.GetComponent<Button>();
        LobbyBTN.onClick.AddListener(()=>EndingBTN());

        StartCoroutine(EndingActive());
    }

    private void EndingBTN()
    {
        SoundManager.Instance.PlaySound("Button");
        SceneLoadManager.Instance.ExitGame("TycoonLobby");
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

            yield return delay;

            if(alpha >= MaxAlpha)
            {
                LobbyBTN.interactable = false;
                EndingUI.SetActive(true);
                yield return StartCoroutine(EndingUIUpdate());
                break;   
            }
        }
    }

    private IEnumerator EndingUIUpdate()
    {
        for (int i = 0; i < EndingTexts.Length; i++)
        {
            yield return StartCoroutine(FadeInText(EndingTexts[i]));
            yield return new WaitForSeconds(DelayBetweenText);
        }

        yield return StartCoroutine(FadeInBTN());
    }

    private IEnumerator FadeInText(TextMeshProUGUI text)
    {
        Color textAlpha = text.color;
        float alpha = 0;

        while(alpha <= MaxAlpha)
        {
            alpha += Time.deltaTime;
            textAlpha.a = alpha;
            text.color = textAlpha;

            yield return null;
        }
    }

    private IEnumerator FadeInBTN()
    {
        Color textAlpha = LobbyBTNImg.color;
        float alpha = 0;

         while(alpha <= MaxAlpha)
        {
            alpha += Time.deltaTime;
            textAlpha.a = alpha;
            LobbyBTNImg.color = textAlpha;
            LobbyBTNText.color = textAlpha;

            yield return null;
        }

        LobbyBTN.interactable = true;
    }
}