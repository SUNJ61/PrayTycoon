using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "PortalData", menuName = "Portal/PortalData")]
public class PortalType : ScriptableObject
{
    public Vector3 spawnPoint;
    public string Portaltype;
    public string SceneName;
}