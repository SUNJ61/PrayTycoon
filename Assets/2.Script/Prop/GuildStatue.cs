using UnityEngine;

public class GuildStatue : MonoBehaviour
{
    //길드 등록 스크립트, 필요기능 소환석 등록, 자동 파밍 기능 활성화, 
    private void OnTriggerEnter2D(Collider2D col)
    {
        UIManager.Instance.GuildUIControl(true);
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        UIManager.Instance.GuildUIControl(false);
        UIManager.Instance.GuilAddUIControl(false);
    }
}
