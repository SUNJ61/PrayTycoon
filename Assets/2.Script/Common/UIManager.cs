using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private Dictionary<string, GameObject> QuestUI = new Dictionary<string, GameObject>();
    [SerializeField] private List<GameObject> QuestUI_List;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(instance);
    }

    void Start()
    {
        QuestUI_List = ObjectManager.instance.GetObject("QuestUI");
        QuestUI = ObjectManager.instance.GetDictionary(QuestUI_List);
    }

    public void QuestUIControl(string key, bool check)
    {
        if (QuestUI.ContainsKey(key))
            QuestUI[key].SetActive(check);
    }   
}
