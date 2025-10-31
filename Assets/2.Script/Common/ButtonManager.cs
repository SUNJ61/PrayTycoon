using UnityEngine;
using UnityEngine.UI;
using System;

public class ButtonManager : Singleton<ButtonManager>
{
    public static event Action OnTeleport;

    public Button GuideButton;
    public Button GuideCloseButton;
    public Button FailButton;
    public Button SummonButton;
    public Button SummonCloseButton;
    public Button InventoryCloseButton;

    public Button[] GuildAddButton;

    private Vector3 CurrentspawnPoint;

    private readonly int[] GuildItemIds = { 14, 15, 16, 24, 25, 26, 34, 35, 36 };

    private int CurrentQuestId;
    private int CurrentSummonId;

    private string NextScene;


    void Start()
    {
        GuideCloseButton.onClick.AddListener(GuideCloseButtonClick);
        FailButton.onClick.AddListener(() => UIManager.Instance.FailUIControl(false));
        InventoryCloseButton.onClick.AddListener(() => UIManager.Instance.InventoryUIControl());

        for(int i = 0; i < GuildItemIds.Length; i++)
        {
            int index = i;
            GuildAddButton[i].onClick.AddListener(() => GuildAdd(index));
        }
    }

    public void ButtonUpdate(int caseId) //버튼 설정 함수.
    {
        switch (caseId)
        {
            case 0: //퀘스트 일때 ID 0번.
                GuideButton.onClick.AddListener(QuestButtonClick);
                break;

            case 1: //소환 일때 ID 1번.
                GuideButton.onClick.AddListener(SummonButtonClick);
                break;

            case 2: //포탈 일때 ID 2번.
                GuideButton.onClick.AddListener(PortalButtonClick);
                break;
        }
    }

    public void ButtonClear()
    {
        GuideButton.onClick.RemoveAllListeners();
    }

    private void QuestButtonClick() //퀘스트 버튼 클릭시 발동하는 함수.
    {
        if (CreditManager.Instance.UseCredit
        (QuestManager.Instance.questCredit[QuestManager.Instance.currentKey], QuestManager.Instance.questCreditType[QuestManager.Instance.currentKey])) //현재 미션에 대해 크레딧이 소모 가능으로 판단하면 미션 업데이트.
        {
            UIManager.Instance.GuideUIControl(false);
            QuestManager.Instance.CompleteQuest(CurrentQuestId);
        }
        else // 현재 미션에 대해 크레딧 소모가 불가능 하면 실패 UI 출력.
        {
            UIManager.Instance.GuideUIControl(false);
            UIManager.Instance.FailUIEdit(QuestManager.Instance.currentKey);
            UIManager.Instance.FailUIControl(true);
        }

        GuideButton.onClick.RemoveListener(QuestButtonClick); //버튼 초기화.
    }

    private void SummonButtonClick() //소환 버튼 클릭시 발동하는 함수.
    {
        if (CreditManager.Instance.UseCredit
        (QuestManager.Instance.questCredit[QuestManager.Instance.currentKey], QuestManager.Instance.questCreditType[QuestManager.Instance.currentKey])) //현재 미션에 대해 크레딧이 소모 가능으로 판단하면 미션 업데이트. (소환 구분 법 필요.)
        {
            UIManager.Instance.GuideUIControl(false);
            switch (CurrentSummonId)
            {
                case 0:
                    CreditManager.Instance.SummonCredit(); // 0번 골드, 석재 소환
                    break;

                case 1:
                    int ItemCode = CreditManager.Instance.SpawnRandomCode(); // 1번 소환석 소환
                    Inventory.Instance.AddItem(ItemCode, 1);
                    break;
            }
        }
        else
        {
            UIManager.Instance.GuideUIControl(false);
            UIManager.Instance.FailUIEdit(QuestManager.Instance.currentKey);
            UIManager.Instance.FailUIControl(true);
        }

        GuideButton.onClick.RemoveListener(SummonButtonClick);
    }

    private void PortalButtonClick() //포탈 버튼 클릭시 발동하는 함수.
    {
        OnTeleport?.Invoke();
        
        UIManager.Instance.GuideUIControl(false);
        GuideButton.onClick.RemoveListener(PortalButtonClick);

        SaveManager.Instance.SaveMap();

        SceneLoadManager.Instance.NextSceneLoad(NextScene, CurrentspawnPoint);
    }

    private void GuideCloseButtonClick()
    {
        GuideButton.onClick.RemoveAllListeners();
        UIManager.Instance.GuideUIControl(false);
    }

    private void GuildAdd(int caseId)
    {
        int itemId = GuildItemIds[caseId];

        if (Inventory.Instance.RemoveItem(itemId, 1))
        {
            //길드 등록후 발동되는 추가효과 함수 불러올 것.
            UIManager.Instance.GuildUIControl(false); //닫기.
        }
        else
        {
            UIManager.Instance.FailUIEdit("GuildAdd");
            UIManager.Instance.FailUIControl(true);
            //등록 아이템이 없음을 알리는 알림창 띄우기. (알림창 버튼 누르면 닫기)
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

    public void SetCurrentPortal(string sceneName, Vector3 spawnPoint)
    {
        NextScene = sceneName;
        CurrentspawnPoint = spawnPoint;
    }
}
