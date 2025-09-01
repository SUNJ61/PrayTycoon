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

        string dbPath = AssetDatabase.GUIDToAssetPath(dbGuids[0]);
        ItemDatabase db = AssetDatabase.LoadAssetAtPath<ItemDatabase>(dbPath);

        // 기존 db 초기화
        db.items.Clear();

        // 프로젝트 내 모든 ItemData 검색 후 db에 추가.
        string[] itemGuids = AssetDatabase.FindAssets("t:ItemData");
        foreach (string guid in itemGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            ItemData item = AssetDatabase.LoadAssetAtPath<ItemData>(path);
            if (item != null) db.items.Add(item);
        }

        EditorUtility.SetDirty(db); //해당 오브젝트가 수정되었음을 표시후 저장
        AssetDatabase.SaveAssets(); //모든 에셋파일 저장.
    }
}
#endif
