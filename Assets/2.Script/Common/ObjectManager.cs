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
}
