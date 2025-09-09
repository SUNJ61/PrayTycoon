using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditManager : Singleton<CreditManager>
{
    private Coroutine PrayCoroutine;

    private Dictionary<string, int> CreditItemCode = new Dictionary<string, int>()
    {
        {"Pray", 1},
        {"Stone", 2},
        {"Gold", 3}
    };
    private Dictionary<string, int> Credit = new Dictionary<string, int>()
    {
        {"Pray", 0},
        {"Stone", 0},
        {"Gold", 0}
    }; //사용자 크레딧 정보를 저장하는 딕셔너리.
    public Dictionary<string, int> credit
    {
        get { return Credit; }
    }

    private int PrayAdd = 5;
    private int PrayDelay = 1;
    private int AddGold = 10;
    private int AddStone = 10;

    private void Start()
    {
        //추후 데이터를 받아서 크레딧 불러오기 함수를 만들면 추가 필요.
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
            Inventory.Instance.AddItem(CreditItemCode["Pray"], PrayAdd);
            yield return new WaitForSeconds(PrayDelay);
        }
    }

    public bool UseCredit(int consumeCredit, string key)
    {
        if(!Credit.ContainsKey(key) || consumeCredit > Credit[key]) //딕셔너리에 key가 존재하지 않거나, 소모 재화가 보유 재화 보다 클 경우.
            return false;

        Credit[key] -= consumeCredit; //조건문에서 걸러지지 않으면 재화 소비 가능.
        UIManager.Instance.CreditUIEdit();
        Inventory.Instance.RemoveItem(CreditItemCode[key], consumeCredit);
        return true;
    }

    public void SummonCredit() // 5:95확률로 랜덤 뽑기 함수. (기도력을 석재, 골드로 바꾸는 뽑기.)
    {
        float randomValue = Random.Range(1f, 101f);

        if (randomValue > 95f)
        {
            Credit["Gold"] += AddGold;
            Inventory.Instance.AddItem(CreditItemCode["Gold"], AddGold);
        }
        else
        {
            Credit["Stone"] += AddStone;
            Inventory.Instance.AddItem(CreditItemCode["Stone"], AddStone);
        }

        UIManager.Instance.CreditUIEdit();
    }

    public int SpawnRandomCode() // 10 : 20 : 70 확률로 랜덤 뽑기 함수. (소환석 뽑기.)
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
            itemCode += 5;
        else // 1~10 마법사
            itemCode += 6;

        return itemCode;
    }
}
