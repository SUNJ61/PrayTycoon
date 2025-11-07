using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    private Dictionary<string, UITextData> textDictionary = new Dictionary<string, UITextData>();

    private UITextDatabase database;

    [SerializeField] private List<GameObject> GuideUI_List;
    [SerializeField] private List<GameObject> FailUI_List;
    [SerializeField] private List<GameObject> InventorySlot_List;

    private TextMeshProUGUI GuideUIText;
    private TextMeshProUGUI GuideCondition;
    private TextMeshProUGUI failCondition;

    public TextMeshProUGUI GoldUI;
    public TextMeshProUGUI PrayUI;
    public TextMeshProUGUI StoneUI;
    public TextMeshProUGUI[] GuildAmountUI;

    public GameObject GuideUI;
    public GameObject FailUI;
    public GameObject SummonUI;
    public GameObject InventoryUI;
    public GameObject GuildUI;
    public GameObject GuildAddUI;

    private readonly int[] GuildItemIds = { 14, 15, 16, 24, 25, 26, 34, 35, 36 };

    void Start()
    {
        GuideUI_List = ObjectManager.Instance.GetObject("UI", 1);
        FailUI_List = ObjectManager.Instance.GetObject("UI", 2);
        InventorySlot_List = ObjectManager.Instance.GetObject(InventoryUI, 0);

        GuideUIText = GuideUI_List[0].GetComponent<TextMeshProUGUI>();
        GuideCondition = GuideUI_List[1].GetComponent<TextMeshProUGUI>();
        failCondition = FailUI_List[1].GetComponent<TextMeshProUGUI>();

        LoadTextData();
    }

    private void LoadTextData()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "UITextData.json");

        if (!File.Exists(path)) //데이터 파일이 없을 경우.
        {
            Debug.LogError("UITextData.json 파일을 찾을 수 없습니다.");
            return;
        }

        string json = File.ReadAllText(path);
        database = JsonUtility.FromJson<UITextDatabase>(json); //해당 json파일을 배열로 변경.

        textDictionary.Clear();

        foreach (var data in database.Texts) //배열에 저장된 값을 딕셔너리에 저장.
        {
            if (!textDictionary.ContainsKey(data.Key))
                textDictionary.Add(data.Key, data);
        }
    }

    public void GuideUIControl(bool active) //퀘스트 UI 활성화, 비활성화 함수.
    {
        GuideUI.SetActive(active);
    }

    public void FailUIControl(bool active) //실패 UI 활성화, 비활성화 함수.
    {
        FailUI.SetActive(active);
    }

    public void InventoryUIControl()
    {
        InventoryUI.SetActive(!InventoryUI.activeSelf); //인벤토리 UI 활성화, 비활성화 함수.
    }

    public void GuildUIControl(bool active)
    {
        GuildUI.SetActive(active); //길드 UI 활성화, 비활성화 함수.
    }

    public void GuilAddUIControl(bool active)
    {
        if (active) //활성화시 갯수 초기화.
        {
            int Amount;
            for (int i = 0; i < GuildItemIds.Length; i++)
            {
                Amount = Inventory.Instance.AmountItem(GuildItemIds[i]);
                GuildAmountUI[i].text = "개수 : " + Amount.ToString();
            }
        }

        GuildAddUI.SetActive(active); //소환석 추가 UI 활성화, 비활성화 함수.
    }

    public void QuestUIEdit(string key) //키 값에 해당하는 미션의 문구로 변경하는 함수.
    {
        GuideUIText.text = textDictionary[key].Title;
        GuideCondition.text = textDictionary[key].Text;
    }

    public void FailUIEdit(string key) //키 값에 해당하는 미션의 실패 메세지로 변경하는 함수.
    {
        failCondition.text = textDictionary[key].FailText;
    }

    public void SummonUIEdit(string key) //키 값에 해당하는 소환의 문구로 변경하는 함수.
    {
        GuideUIText.text = textDictionary[key].Title;
        GuideCondition.text = textDictionary[key].Text;
    }

    public void PortalUIEdit(string key) //키 값에 해당하는 포탈의 문구로 변경하는 함수.
    {
        GuideUIText.text = textDictionary[key].Title;
        GuideCondition.text = textDictionary[key].Text;
    }

    public void CreditUIEdit()
    {
        GoldUI.text = CreditManager.Instance.credit["Gold"].ToString();
        PrayUI.text = CreditManager.Instance.credit["Pray"].ToString();
        StoneUI.text = CreditManager.Instance.credit["Stone"].ToString();
    }

    public void InventoryAmountEdit(int Index, int Amount)
    {
        TextMeshProUGUI AmountText = InventorySlot_List[Index].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        AmountText.text = Amount.ToString();
    }

    public void InventoryEmptyEdit(int Index, int Amount, int ItemID)
    {
        ItemData Item = ObjectManager.Instance.itemDatabase.GetItem(ItemID);

        Instantiate(Item.Icon, InventorySlot_List[Index].transform);
        TextMeshProUGUI AmountText = InventorySlot_List[Index].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        AmountText.text = Amount.ToString();
    }

    public void InventoryDeleteEdit(int Index)
    {
        GameObject CloneItem = InventorySlot_List[Index].transform.GetChild(1).gameObject;

        if (CloneItem != null)
            Destroy(CloneItem);
    }
}
