using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    private Dictionary<string, string> QuestText = new Dictionary<string, string>();
    private Dictionary<string, string> QuestConditionText = new Dictionary<string, string>();
    private Dictionary<string, string> FailText = new Dictionary<string, string>();
    private Dictionary<string, string> SummonText = new Dictionary<string, string>();
    private Dictionary<string, string> SummonConditonText = new Dictionary<string, string>();

    [SerializeField] private List<GameObject> GuideUI_List;
    [SerializeField] private List<GameObject> FailUI_List;
    [SerializeField] private List<GameObject> InventorySlot_List;

    private TextMeshProUGUI GuideUIText;
    private TextMeshProUGUI GuideCondition;
    private TextMeshProUGUI failCondition;

    public TextMeshProUGUI GoldUI;
    public TextMeshProUGUI PrayUI;
    public TextMeshProUGUI StoneUI;

    public GameObject GuideUI;
    public GameObject FailUI;
    public GameObject SummonUI;
    public GameObject InventoryUI;

    void Start()
    {
        GuideUI_List = ObjectManager.Instance.GetObject("UI", 1);
        FailUI_List = ObjectManager.Instance.GetObject("UI", 2);
        InventorySlot_List = ObjectManager.Instance.GetObject(InventoryUI, 0);

        GuideUIText = GuideUI_List[0].GetComponent<TextMeshProUGUI>();
        GuideCondition = GuideUI_List[1].GetComponent<TextMeshProUGUI>();
        failCondition = FailUI_List[1].GetComponent<TextMeshProUGUI>();

        AddQuestText();
        AddSummonText();
        AddQuestConditionText();
        AddSummonCondition();
        AddFailText();
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

    public void QuestUIEdit(string key) //키 값에 해당하는 미션의 문구로 변경하는 함수.
    {
        GuideUIText.text = QuestText[key];
        GuideCondition.text = QuestConditionText[key];
    }

    public void FailUIEdit(string key) //키 값에 해당하는 미션의 실패 메세지로 변경하는 함수.
    {
        failCondition.text = FailText[key];
    }

    public void SummonUIEdit(string key) //키 값에 해당하는 미션의 문구로 변경하는 함수.
    {
        GuideUIText.text = SummonText[key];
        GuideCondition.text = SummonConditonText[key];
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

    private void AddQuestText() //퀘스트 UI에 필요한 문구를 딕셔너리에 추가하는 함수. "GraveStone"
    {
        QuestText.Add("Stair-Main", "Q. 계단을 수리하시겠습니까?");
        QuestText.Add("Gate", "Q. 잠긴 문을 열겠습니까?");
        QuestText.Add("GraveStone", "Q. 무덤을 수리하시겠습니까?");
    }

    private void AddQuestConditionText() //퀘스트 UI에 필요한 문구를 딕셔너리에 추가하는 함수.
    {
        QuestConditionText.Add("Stair-Main", "계단을 수리하기 위해서는\n5의 기도력이 필요합니다.");
        QuestConditionText.Add("Gate", "문을 열기 위해서는\n5G의 골드가 필요합니다.");
        QuestConditionText.Add("GraveStone", "무덤을 수리하기 위해서는\n5의 석재가 필요합니다.");
    }

    private void AddFailText() //퀘스트에 실패한 문구를 딕셔너리에 추가하는 함수.
    {
        FailText.Add("Stair-Main", "기도력이 부족합니다.");
        FailText.Add("Gate", "골드가 부족합니다.");
        FailText.Add("GraveStone", "석재가 부족합니다.");
        FailText.Add("Summon", "기도력이 부족합니다.");
        FailText.Add("Spawn", "기도력이 부족합니다.");
    }

    private void AddSummonText()
    {
        SummonText.Add("Summon", "소환을 진행하시겠습니까?");
        SummonText.Add("Spawn", "Q. 소환석을 만들겠습니까?");
    }

    private void AddSummonCondition()
    {
        SummonConditonText.Add("Summon", "소환을 하기 위해서는\n5의 기도력이 필요합니다.");
        SummonConditonText.Add("Spawn", "소환석을 만들기 위해서는\n20의 기도력이 필요합니다.");
    }
}
