using System.Collections.Generic;

[System.Serializable]
public class UITextData
{
    public int Id;
    public string key;
    public string Title;
    public string FailText;
}

[System.Serializable]
public class UITextDatabase
{
    public List<UITextData> texts = new List<UITextData>();
}
