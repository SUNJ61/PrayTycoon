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

    public Button[] GuildSlot;
    public Button[] GuildAddButton;

    private Vector3 CurrentspawnPoint;

    private readonly int[] GuildItemIds = { 14, 15, 16, 24, 25, 26, 34, 35, 36 };

    private int CurrentQuestId;
    private int CurrentSummonId;
    private int CurrnetGuildSlot;

    private string NextScene;


    void Start()
    {
        GuideCloseButton.onClick.AddListener(GuideCloseButtonClick);
        FailButton.onClick.AddListener(() => UIManager.Instance.FailUIControl(false));
        InventoryCloseButton.onClick.AddListener(() => UIManager.Instance.InventoryUIControl());

        for(int i=0; i < GuildSlot.Length; i++)
        {
            if (i < 3) //길드 슬롯 추가 버튼 함수 등록
            {
                int index = i;
                GuildSlot[i].onClick.AddListener(() => GuildSlotAddBT(index));
            }

            else //길드 슬롯 제거 버튼 함수 등록
            {
                int index = i - 3;
                GuildSlot[i].onClick.AddListener(() => GuildSoltRemoveBT(index));
            }
        }

        for (int i = 0; i < GuildAddButton.Length; i++) //용병 추가 버튼 함수 등록
        {
            int index = i; //버튼 등록시 i로 등록하면 for문이 끝난후의 i값이 일괄적용 즉, 9가 적용됨.
            GuildAddButton[i].onClick.AddListener(() => AddMercenary(index));
        }
    }

    public void ButtonUpdate(int caseId) //가이드 버튼 설정 함수.
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

    private void GuildSlotAddBT(int index) //길드 슬롯에 용병추가 버튼
    {
        CurrnetGuildSlot = index;
        UIManager.Instance.GuilAddUIControl(true);
    }

    private void GuildSoltRemoveBT(int index) //길드 슬롯에 용병 제거
    {
        CreditManager.Instance.RemoveGuildSlot(index);

        UIManager.Instance.GuildSlotRemove(index);
        UIManager.Instance.GuilAddUIControl(false);
        
        CurrnetGuildSlot = -1;
    }

    private void AddMercenary(int caseId) //용병 등록
    {
        int itemId = GuildItemIds[caseId];

        if(Inventory.Instance.HasItem(itemId, 1) && CreditManager.Instance.CheckGuildSlot(CurrnetGuildSlot)) //슬롯에 용병이 있을 때 등록 성공시 (해당 슬롯에 아이콘 등록 필요.)
        {
            Debug.Log("기존 소환석 삭제 후 등록 진행");
            Inventory.Instance.RemoveItem(itemId, 1); //인벤토리 아이템 1개 제거.

            CreditManager.Instance.RemoveGuildSlot(CurrnetGuildSlot); //진행중인 추가 효과 제거.
            CreditManager.Instance.GuildSlotAdd(itemId, CurrnetGuildSlot); //길드 등록 추가 효과.

            UIManager.Instance.GuildSlotEdit(itemId, CurrnetGuildSlot);
            UIManager.Instance.GuilAddUIControl(false); //용병 추가 UI 닫기
        }
        else if(Inventory.Instance.HasItem(itemId, 1) && !CreditManager.Instance.CheckGuildSlot(CurrnetGuildSlot)) //슬롯에 용병이 없을 때 등록 성공시 (해당 슬롯에 아이콘 등록 필요.)
        {
            Debug.Log("소환석 등록");
            Inventory.Instance.RemoveItem(itemId, 1);

            CreditManager.Instance.GuildSlotAdd(itemId, CurrnetGuildSlot);

            UIManager.Instance.GuildSlotEdit(itemId, CurrnetGuildSlot);
            UIManager.Instance.GuilAddUIControl(false);
        }
        else //실패시 UI
        {
            Debug.Log("소환석 없음");
            UIManager.Instance.GuilAddUIControl(false);

            UIManager.Instance.FailUIEdit("GuildAdd");
            UIManager.Instance.FailUIControl(true);
        }

        CurrnetGuildSlot = -1;
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
