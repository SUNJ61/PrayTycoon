using UnityEngine;

[CreateAssetMenu(fileName = "PortalData", menuName = "Game/PortalData")]
public class PortalType : ScriptableObject
{
    [System.Serializable]
    public class PortalInfo
    {
        public PortalType type;
        public string sceneName;
    }

    public PortalInfo[] portalInfos;

    public string GetSceneName(PortalType type)
    {
        foreach (var info in portalInfos)
        {
            if (info.type == type)
                return info.sceneName;
        }
        
        return null;
    }
}