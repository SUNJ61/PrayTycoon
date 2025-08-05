using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    Dictionary<string, int> QuestCredit = new Dictionary<string, int>();
    Dictionary<string, bool> QuestCheck = new Dictionary<string, bool>();

    [SerializeField] private List<GameObject> Stair;
    [SerializeField] private List<GameObject> Gate;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(instance);
    }

    void Start()
    {
        Stair = ObjectManager.instance.GetObject("Stair-Main");
        Gate = ObjectManager.instance.GetObject("Gate");

        QuestCheck.Add("Stair-Main", false);
        QuestCheck.Add("Gate", false);
    }

    public void Quest(string key ,int credit)
    {
        if (!QuestCredit.ContainsKey(key)) //해당 키에 대한 값이 없을 때만 딕셔너리에 데이터 저장.
            QuestCredit.Add(key, credit);
            
        //UI 버튼 활성화 아래의 해당 내용을 버튼을 누르면 발동하도록 변경 필요함.
        if (CreaditManager.instance.UseCredit(credit))
        {
            Stair[0].gameObject.SetActive(true);
            Stair[1].gameObject.SetActive(false);

            QuestCheck[key] = true;
        }
        else
        {
            Debug.Log("크레딧 부족"); // 실패 UI 띄우기.
        }
    }

    public void GateQuest(int credit)
    {
        if (CreaditManager.instance.UseCredit(credit))
        {
            Gate[0].gameObject.SetActive(false);
            Gate[1].gameObject.SetActive(true);

            QuestCheck["Gate"] = true;
        }
        else
        {
            Debug.Log("크레딧 부족");
        }
    }

    public bool ClearCheck(string key)
    {
        bool clear;
        clear = QuestCheck[key];

        return clear;
    }
}
