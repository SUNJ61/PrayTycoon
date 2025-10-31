using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuildStatue : MonoBehaviour
{
    void Start() //길드 등록 스크립트, 필요기능 소환석 등록, 자동 파밍 기능 활성화, 
    {

    }

    private void OnTigerEnter2D(Collider2D col)
    {
        UIManager.Instance.GuildUIControl(true);
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        UIManager.Instance.GuildUIControl(false);
    }
}
