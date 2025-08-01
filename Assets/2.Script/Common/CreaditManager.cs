using System.Collections;
using UnityEngine;

public class CreaditManager : MonoBehaviour
{
    public static CreaditManager instance;

    private Coroutine PrayCoroutine;

    [SerializeField] private int PrayCredit;
    private int PrayAdd = 5;
    private int PrayDelay = 1;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(instance);
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
            PrayCredit += PrayAdd;
            yield return new WaitForSeconds(PrayDelay);
        }
    }

    public bool UseCredit(int credit)
    {
        bool CanUse;

        if (credit > PrayCredit)
            CanUse = false;
        else if (credit <= PrayCredit)
        {
            CanUse = true;
            PrayCredit -= credit;
        }
        else
            CanUse = false;

        return CanUse;
    }
}
