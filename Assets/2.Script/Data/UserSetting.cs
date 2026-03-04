using Firebase.Firestore;

[System.Serializable] //유니티 인스팩터에서 보기 위한 
[FirestoreData] //FireBase DB 데이터 규격에 맞게 변환시켜주는 어트리뷰트
public class UserSetting
{
    [FirestoreProperty] //DB와 클래스내 변수의 이름을 제대로 매칭시키기 위해 사용. (프로퍼티로 선언되어있어야 함.)
    public float Volume_BGM {get; set;} = 10.0f;
    [FirestoreProperty]
    public float Volume_SFX {get; set;} = 10.0f;
    [FirestoreProperty]
    public int ResolutionIndex {get; set;} = 0;
    [FirestoreProperty]
    public bool isFullScreen {get; set;} = true;

    public UserSetting() {} // 서버에서 데이터를 로드를 할때 필요한 생성자. (서버에서 데이터를 해당 생성자를 통해 주입한다.)
}
