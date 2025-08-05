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

        AddQuestText();
        AddConditionText();
        AddFailText();
    }

    public void QuestUIControl(string key) //키 값은 미션을 발생시키는 오브젝트 이름으로 고정. 해당 미션으로 UI를 변경하는 함수.
    {
        TextMeshPro quest = QuestUI_List[0].gameObject.GetComponent<TextMeshPro>();
        TextMeshPro condition = QuestUI_List[1].gameObject.GetComponent<TextMeshPro>();

        quest.text = QuestText[key];
        condition.text = QuestText[key];
    }

    private void AddQuestText() //퀘스트 UI에 필요한 문구를 딕셔너리에 추가하는 함수.
    {
        QuestText.Add("Stair-Main", "Q. 계단을 수리하시겠습니까?");
    }

    private void AddConditionText() //퀘스트 UI에 필요한 문구를 딕셔너리에 추가하는 함수.
    {
        ConditionText.Add("Stair-Main", "계단을 수리하기 위해서는\n5의 기도력이 필요합니다.");
    }

    private void AddFailText() //퀘스트에 실패한 문구를 딕셔너리에 추가하는 함수.
    {
        FailText.Add("Stair-Main", "기도력이 부족합니다.");
    }
}
