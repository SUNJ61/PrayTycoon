using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public PortalType portalType;

    public List<SpriteRenderer> runes;

    private Color curColor;
    private Color targetColor;

    private float lerpSpeed = 3;

    private int caseId = 2;

    private bool teleport = false;

    void Awake()
    {
        targetColor = runes[0].color;
    }

    private void OnEnable()
    {
        ButtonManager.OnTeleport += MoveScene;
    }

    private void OnDisable()
    {
        ButtonManager.OnTeleport -= MoveScene;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        targetColor.a = 1.0f;
        StartCoroutine(RuneUpdate());
        ButtonManager.Instance.SetCurrentPortal(portalType.SceneName, portalType.spawnPoint);
        ButtonManager.Instance.ButtonUpdate(caseId);

        UIManager.Instance.PortalUIEdit(portalType.Portaltype);
        UIManager.Instance.GuideUIControl(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        ButtonManager.Instance.ButtonClear();
        UIManager.Instance.GuideUIControl(false);

        if (teleport == false)
        {
            targetColor.a = 0.0f;
            StartCoroutine(RuneUpdate());
        }
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

    private void MoveScene()
    {
        teleport = true;
    }
}
