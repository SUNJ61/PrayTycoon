using System.Collections.Generic;

[System.Serializable]
public class MapObjectData
{
    public string objectId;   // 고유 ID
    public bool isRepaired;   // 수리 여부
}

[System.Serializable]
public class MapSaveData
{
    public List<MapObjectData> objects = new();
}
