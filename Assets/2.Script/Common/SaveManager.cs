public class SaveManager : Singleton<SaveManager>
{
    private MapSaveData currentSave = new MapSaveData();

    public void SaveMap() //맵 오브젝트 데이터 저장, 씬 넘어가기 전에 호출.
    {
        currentSave.objects.Clear();
        foreach (var obj in FindObjectsOfType<SaveObject>())
            currentSave.objects.Add(obj.SetData());
    }

    public void LoadMap() //맵 오브젝트 데이터 로드, 씬 넘어온 직후 호출.
    {
        foreach (var obj in FindObjectsOfType<SaveObject>())
        {
            var data = currentSave.objects.Find(d => d.objectId == obj.ObjectId);

            if (data != null)
                obj.LoadFromData(data);
        }
    }
}
