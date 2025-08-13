using System.Collections;
using System.Collections.Generic;
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

    public void QuestCheck(string key, string creditType, int credit, BoxCollider2D col = null) //플레이어 미션 키 업데이트, 미션에 필요한 크레딧 저장, 미션을 반응하게 하는 콜라이더 저장. (퀘스트 매니저에 있어도 무방할듯?)
    {
        CurrentKey = key; //플레이어가 진행중인 미션을 업데이트.
        CurrentCol = col; //진행중인 미션을 판단하는 콜라이더 업데이트.

        if (!QuestCredit.ContainsKey(key)) //해당 키에 대한 값이 없을 때만 딕셔너리에 데이터 저장.
            QuestCredit.Add(key, credit);

        if (!QuestCreditType.ContainsKey(key))
            QuestCreditType.Add(key, creditType);
    }
}
