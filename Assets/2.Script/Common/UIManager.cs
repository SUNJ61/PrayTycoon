using System.Collections;
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

    [SerializeField] private List<GameObject> QuestUI_List;
    [SerializeField] private List<GameObject> FailUI_List;
    [SerializeField] private List<GameObject> SummonUI_List;

    private TextMeshProUGUI quest;
    private TextMeshProUGUI questCondition;
    private TextMeshProUGUI failCondition;
    private TextMeshProUGUI summon;
    private TextMeshProUGUI summonCondition;

    public TextMeshProUGUI GoldUI;
    public TextMeshProUGUI PrayUI;
    public TextMeshProUGUI StoneUI;

    public GameObject QuestUI;
    public GameObject FailUI;
    public GameObject SummonUI;

    void Start()
    {
        QuestUI_List = ObjectManager.Instance.GetObject("UI", 1);
        FailUI_List = ObjectManager.Instance.GetObject("UI", 2);
        SummonUI_List = ObjectManager.Instance.GetObject("UI", 3);

        quest = QuestUI_List[0].GetComponent<TextMeshProUGUI>();
        questCondition = QuestUI_List[1].GetComponent<TextMeshProUGUI>();
        failCondition = FailUI_List[1].GetComponent<TextMeshProUGUI>();
        summon = SummonUI_List[0].GetComponent<TextMeshProUGUI>();
        summonCondition = SummonUI_List[1].GetComponent<TextMeshProUGUI>();

        AddQuestText();
        AddSummonText();
        AddQuestConditionText();
        AddSummonCondition();
        AddFailText();
    }

    public void QuestUIControl(bool active) //퀘스트 UI 활성화, 비활성화 함수.
    {
        QuestUI.SetActive(active);
    }

    public void FailUIControl(bool active) //실패 UI 활성화, 비활성화 함수.
    {
        FailUI.SetActive(active);
    }

    public void SummonUIControl(bool active) //소환 UI 활성화, 비활성화 함수.
    {
        SummonUI.SetActive(active);
    }

    public void QuestUIEdit(string key) //키 값에 해당하는 미션의 문구로 변경하는 함수.
    {
        quest.text = QuestText[key];
        questCondition.text = QuestConditionText[key];
    }

    public void FailUIEdit(string key) //키 값에 해당하는 미션의 실패 메세지로 변경하는 함수.
    {
        failCondition.text = FailText[key];
    }

    public void SummonUIEdit(string key) //키 값에 해당하는 미션의 문구로 변경하는 함수.
    {
        summon.text = SummonText[key];
        summonCondition.text = SummonConditonText[key];
    }

    public void CreditUIEdit()
    {
        GoldUI.text = CreditManager.Instance.credit["Gold"].ToString();
        PrayUI.text = CreditManager.Instance.credit["Pray"].ToString();
        StoneUI.text = CreditManager.Instance.credit["Stone"].ToString();
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
    }

    private void AddSummonText()
    {
        SummonText.Add("Summon", "소환을 진행하시겠습니까?");
    }

    private void AddSummonCondition()
    {
        SummonConditonText.Add("Summon", "소환을 하기 위해서는\n5의 기도력이 필요합니다.");
    }
}
