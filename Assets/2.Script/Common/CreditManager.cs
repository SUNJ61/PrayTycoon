using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditManager : Singleton<CreditManager>
{
    private Coroutine PrayCoroutine;

    [SerializeField] private Dictionary<string, int> Credit = new Dictionary<string, int>();

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
            yield return new WaitForSeconds(PrayDelay);
        }
    }

    public bool UseCredit(int consumeCredit, string key)
    {
        if(!Credit.ContainsKey(key) || consumeCredit > Credit[key]) //딕셔너리에 key가 존재하지 않거나, 소모 재화가 보유 재화 보다 클 경우.
            return false;

        Credit[key] -= consumeCredit; //조건문에서 걸러지지 않으면 재화 소비 가능.
        return true;
    }

    public void SummonCredit() // 3:7확률로 랜덤 뽑기 함수.
    {
        float randomValue = Random.Range(0f, 100f);

        if (randomValue > 70f)
            Credit["Gold"] += AddGold;
        else
            Credit["Stone"] += AddStone;
    }

    private void AddCreditDic() //추후 데이터 저장 시스템 필요. 시작 할 때마다 초기화 됨.
    {
        Credit.Add("Pray", 0);
        Credit.Add("Stone", 0);
        Credit.Add("Gold", 0);
    }
}
