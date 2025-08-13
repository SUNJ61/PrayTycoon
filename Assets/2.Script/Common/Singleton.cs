using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>(); //T 타입의 오브젝트를 찾아 인스턴스로 할당. (T는 각 매니저 클래스로 변경이 됨.)

                if (instance == null) //없으면 미리 씬에 매니저가 생성 되지 않았음을 알림.
                {
                    Debug.LogError($"{typeof(T).Name} 인스턴스를 찾을 수 없습니다.");
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance != null && instance != this) //이미 씬에 기존 instance가 존재하면 새로 생성된 instance를 제거.
        {
            Destroy(gameObject);
            return; // 기존 매니저가 있다면 Awake 종료.
        }

        instance = this as T; //기존 매니저가 없다면, T 타입에 해당하는 싱글톤 생성.

        //DontDestroyOnLoad(gameObject);
    }
}
