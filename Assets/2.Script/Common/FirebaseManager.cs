using Firebase;
using Firebase.Extensions;
using Firebase.Auth;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseApp App;
    public static FirebaseManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeFirebase();
        }
        else
        {
            Destroy(gameObject); // 중복 생성 방지
        }
    }

    private void InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                App = FirebaseApp.DefaultInstance;
                Debug.Log("Firebase 초기화 성공");

                TestConnection();
            }
            else
            {
                Debug.LogError($"Firebase 초기화 실패: {dependencyStatus}");
            }
        });
    }

    void TestConnection() //익명으로 로그인하는 코드 (추후 삭제 및 firebase에서 익명 로그인 기능 끄기.)
    {
        FirebaseAuth.DefaultInstance.SignInAnonymouslyAsync().ContinueWith(task =>
        {
        
        if (task.IsCompleted && !task.IsFaulted)
            Debug.Log("서버 연결 확인: 익명 로그인 성공!");
        
        else
            Debug.LogError("장비는 멀쩡한데 서버 응답이 없습니다. (인터넷 혹은 콘솔 설정 확인)");
        });
    }
}
