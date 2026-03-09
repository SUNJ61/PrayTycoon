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
            }
            else
            {
                
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
                return;
            }
            
            FirebaseUser user = task.Result.User;

            SaveManager.Instance.LogInState = true;
            LobbyManager.Instance.LobbyLogInUI(false); //유니티 요소 함수는 메인스레드에서만 조작가능
            LobbyManager.Instance.SetLogInUI();

            string uid = task.Result.User.UserId;
        
            SaveManager.Instance.LoadSettingsFromServer(uid); // 로그인 시 옵션 데이터 가져오기.
            SaveManager.Instance.LoadGameData(); // 로그인 시 게임 세이브 파일 데이터 가져오기.
        });
    }

    public void LogOut()
    {
        if (auth.CurrentUser != null)
        {
            auth.SignOut();
            SaveManager.Instance.LogInState = false;
            LobbyManager.Instance.SetLogOutUI();
        
        }
        else
        {
            
        }
    }
}
