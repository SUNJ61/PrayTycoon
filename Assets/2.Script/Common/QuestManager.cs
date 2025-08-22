using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    private Dictionary<string, int> QuestCredit = new Dictionary<string, int>();
    public Dictionary<string, int> questCredit
    {
        get { return QuestCredit; }
    }
    private Dictionary<string, string> QuestCreditType = new Dictionary<string, string>();
    public Dictionary<string, string> questCreditType
    {
        get { return QuestCreditType; }
    }
    private Dictionary<int, IQuest> QuestObj = new Dictionary<int, IQuest>();

    private BoxCollider2D CurrentCol = null;
    public BoxCollider2D currentCol
    {
        get { return CurrentCol; }
    }

    private string CurrentKey = null;
    public string currentKey
    {
        get { return CurrentKey; }
    }

    public void QuestCheck(string key, string creditType, int credit, IQuest quest) //플레이어 미션, 미션 키 업데이트, 미션에 필요한 크레딧 저장, 미션을 반응하게 하는 콜라이더 저장.
    {
        CurrentKey = key; //플레이어가 진행중인 미션을 업데이트.

        if (!QuestCredit.ContainsKey(key)) //해당 키에 대한 값이 없을 때만 딕셔너리에 데이터 저장.
            QuestCredit.Add(key, credit);

        if (!QuestCreditType.ContainsKey(key))
            QuestCreditType.Add(key, creditType);

         if (!QuestObj.ContainsKey(quest.QuestID))
            QuestObj.Add(quest.QuestID, quest);
    }

    public void CompleteQuest(int questID) //퀘스트 완료시 클리어 함수를 불러옴.
    {
        if (QuestObj.TryGetValue(questID, out IQuest quest)) // 입력된 QuestID와 같은 키가 등록되어있다면 해당 키에 등록된 값을 출력.
            quest.SetQuestClear();
        else
            Debug.Log("퀘스트가 등록되지 않음.");
    }

    public bool QuestState(int questID) // 현재 퀘스트 상태를 체크하는 함수.
    {
        return QuestObj.TryGetValue(questID, out IQuest quest) && quest.IQuestClear;
    }
}
