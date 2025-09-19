using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public PortalType portalType;

    private void OnTriggerEnter2D(Collider2D other)
    {
            //이동 UI 띄우기.
    }

    private void OnTriggerExit2D (Collider2D other)
    {
        //이동 UI 내리기.
    }
}
