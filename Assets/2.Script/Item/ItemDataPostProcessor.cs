#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class ItemDataPostprocessor : AssetPostprocessor
{
    static void OnPostprocessAllAssets(
        string[] importedAssets, string[] deletedAssets,
        string[] movedAssets, string[] movedFromAssetPaths)
    {
        // ItemDatabase 에셋 찾기
        string[] dbGuids = AssetDatabase.FindAssets("t:ItemDatabase");
        if (dbGuids.Length == 0) return; // DB 없으면 무시

        // DataBase의 경로를 찾고, db에 데이터 저장.
        string dbPath = AssetDatabase.GUIDToAssetPath(dbGuids[0]);
        ItemDatabase db = AssetDatabase.LoadAssetAtPath<ItemDatabase>(dbPath);

        var newList = new System.Collections.Generic.List<ItemData>();
        string[] itemGuids = AssetDatabase.FindAssets("t:ItemData");
        foreach (string guid in itemGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            ItemData item = AssetDatabase.LoadAssetAtPath<ItemData>(path);
            if (item != null) newList.Add(item);
        }

        // 기존 데이터와 다를 때만 업데이트
        bool changed = false;

        if (db.items.Count != newList.Count) //db 길이 가 변경됬을 경우. (아이템 추가.)
        {
            changed = true;
        }
        else
        {
            for (int i = 0; i < db.items.Count; i++) //db 내부의 아이템이 삭제되고 추가 되었을 경우. (길이는 같지만 저장된 아이템이 달라짐.)
            {
                if (db.items[i] != newList[i])
                {
                    changed = true;
                    break;
                }
            }
        }

        if (changed)
        {
            db.items = newList;
            EditorUtility.SetDirty(db);
        }
    }
}
#endif
