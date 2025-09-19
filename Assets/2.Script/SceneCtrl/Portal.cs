using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public PortalType portalType;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //이동 UI 띄우기.
        }
    }

    private void OriggerExit(Collider other)
    {
        //이동 UI 내리기.        
    }
}
