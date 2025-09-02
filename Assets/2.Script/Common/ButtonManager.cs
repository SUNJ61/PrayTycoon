using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : Singleton<ButtonManager>
{
    private GameObject UI;

    public Button QuestButton;
    public Button QuestCloseButton;
    public Button FailButton;
    public Button SummonButton;
    public Button SummonCloseButton;
    public Button InventoryCloseButton;

    private int CurrentQuestId;
    private int CurrentSummonId;

    void Start()
    {
        QuestButton.onClick.AddListener(QuestButtonClick);
        QuestCloseButton.onClick.AddListener(() => UIManager.Instance.QuestUIControl(false));
        FailButton.onClick.AddListener(() => UIManager.Instance.FailUIControl(false));
        SummonButton.onClick.AddListener(SummonButtonClick);
        SummonCloseButton.onClick.AddListener(() => UIManager.Instance.SummonUIControl(false));
        InventoryCloseButton.onClick.AddListener(() => UIManager.Instance.InventoryUIControl());
    }

    private void QuestButtonClick() //퀘스트 버튼 클릭시 발동하는 함수.
    {
        if (CreditManager.Instance.UseCredit
        (QuestManager.Instance.questCredit[QuestManager.Instance.currentKey], QuestManager.Instance.questCreditType[QuestManager.Instance.currentKey])) //현재 미션에 대해 크레딧이 소모 가능으로 판단하면 미션 업데이트.
        {
            UIManager.Instance.QuestUIControl(false);
            QuestManager.Instance.CompleteQuest(CurrentQuestId);
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

    public void SetCurrentQuest(int questId)
    {
        CurrentQuestId = questId;
    }

    public void SetCurrentSummon(int summonId)
    {
        CurrentSummonId = summonId;
    }
}
