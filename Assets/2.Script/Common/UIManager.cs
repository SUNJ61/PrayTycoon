using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    private Dictionary<string, string> QuestText = new Dictionary<string, string>();
    private Dictionary<string, string> ConditionText = new Dictionary<string, string>();
    private Dictionary<string, string> FailText = new Dictionary<string, string>();
    [SerializeField] private List<GameObject> QuestUI_List;
    [SerializeField] private List<GameObject> FailUI_List;

    private TextMeshProUGUI quest;
    private TextMeshProUGUI questCondition;
    private TextMeshProUGUI failCondition;

    private GameObject UI;
    private GameObject QuestUI;
    private GameObject FailUI;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(instance);
    }

    void Start()
    {
        UI = GameObject.Find("UI");
        QuestUI = UI.transform.GetChild(1).gameObject;
        FailUI = UI.transform.GetChild(2).gameObject;

        QuestUI_List = ObjectManager.instance.GetObject("UI", 1);
        FailUI_List = ObjectManager.instance.GetObject("UI", 2);

        quest = QuestUI_List[0].GetComponent<TextMeshProUGUI>();
        questCondition = QuestUI_List[1].GetComponent<TextMeshProUGUI>();
        failCondition = FailUI_List[1].GetComponent<TextMeshProUGUI>();

        AddQuestText();
        AddConditionText();
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

    public void QuestUIEdit(string key) //키 값에 해당하는 미션의 문구로 변경하는 함수.
    {
        quest.text = QuestText[key];
        questCondition.text = ConditionText[key];
    }

    public void FailUIEdit(string key) //키 값에 해당하는 미션의 실패 메세지로 변경하는 함수.
    {
        failCondition.text = FailText[key];
    }

    private void AddQuestText() //퀘스트 UI에 필요한 문구를 딕셔너리에 추가하는 함수. "GraveStone"
    {
        QuestText.Add("Stair-Main", "Q. 계단을 수리하시겠습니까?");
        QuestText.Add("Gate", "Q. 잠긴 문을 열겠습니까?");
        QuestText.Add("GraveStone", "Q. 용병 무덤을 수리하시겠습니까?");
        QuestText.Add("Summon", "Q. 소환을 진행하시겠습니까?");
    }

    private void AddConditionText() //퀘스트 UI에 필요한 문구를 딕셔너리에 추가하는 함수.
    {
        ConditionText.Add("Stair-Main", "계단을 수리하기 위해서는\n5의 기도력이 필요합니다.");
        ConditionText.Add("Gate", "문을 열기 위해서는\n5의 기도력이 필요합니다.");
        ConditionText.Add("GraveStone", "무덤을 수리하기 위해서는\n5의 석재가 필요합니다.");
        ConditionText.Add("Summon", "소환을 하기 위해서는\n5의 기도력이 필요합니다.");
    }

    private void AddFailText() //퀘스트에 실패한 문구를 딕셔너리에 추가하는 함수.
    {
        FailText.Add("Stair-Main", "기도력이 부족합니다.");
        FailText.Add("Gate", "기도력이 부족합니다.");
        FailText.Add("GraveStone", "석재가 부족합니다.");
        FailText.Add("Summon", "기도력이 부족합니다.");
    }
}
