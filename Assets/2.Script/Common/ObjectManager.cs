using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager instance;

    private Dictionary<string, List<GameObject>> QuestOJ = new Dictionary<string,List<GameObject>>();

    [SerializeField] private List<GameObject> Stair;
    [SerializeField] private List<GameObject> Gate;
    [SerializeField] private List<GameObject> Grave;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(instance);
    }

    void Start()
    {
        Stair = GetObject("Stair-Main");
        Gate = GetObject("Gate");
        Grave = GetObject("GraveStone");

        QuestOJ.Add("Stair-Main", Stair);
        QuestOJ.Add("Gate", Gate);
        QuestOJ.Add("GraveStone", Grave);
    }

    public List<GameObject> GetObject(string Ob_Name, int index = -1) // 해당 이름을 가진 오브젝트 자식 오브젝트를 리스트에 담는 함수, 자식 index 입력이 없으면 부모에서 리스트 생성, 있으면 자식을 찾아 리스트 생성.
    {
        Transform ob;
        List<GameObject> list = new List<GameObject>();
        
        if (index == -1)
            ob = GameObject.Find(Ob_Name).transform;
        else
            ob = GameObject.Find(Ob_Name).transform.GetChild(index).transform;

        if (ob != null)
        {
            foreach (Transform obj in ob)
                list.Add(obj.gameObject);
        }

        return list;
    }

    public void QuestObjectActive(string key, BoxCollider2D col = null) // 진행중인 퀘스트 오브젝트 관리 함수.
    {
        QuestOJ[key][0].SetActive(false);
        QuestOJ[key][1].SetActive(true);

        if (col != null)
            col.enabled = false;
        else
            Debug.Log("감지 x");
    }

    public Dictionary<string, GameObject> GetDictionary(List<GameObject> list) //딕셔너리 자동 생성 함수, UI에 쓰려다 사용처를 잃음.
    {
        Dictionary<string, GameObject> dict = new Dictionary<string, GameObject>();

        if (list != null)
        {
            foreach (GameObject item in list)
                dict.Add(item.name, item);
        }

        return dict;
    }
}
