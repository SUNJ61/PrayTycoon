using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveObject : MonoBehaviour
{
    [SerializeField] private string objectId;
    public bool isRepaired = false;
    public string ObjectId => objectId;

    public MapObjectData SetData()
    {
        return new MapObjectData
        {
            objectId = objectId,
            isRepaired = isRepaired
        };
    }

    public void LoadFromData(MapObjectData data)
    {
        isRepaired = data.isRepaired;

        gameObject.SetActive(isRepaired);
    }
}
