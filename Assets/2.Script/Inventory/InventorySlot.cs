
public class InventorySlot
{
    public int ItemId = -1; // 해당 슬롯에 등록된 아이템 Id, -1은 빈칸.
    public int Amount = 0; // 해당 슬롯에 아이템 개수.

    public bool IsEmpty => ItemId == -1 || Amount <= 0; // 해당 슬롯이 비었는지 체크.
}
