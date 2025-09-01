using UnityEngine;


[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public int Id;
    public string ItemName;
    public GameObject Icon;
    public int MaxStack = 999999;
}
