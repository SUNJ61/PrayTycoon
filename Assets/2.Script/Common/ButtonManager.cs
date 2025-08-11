using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public static ButtonManager instance;

    private GameObject UI;

    [SerializeField] private Button QuestButton;
    [SerializeField] private Button CloseButton;
    [SerializeField] private Button FailButton;

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
        if (CreaditManager.instance.UseCredit
        (QuestManager.instance.questCredit[QuestManager.instance.currentKey], QuestManager.instance.questCreditType[QuestManager.instance.currentKey])) //현재 미션에 대해 크레딧이 소모 가능으로 판단하면 미션 업데이트.
        {
            UIManager.instance.QuestUIControl(false);
            ObjectManager.instance.QuestObjectActive(QuestManager.instance.currentKey, QuestManager.instance.currentCol);
        }
        else // 현재 미션에 대해 크레딧 소모가 불가능 하면 실패 UI 출력.
        {
            UIManager.instance.QuestUIControl(false);
            UIManager.instance.FailUIEdit(QuestManager.instance.currentKey);
            UIManager.instance.FailUIControl(true);
        }
    }

    private void CloseButtonClick() // 닫기 버튼 클릭시 발동하는 함수.
    {
        UIManager.instance.QuestUIControl(false);
    }

    private void FailButtonClick() //실패 버튼 클릭시 발동하는 함수.
    {
        UIManager.instance.FailUIControl(false);
    }
}
