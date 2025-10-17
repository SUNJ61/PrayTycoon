using System.Collections.Generic;

[System.Serializable]
public class UITextData
{
    public string Key;
    public string Title;
    public string Text;
    public string FailText;
}

[System.Serializable]
public class UITextDatabase
{
    public List<UITextData> Texts = new List<UITextData>();
}
