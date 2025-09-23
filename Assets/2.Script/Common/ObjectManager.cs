using System.Collections;
using System.Collections.Generic;
using Cainos.PixelArtTopDown_Basic;
using UnityEngine;

public class ObjectManager : Singleton<ObjectManager>
{
    private Dictionary<string, List<GameObject>> QuestOJ = new Dictionary<string, List<GameObject>>();

    [SerializeField] private List<GameObject> Stair;
    [SerializeField] private List<GameObject> Gate;
    //[SerializeField] private List<GameObject> Grave;

    private ItemDatabase _itemDatabase;
    public ItemDatabase itemDatabase
    {
        get { return _itemDatabase; }
    }

    [SerializeField]private GameObject PlayerObj;
    [SerializeField]private GameObject CurrentPlayer;
    private GameObject MainCamera;

    private CameraFollow cameraFollow;

    private Vector3 spawnPoint = new Vector3(-4, -10, 0);

    public override void Awake()
    {
        base.Awake();

        _itemDatabase = Resources.Load<ItemDatabase>("ItemDataBase");
        PlayerObj = Resources.Load<GameObject>("Player");
    }
    void Start()
    {
        Stair = GetObject("Stair-Main");
        Gate = GetObject("Gate");
        //Grave = GetObject("GraveStone");

        QuestOJ.Add("Stair-Main", Stair);
        QuestOJ.Add("Gate", Gate);
        //QuestOJ.Add("GraveStone", Grave);

        PlayerSpawn(spawnPoint);
    }

    public List<GameObject> GetObject(string Ob_Name, int index = -1) // 해당 이름을 가진 오브젝트의 자식 오브젝트의 자식을 리스트에 담는 함수, 자식 index 입력이 없으면 입력된 오브젝트 자식 리스트 생성.
    {
        Transform parent;
        List<GameObject> list = new List<GameObject>();

        if (index == -1)
            parent = GameObject.Find(Ob_Name).transform;
        else
            parent = GameObject.Find(Ob_Name).transform.GetChild(index).transform;

        if (parent != null)
        {
            foreach (Transform child in parent)
                list.Add(child.gameObject);
        }

        return list;
    }

    public List<GameObject> GetObject(GameObject Obj, int index = -1) // 해당 오브젝트의 자식 오브젝트의 자식을 리스트에 담는 함수, 자식 index 입력이 없으면 입력된 오브젝트 자식 리스트 생성.
    {
        Transform parent;
        List<GameObject> list = new List<GameObject>();

        if (index == -1)
            parent = Obj.transform;
        else
            parent = Obj.transform.GetChild(index).transform;

        if (parent != null)
        {
            foreach (Transform child in parent)
                list.Add(child.gameObject);
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

    public void PlayerSpawn(Vector3 spawnPoint)
    {
        //플레이어 스폰관리.
        CurrentPlayer = Instantiate(PlayerObj, spawnPoint, Quaternion.identity);

        CameraCtrl();
    }

    public void CameraCtrl()
    {
        //카메라 붙이기.
        MainCamera = GameObject.Find("Main Camera");
        cameraFollow = MainCamera.GetComponent<CameraFollow>();

        cameraFollow.target = CurrentPlayer.transform;
    }
}
