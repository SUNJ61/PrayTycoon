using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public static ButtonManager instance;

    private Dictionary<string, int> QuestCredit = new Dictionary<string, int>();

    private GameObject UI;

    [SerializeField] private Button QuestButton;
    [SerializeField] private Button CloseButton;
    [SerializeField] private Button FailButton;

    private BoxCollider2D CurrentCol = null;

    private string CurrentKey = null;

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

        QuestButton = UI.transform.GetChild(1).GetChild(2).GetComponent<Button>();
        CloseButton = UI.transform.GetChild(1).GetChild(3).GetComponent<Button>();
        FailButton = UI.transform.GetChild(2).GetChild(2).GetComponent<Button>();

        QuestButton.onClick.AddListener(QuestButtonClick);
        CloseButton.onClick.AddListener(CloseButtonClick);
        FailButton.onClick.AddListener(FailButtonClick);
    }

    private void QuestButtonClick() //퀘스트 버튼 클릭시 발동하는 함수.
    {
        if (CreaditManager.instance.UseCredit(QuestCredit[CurrentKey])) //현재 미션에 대해 크레딧이 소모 가능으로 판단하면 미션 업데이트. (소모 재화가 기도력, 골드, 석재가 될 수 있음 업데이트 필요.)
        {
            UIManager.instance.QuestUIControl(false);
            ObjectManager.instance.QuestObjectActive(CurrentKey, CurrentCol);

            CurrentCol = null; //퀘스트 수정 후 판단 변수 초기화.
        }
        else // 현재 미션에 대해 크레딧 소모가 불가능 하면 실패 UI 출력.
        {
            UIManager.instance.QuestUIControl(false);
            UIManager.instance.FailUIEdit(CurrentKey);
            UIManager.instance.FailUIControl(true);
        }

        CurrentKey = null; //퀘스트 수정 후 판단 변수 초기화.
    }

    private void CloseButtonClick() // 닫기 버튼 클릭시 발동하는 함수.
    {
        UIManager.instance.QuestUIControl(false);
    }

    private void FailButtonClick() //실패 버튼 클릭시 발동하는 함수.
    {
        UIManager.instance.FailUIControl(false);
    }

    public void QuestCheck(string key, int credit, BoxCollider2D col = null) //플레이어 미션 키 업데이트, 미션에 필요한 크레딧 저장, 미션을 반응하게 하는 콜라이더 저장.
    {
        CurrentKey = key; //플레이어가 진행중인 미션을 업데이트.
        CurrentCol = col; //진행중인 미션을 판단하는 콜라이더 업데이트.

        if (!QuestCredit.ContainsKey(key)) //해당 키에 대한 값이 없을 때만 딕셔너리에 데이터 저장.
            QuestCredit.Add(key, credit);
    }
}
