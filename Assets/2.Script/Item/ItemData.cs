using UnityEngine;


[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public int Id;
    public string ItemName;
    public Sprite Icon;
    public int MaxStack = 999999;
}
