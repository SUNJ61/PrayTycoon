using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditManager : Singleton<CreditManager>
{
    private Coroutine PrayCoroutine;

    [SerializeField] private Dictionary<string, int> Credit = new Dictionary<string, int>();
    public Dictionary<string, int> credit
    {
        get { return Credit; }
    }

    private int PrayAdd = 5;
    private int PrayDelay = 1;
    private int AddGold = 10;
    private int AddStone = 10;

    void Start()
    {
        AddCreditDic();
    }

    public void PrayCountCheck() //Pray(기도력) 증가하는지 확인, 증가 실행과 정지를를 하는 함수.
    {
        if (PrayCoroutine != null)
        {
            StopCoroutine(PrayCoroutine);
            PrayCoroutine = null;
        }
        else
            PrayCoroutine = StartCoroutine(PrayControl());
    }

    private IEnumerator PrayControl()
    {
        while (true)
        {
            Credit["Pray"] += PrayAdd;
            UIManager.Instance.CreditUIEdit();
            yield return new WaitForSeconds(PrayDelay);
        }
    }

    public bool UseCredit(int consumeCredit, string key)
    {
        if(!Credit.ContainsKey(key) || consumeCredit > Credit[key]) //딕셔너리에 key가 존재하지 않거나, 소모 재화가 보유 재화 보다 클 경우.
            return false;

        Credit[key] -= consumeCredit; //조건문에서 걸러지지 않으면 재화 소비 가능.
        UIManager.Instance.CreditUIEdit();
        return true;
    }

    public void SummonCredit() // 5:95확률로 랜덤 뽑기 함수. (기도력을 석재, 골드로 바꾸는 뽑기.)
    {
        float randomValue = Random.Range(1f, 101f);

        if (randomValue > 95f)
            Credit["Gold"] += AddGold;
        else
            Credit["Stone"] += AddStone;

        UIManager.Instance.CreditUIEdit();
    }

    public int SpawnCredit() // 10 : 20 : 70 확률로 랜덤 뽑기 함수. (소환석 뽑기.)
    {
        int itemCode;
        float Rarity = Random.Range(1f, 101f);
        float randomValue = Random.Range(1f, 101f);

        if (Rarity > 20f) // 21~100 노말
            itemCode = 10;
        else if (Rarity > 5f) // 6~19 레어
            itemCode = 20;
        else // 1~5 유니크
            itemCode = 30;

        if (randomValue > 30f) // 31~100 광부
            itemCode += 4;
        else if (randomValue > 10f) // 11~30 사냥꾼
            itemCode = 5;
        else // 1~10 마법사
            itemCode = 6;

        return itemCode;
    }

    private void AddCreditDic() //추후 데이터 저장 시스템 필요. 시작 할 때마다 초기화 됨.
    {
        Credit.Add("Pray", 0);
        Credit.Add("Stone", 0);
        Credit.Add("Gold", 0);
    }
}
