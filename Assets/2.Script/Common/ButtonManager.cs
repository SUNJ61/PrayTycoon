using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : Singleton<ButtonManager>
{
    private GameObject UI;

    [SerializeField] private Button QuestButton;
    [SerializeField] private Button QuestCloseButton;
    [SerializeField] private Button FailButton;
    [SerializeField] private Button SummonButton;
    [SerializeField] private Button SummonCloseButton;

    void Start()
    {
        UI = GameObject.Find("UI"); //퍼블릭으로 잡기, 퍼블릭으로 연결하는게 메모리에 할당하는 방법이라, 순회 구조보다 더 가벼움.

        QuestButton = UI.transform.GetChild(1).GetChild(2).GetComponent<Button>();
        QuestCloseButton = UI.transform.GetChild(1).GetChild(3).GetComponent<Button>();
        FailButton = UI.transform.GetChild(2).GetChild(2).GetComponent<Button>();
        SummonButton = UI.transform.GetChild(3).GetChild(2).GetComponent<Button>();
        SummonCloseButton = UI.transform.GetChild(3).GetChild(3).GetComponent<Button>();

        QuestButton.onClick.AddListener(QuestButtonClick);
        QuestCloseButton.onClick.AddListener(() => UIManager.Instance.QuestUIControl(false));
        FailButton.onClick.AddListener(() => UIManager.Instance.FailUIControl(false));
        SummonButton.onClick.AddListener(SummonButtonClick);
        SummonCloseButton.onClick.AddListener(() => UIManager.Instance.SummonUIControl(false));
    }

    private void QuestButtonClick() //퀘스트 버튼 클릭시 발동하는 함수.
    {
        if (CreditManager.Instance.UseCredit
        (QuestManager.Instance.questCredit[QuestManager.Instance.currentKey], QuestManager.Instance.questCreditType[QuestManager.Instance.currentKey])) //현재 미션에 대해 크레딧이 소모 가능으로 판단하면 미션 업데이트.
        {
            UIManager.Instance.QuestUIControl(false);
            ObjectManager.Instance.QuestObjectActive(QuestManager.Instance.currentKey, QuestManager.Instance.currentCol);
        }
        else // 현재 미션에 대해 크레딧 소모가 불가능 하면 실패 UI 출력.
        {
            UIManager.Instance.QuestUIControl(false);
            UIManager.Instance.FailUIEdit(QuestManager.Instance.currentKey);
            UIManager.Instance.FailUIControl(true);
        }
    }

    private void SummonButtonClick() //소환 버튼 클릭시 발동하는 함수.
    {
        if (CreditManager.Instance.UseCredit
        (QuestManager.Instance.questCredit[QuestManager.Instance.currentKey], QuestManager.Instance.questCreditType[QuestManager.Instance.currentKey])) //현재 미션에 대해 크레딧이 소모 가능으로 판단하면 미션 업데이트. (소환 구분 법 필요.)
        {
            UIManager.Instance.SummonUIControl(false);
            CreditManager.Instance.SummonCredit();
        }
        else
        {
            UIManager.Instance.SummonUIControl(false);
            UIManager.Instance.FailUIEdit(QuestManager.Instance.currentKey);
            UIManager.Instance.FailUIControl(true);
        }
    }
}
