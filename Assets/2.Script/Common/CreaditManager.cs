using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreaditManager : MonoBehaviour
{
    public static CreaditManager instance;

    private Coroutine PrayCoroutine;

    [SerializeField] private Dictionary<string, int> Credit = new Dictionary<string, int>();

    private int PrayAdd = 5;
    private int PrayDelay = 1;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(instance);
    }

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

    public bool UseCredit(int credit, string key)
    {
        bool CanUse;

        if (credit > Credit[key])
            CanUse = false;
        else if (credit <= Credit[key])
        {
            CanUse = true;
            Credit[key] -= credit;
        }
        else
            CanUse = false;

        return CanUse;
    }

    private void AddCreditDic() //추후 데이터 저장 시스템 필요. 시작 할 때마다 초기화 됨.
    {
        Credit.Add("Pray", 0);
        Credit.Add("Stone", 0);
        Credit.Add("Gold", 0);
    }
}
