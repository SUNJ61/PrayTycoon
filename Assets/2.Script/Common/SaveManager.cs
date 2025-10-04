using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SaveManager : Singleton<SaveManager>
{
    private MapSaveData currentSave = new MapSaveData();

    public void SaveMap() //맵 오브젝트 데이터 저장, 씬 넘어가기 전에 호출.
    {
        string sceneName = SceneManager.GetActiveScene().name;

        var list = new List<MapObjectData>();

        foreach (var obj in FindObjectsOfType<SaveObject>(true)) //맵에 존재하는 SaveObject 스크립트가 존재하는 오브젝트의 현재 데이터를 모두 저장. 
            list.Add(obj.GetData());
        
        currentSave.sceneObjects[sceneName] = list; //위에서 저장한 데이터를 딕셔너리에 씬별로 저장
    }

    public void LoadMap(string sceneName) //맵 오브젝트 데이터 로드, 씬 넘어온 직후 호출.
    {
        if (currentSave.sceneObjects.ContainsKey(sceneName))
        {
            foreach (var obj in FindObjectsOfType<SaveObject>(true))
            {
                var data = currentSave.sceneObjects[sceneName].Find(d => d.objectId == obj.ObjectId); //MapSaveData 딕셔너리에 저장된 맵 데이터에서 로드된 씬에 있는 오브젝트의 같은 ID를 찾아 데이터를 저장. 

                if (data != null)
                    obj.LoadFromData(data); //불러와진 씬 오브젝트에 딕셔너리에 저장된 데이터 덮어쓰기.
            }
        }
    }
}
