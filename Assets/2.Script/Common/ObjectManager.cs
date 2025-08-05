using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager instance;


    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(instance);
    }

    public List<GameObject> GetObject(string Ob_Name)
    {
        List<GameObject> list = new List<GameObject>();
        var ob = GameObject.Find(Ob_Name).transform;

        if (ob != null)
        {
            foreach (Transform obj in ob)
                list.Add(obj.gameObject);
        }

        return list;
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
