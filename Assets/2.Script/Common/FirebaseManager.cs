using Firebase;
using Firebase.Extensions;
using Firebase.Auth;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseApp App;
    public static FirebaseManager Instance;

    private FirebaseAuth auth;
    private string dummyDomain = "@test.com";

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
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => 
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                App = Firebase.FirebaseApp.DefaultInstance;
                auth = FirebaseAuth.GetAuth(App); // 인증 객체 가져오기
                Debug.Log("Firebase 준비 완료");
            }
            else
            {
                Debug.LogError($"Firebase 인증 실패: {dependencyStatus}");
            }
        });
    }

    public void SignIn()
    {
        string email = LobbyManager.Instance.SignInIdInput.text + dummyDomain;
        string password = LobbyManager.Instance.SignInPwInput.text;

        if (password.Length < 6 || email.Length < 6) //아이디 비밀번호 6자 이하 회원가입 불가.
        {
            LobbyManager.Instance.ShowErrorText(LobbyManager.Instance.SignInErrorText.gameObject);
            return;
        }

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                LobbyManager.Instance.ShowErrorText(LobbyManager.Instance.SignInErrorText.gameObject);
                Debug.Log($"회원가입 실패");
                return;
            }

            LobbyManager.Instance.LobbySignInUI(false);
        });
    }

    public void Login()
    {
        string email = LobbyManager.Instance.LogInIdInput.text + dummyDomain;
        string password = LobbyManager.Instance.LogInPwInput.text;

        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                LobbyManager.Instance.ShowErrorText(LobbyManager.Instance.LogInErrorText.gameObject);
                Debug.Log($"로그인 실패");
                return;
            }
            
            FirebaseUser user = task.Result.User;
            Debug.Log($"로그인 성공: {user.UserId}");

            LobbyManager.Instance.LobbyLogInUI(false); //유니티 요소인 Setactive함수는 메인스레드에서만 조작가능
            LobbyManager.Instance.SetLogInUI();

            // 여기서 세이브 데이터를 불러오는 코드 추가. 로그인 된 UI로 변경되게 하는 코드 추가.
        });
    }

    public void LogOut()
    {
        if (auth.CurrentUser != null)
        {
            auth.SignOut();
            LobbyManager.Instance.SetLogOutUI();
        
            Debug.Log("로그아웃 성공");
        }
        else
        {
            Debug.LogWarning("로그아웃 상태: 현재 로그인된 유저가 없습니다.");
        }
    }
}
