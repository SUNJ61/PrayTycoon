using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    private Dictionary<string, UITextData> textDictionary = new Dictionary<string, UITextData>();

    private UITextDatabase database;

    [SerializeField] private List<GameObject> GuideUI_List;
    [SerializeField] private List<GameObject> FailUI_List;
    [SerializeField] private List<GameObject> InventorySlot_List;
    [SerializeField] private List<GameObject> GuildSlot_List;

    private TextMeshProUGUI GuideUIText;
    private TextMeshProUGUI GuideCondition;
    private TextMeshProUGUI failCondition;

    public TextMeshProUGUI GoldUI;
    public TextMeshProUGUI PrayUI;
    public TextMeshProUGUI StoneUI;
    public TextMeshProUGUI[] GuildAmountUI;

    public TextMeshProUGUI[] SaveSlotText;

    public TMP_Dropdown resDropdown; //해상도 가져오기 로직 넣어야함, 정렬 변경 후 추가 예정
    public Slider BGMSlider;
    public Slider SFXSlider;
    public Toggle FullScreenToggle;

    private GameObject GuildSolt1 = null;
    private GameObject GuildSolt2 = null;
    private GameObject GuildSolt3 = null;
    public GameObject GuideUI;
    public GameObject FailUI;
    public GameObject SummonUI;
    public GameObject InventoryUI;
    public GameObject GuildUI;
    public GameObject GuildAddUI;
    public GameObject MenuUI;
    public GameObject OptionUI;
    public GameObject SaveUI;
    public GameObject SaveDataUI;

    private readonly int[] GuildItemIds = {14, 15, 16, 24, 25, 26, 34, 35, 36};
    private readonly List<int> standardWidths = new List<int> { 1280, 1600, 1920, 2560, 3840 };
    private List<Resolution> filteredResolutions = new List<Resolution>();

    void Start()
    {
        GuideUI_List = ObjectManager.Instance.GetObject("UI", 1);
        FailUI_List = ObjectManager.Instance.GetObject("UI", 3);
        InventorySlot_List = ObjectManager.Instance.GetObject(InventoryUI, 0);
        GuildSlot_List = ObjectManager.Instance.GetObject(GuildUI, 1);

        GuideUIText = GuideUI_List[0].GetComponent<TextMeshProUGUI>();
        GuideCondition = GuideUI_List[1].GetComponent<TextMeshProUGUI>();
        failCondition = FailUI_List[1].GetComponent<TextMeshProUGUI>();

        BGMSlider.onValueChanged.AddListener(BGMsetting);
        SFXSlider.onValueChanged.AddListener(SFXsetting);

        LoadTextData();
        InitResolution();
    }

    private void LoadTextData()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "UITextData.json");

        if (!File.Exists(path)) //데이터 파일이 없을 경우.
        {
            Debug.LogError("UITextData.json 파일을 찾을 수 없습니다.");
            return;
        }

        string json = File.ReadAllText(path);
        database = JsonUtility.FromJson<UITextDatabase>(json); //해당 json파일을 배열로 변경.

        textDictionary.Clear();

        foreach (var data in database.Texts) //배열에 저장된 값을 딕셔너리에 저장.
        {
            if (!textDictionary.ContainsKey(data.Key))
                textDictionary.Add(data.Key, data);
        }
    }

    private void InitResolution()
    {
        Resolution[] allResolutions = Screen.resolutions;
        resDropdown.options.Clear();

        // 현재 모니터의 종횡비 계산
        float targetAspect = (float)Screen.currentResolution.width / Screen.currentResolution.height;

        for (int i = 0; i < allResolutions.Length; i++)
        {
            float currentAspect = (float)allResolutions[i].width / allResolutions[i].height;

            //필터링, 위에서 계산한 비율과 비슷한 값의 표준 해상도만 선택.
            if (Mathf.Abs(currentAspect - targetAspect) < 0.01f)
            {
                if (standardWidths.Contains(allResolutions[i].width))
                {
                    // 중복 제거 (주사율 차이 무시)
                    if (filteredResolutions.Exists(r => r.width == allResolutions[i].width))
                        continue;

                    filteredResolutions.Add(allResolutions[i]);
                }
            }
        }

        //만약 필터링된게 하나도 없다면(특수 해상도), 현재 해상도는 무조건 추가
        if (filteredResolutions.Count == 0)
        {
            filteredResolutions.Add(Screen.currentResolution);
        }

        filteredResolutions.Reverse(); // 리스트 역순으로 뒤집기 (저해상도 먼저 등록됨)

        // 드롭다운 UI 업데이트
        foreach (var res in filteredResolutions)
        {
            string option = $"{res.width} x {res.height}";
            if (res.width == Screen.currentResolution.width) option += " (권장)";
            resDropdown.options.Add(new TMP_Dropdown.OptionData(option));
        }

        resDropdown.onValueChanged.AddListener(OnResolutionChanged);
        
        resDropdown.RefreshShownValue();
    }

    private void OnResolutionChanged(int index)
    {
        // filteredResolutions 리스트에서 선택된 인덱스의 해상도 정보를 가져옴
        Resolution selectedRes = filteredResolutions[index];
    
        // 실제 해상도 변경 적용
        Screen.SetResolution(selectedRes.width, selectedRes.height, Screen.fullScreen);
    
        // 변경된 인덱스를 SaveManager 등에 저장
        SaveManager.Instance.currentSettings.ResolutionIndex = index;
        Debug.Log($"해상도 변경: {selectedRes.width}x{selectedRes.height}");
    }

    public void SetFullScreen(bool isFull)
    {
        Screen.fullScreen = isFull;

        SaveManager.Instance.currentSettings.isFullScreen = isFull;

        Debug.Log($"전체 화면: {isFull}");
    }

    public void BGMsetting(float value)
    {
        SaveManager.Instance.currentSettings.Volume_BGM = value;
    }

    public void SFXsetting(float value)
    {
        SaveManager.Instance.currentSettings.Volume_SFX = value;
    }


    public void GuideUIControl(bool active) //퀘스트 UI 활성화, 비활성화 함수.
    {
        GuideUI.SetActive(active);
    }

    public void FailUIControl(bool active) //실패 UI 활성화, 비활성화 함수.
    {
        FailUI.SetActive(active);
    }

    public void InventoryUIControl()
    {
        InventoryUI.SetActive(!InventoryUI.activeSelf); //인벤토리 UI 활성화, 비활성화 함수.
    }

    public void GuildUIControl(bool active)
    {
        GuildUI.SetActive(active); //길드 UI 활성화, 비활성화 함수.
    }

    public void MenuUIControl()
    {
        MenuUI.SetActive(!MenuUI.activeSelf);
    }

    public void OptionUIControl()
    {
        if(OptionUI.activeSelf == true && SaveManager.Instance.LogInState == true)
        {
            string uid = Firebase.Auth.FirebaseAuth.DefaultInstance.CurrentUser.UserId;
            SaveManager.Instance.SaveSettingsToServer(uid);
        }

        MenuUI.SetActive(false);
        OptionUI.SetActive(!OptionUI.activeSelf);
    }

    public void SaveUIControl() // 세이브 확정 UI 띄우기
    {
        MenuUI.SetActive(false);
        SaveUI.SetActive(!SaveUI.activeSelf);
    }

    public void SetSaveData(int slotIndex) // 데이터 세이브 확인 UI 띄우기
    {
        ButtonManager.Instance.SaveSlotIndex(slotIndex);
        SaveDataUI.SetActive(!SaveDataUI.activeSelf);
    }

    public void UIOff()
    {
        GuideUI.SetActive(false);
        FailUI.SetActive(false);
        InventoryUI.SetActive(false);
        GuildUI.SetActive(false);
        SaveUI.SetActive(false);
    }

    public void GuilAddUIControl(bool active)
    {
        if (active) //활성화시 갯수 초기화.
        {
            int Amount;
            for (int i = 0; i < GuildItemIds.Length; i++)
            {
                Amount = Inventory.Instance.AmountItem(GuildItemIds[i]);
                GuildAmountUI[i].text = "개수 : " + Amount.ToString();
            }
        }

        GuildAddUI.SetActive(active); //소환석 추가 UI 활성화, 비활성화 함수.
    }

    public void QuestUIEdit(string key) //키 값에 해당하는 미션의 문구로 변경하는 함수.
    {
        GuideUIText.text = textDictionary[key].Title;
        GuideCondition.text = textDictionary[key].Text;
    }

    public void FailUIEdit(string key) //키 값에 해당하는 미션의 실패 메세지로 변경하는 함수.
    {
        failCondition.text = textDictionary[key].FailText;
    }

    public void PortalUIEdit(string key) //키 값에 해당하는 포탈의 문구로 변경하는 함수.
    {
        GuideUIText.text = textDictionary[key].Title;
        GuideCondition.text = textDictionary[key].Text;
    }

    public void CreditUIEdit()
    {
        GoldUI.text = CreditManager.Instance.credit["Gold"].ToString();
        PrayUI.text = CreditManager.Instance.credit["Pray"].ToString();
        StoneUI.text = CreditManager.Instance.credit["Stone"].ToString();
    }

    public void InventoryAmountEdit(int Index, int Amount)
    {
        TextMeshProUGUI AmountText = InventorySlot_List[Index].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        AmountText.text = Amount.ToString();
    }

    public void InventoryEmptyEdit(int Index, int Amount, int ItemID)
    {
        ItemData Item = ObjectManager.Instance.itemDatabase.GetItem(ItemID);

        Instantiate(Item.Icon, InventorySlot_List[Index].transform);
        TextMeshProUGUI AmountText = InventorySlot_List[Index].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        AmountText.text = Amount.ToString();
    }

    public void InventoryDeleteEdit(int Index) //인벤토리 0개 물품 제거
    {
        GameObject CloneItem = InventorySlot_List[Index].transform.GetChild(1).gameObject;

        if (CloneItem != null)
            Destroy(CloneItem);
    }

    public void GuildSlotEdit(int ItemID, int Index) //길드 슬롯에 소환석 추가시 UI 추가 함수.
    {
        ItemData Item = ObjectManager.Instance.itemDatabase.GetItem(ItemID);
        RectTransform Size;

        switch (Index)
        {
            case 0:
                if (GuildSolt1 != null)
                    GuildSolt1 = null;
                GuildSolt1 = Instantiate(Item.Icon, GuildSlot_List[Index].transform);
                Size = GuildSolt1.GetComponent<RectTransform>();

                Size.localScale = new Vector3(2.0f, 2.0f, 1f);
                break;

            case 1:
                if (GuildSolt2 != null)
                    GuildSolt2 = null;
                GuildSolt2 = Instantiate(Item.Icon, GuildSlot_List[Index].transform);
                Size = GuildSolt2.GetComponent<RectTransform>();

                Size.localScale = new Vector3(2.0f, 2.0f, 1f);
                break;

            case 2:
                if (GuildSolt3 != null)
                    GuildSolt3 = null;
                GuildSolt3 = Instantiate(Item.Icon, GuildSlot_List[Index].transform);
                Size = GuildSolt3.GetComponent<RectTransform>();

                Size.localScale = new Vector3(2.0f, 2.0f, 1f);
                break;
        }
    }
    
    public void GuildSlotRemove(int Index) //길드 슬롯 제거 함수
    {
        switch (Index)
        {
            case 0:
                if (GuildSolt1 != null)
                    Destroy(GuildSolt1);
                
                GuildSolt1 = null;
                break;

            case 1:
                if (GuildSolt2 != null)
                    Destroy(GuildSolt2);
                
                GuildSolt2 = null;
                break;

            case 2:
                if (GuildSolt3 != null)
                    Destroy(GuildSolt3);
                
                GuildSolt3 = null;
                break;
        }
    }
}
