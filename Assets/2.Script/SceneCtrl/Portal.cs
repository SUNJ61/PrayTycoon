using UnityEngine;

public class Portal : MonoBehaviour
{
    public PortalType portalType;

    private int caseId = 2;

    private void OnTriggerEnter2D(Collider2D other)
    {
        ButtonManager.Instance.SetCurrentPortal(portalType.SceneName, portalType.spawnPoint);
        ButtonManager.Instance.ButtonUpdate(caseId);

        UIManager.Instance.PortalUIEdit(portalType.Portaltype);
        UIManager.Instance.GuideUIControl(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        ButtonManager.Instance.ButtonClear();
        UIManager.Instance.GuideUIControl(false);
    }
}
