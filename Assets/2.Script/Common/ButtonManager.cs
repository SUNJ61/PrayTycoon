using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public static ButtonManager instance;

    private Dictionary<string, int> QuestCredit = new Dictionary<string, int>();

    private Button QuestButton;
    private Button FailButton;

    private string CurrentKey;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(instance);
    }

    void Start()
    {
        QuestButton = GameObject.Find("QuestButton").GetComponent<Button>();
        FailButton = GameObject.Find("FailButton").GetComponent<Button>();

        QuestButton.onClick.AddListener(QuestButtonClick);
        FailButton.onClick.AddListener(FailButtonClick);
    }

    private void QuestButtonClick() //퀘스트 버튼 클릭시 발동하는 함수.
    {
        //버튼이 현재 어떤 퀘스트를 진행 중인지 알아야함. (완)
        //버튼에 퀘스트에 필요한 크레딧을 전달 받고 해당 함수에서 퀘스트 매니저로 전달 필요. (완)
        //해당 결과로 오브젝트 변경이 필요. (계단이 변경되는 것, 퀘스트 감지 2d콜라이더가 꺼지는 것.)
        //퀘스트 매니저가 필요한가? 해당 버튼의 결과에 따라 크레딧 매니저, 오브젝트 매니저에서 값을 변경하면 되는 것 아닌가?

        //퀘스트에 필요한 크레딧, 키값은 미션을 발생시키는 오브젝트에서 전달 (딕셔너리로 저장, 이미 존재하면 저장 x)
        //퀘스트 완료시 해당 딕셔너리 삭제 및 크레딧 매니저에서 크레딧 감소, 오브젝트 매니저에서 오브젝트 상태 변경 (ex. 계단 변경과 플레이어 감지하는 콜라이더 비활성화 등.)
        //남은 퀘스트 매니저의 역할은 퀘스트가 완료 되었는지 확인하는 역할.
    }

    private void FailButtonClick() //실패 버튼 클릭시 발동하는 함수.
    {

    }

    public void QuestCheck(string key, int credit)
    {
        CurrentKey = key; //플레이어가 진행중인 미션을 업데이트.

        if (!QuestCredit.ContainsKey(key)) //해당 키에 대한 값이 없을 때만 딕셔너리에 데이터 저장.
            QuestCredit.Add(key, credit);
    }
}
