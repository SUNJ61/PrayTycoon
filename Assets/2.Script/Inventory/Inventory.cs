using System.Collections.Generic;
using UnityEngine;

public class Inventory : Singleton<Inventory>
{
    [SerializeField] private int SlotCount = 20; // 인벤토리 슬롯 수, 임의로 20개로 가정.

    public InventorySlot[] Slots;

    void Start()
    {
        Slots = new InventorySlot[SlotCount]; // 인벤토리 슬롯 리스트 생성, 길이는 SlotCount.

        for (int i = 0; i < SlotCount; i++) // 인벤토리 슬롯 리스트 안에 인벤토리 슬롯 데이터 저장을 위한 데이터 삽입.  
            Slots[i] = new InventorySlot();
    }

    public bool AddItem(int ItemId, int Amount)
    {
        ItemData data = ObjectManager.Instance.itemDatabase.GetItem(ItemId); // 저장할 아이템 찾기.
        if (data == null) return false;

        for (int i = 0; i < Slots.Length; i++) //기존 슬롯에 아이템 합치기.
        {
            if (Slots[i].ItemId == ItemId && Slots[i].Amount < data.MaxStack) //기존 슬롯에 같은 아이템이 존재하고, 저장된 아이템의 최대개수의 도달하지 않았다면 아래 함수 진행.
            {
                int canAdd = Mathf.Min(Amount, data.MaxStack - Slots[i].Amount); //입력 값과 저장 가능 값을 비교해 더 작은 값을 저장.
                Slots[i].Amount += canAdd; //계산한 값을 토대로 해당 슬롯에 개수 추가.
                Amount -= canAdd; //입력된 추가할 아이템의 개수에 저장된 값 차감.

                UIManager.Instance.InventoryAmountEdit(i, Slots[i].Amount);

                if (Amount <= 0) return true; //입력된 추가할 아이템의 개수가 0보다 작아지면 함수 종료.
            }
        }

        for (int i = 0; i < Slots.Length; i++) //기존 슬롯에 더이상 저장할 수 없을 경우 빈 칸에 아이템을 추가.
        {
            if (Slots[i].IsEmpty)
            {
                Slots[i].ItemId = ItemId; //빈 칸의 ItemId를 저장할 아이템의 Id로 변경.
                Slots[i].Amount = Mathf.Min(Amount, data.MaxStack); //입력된 아이템 개수가 해당 슬롯의 최대 한도와 비교, 더 적은 수 저장.
                Amount -= Slots[i].Amount; //저장된 아이템만큼 입력된 추가할 아이템의 개수에서 값 차감.

                UIManager.Instance.InventoryEmptyEdit(i, Slots[i].Amount, ItemId);

                if (Amount <= 0) return true; //입력된 추가할 아이템의 개수가 0보다 작아지면 함수 종료.
            }
        }

        Debug.Log("인벤토리 꽉참.");
        return false; //모든 과정을 거친 후 입력된 아이템이 남을 경우, 인벤토리가 꽉참. 추가 불가능.
    }

    public bool RemoveItem(int itemId, int Amount) //인벤토리에 아이템 제거 함수. (개수가 모자라거나, 슬롯의 최대 개수 보다 더 많이 제거 될때 예외 처리가 없음.)
    {
        if (Amount <= 0) //요구 개수가 0 이하면 반드시 성공.
            return true;

        if (!HasItem(itemId, Amount))
            return false; //총 개수가 부족해서 실패. (추후 UI 업데이트 필요.)

        int removeAmount = Amount; //제거할 아이템 총 개수.
        for (int i = 0; i < Slots.Length; i++)
        {
            if (Slots[i].ItemId != itemId || Slots[i].Amount <= 0) continue; //요청된 아이템의 ID가 아니거나 개수가 0보다 작거나 같으면 해당 칸은 패스.

            int remove = Mathf.Min(removeAmount, Slots[i].Amount); //제거할 아이템 개수. 입력값 or 해당 슬롯에 저장된값 중 더 작은 수.
            Slots[i].Amount -= remove; //아이템 제거.
            removeAmount -= remove; //제거할 아이템 총개수 수정.

            if (Slots[i].Amount <= 0) //빈칸으로 초기화.
            {
                Slots[i].ItemId = -1;
                Slots[i].Amount = 0;
            }

            UIManager.Instance.InventoryAmountEdit(i, Slots[i].Amount); //UI수령 조정
            UIManager.Instance.InventoryDeleteEdit(i); //UI 삭제
        }

        return true;
    }

    public bool HasItem(int ItemId, int Amount) //인벤토리에 가진 총 아이템 개수와 입력된 값을 비교하여 아이템이 충분한지 알려주는 함수.
    {
        int total = 0;
        foreach (var slot in Slots)
            if(slot.ItemId == ItemId)
                total += slot.Amount;

        return total >= Amount;
    }
}
