using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    void Awake()
    {
        GameObject[] UIobj = GameObject.FindGameObjectsWithTag("UI");

        if (UIobj.Length > 1) //중복 생성 방지. 여러개 생성되면 이후 생성된 오브젝트 삭제.
        {
            for (int i = 0; i < UIobj.Length; i++)
            {
                if (i != 0)
                    Destroy(UIobj[i]);
            }
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
}
