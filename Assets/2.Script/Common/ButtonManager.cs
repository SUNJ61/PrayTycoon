using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public static ButtonManager instance;

    private Button QuestButton;
    private Button FailButton;

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
        //버튼에 퀘스트에 필요한 크레딧을 전달 받고 해당 함수에서 퀘스트 매니저로 전달 필요.
        //해당 결과로 오브젝트 변경이 필요. (계단이 변경되는 것, 퀘스트 감지 2d콜라이더가 꺼지는 것.)
    }

    private void FailButtonClick() //실패 버튼 클릭시 발동하는 함수.
    {

    }
}
