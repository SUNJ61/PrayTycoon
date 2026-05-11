using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour,IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private RectTransform background;
    private RectTransform handle;
    private Vector2 inputVector;

    public float Horizontal;
    public float Vertical;

    void Awake()
    {
        background = GetComponent<RectTransform>();
        handle = transform.GetChild(0).GetComponent<RectTransform>();
    }

    public void OnDrag(PointerEventData eventData) // 드래그 중 실행 함수
    {
        Vector2 pos;

        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(background, eventData.position, eventData.pressEventCamera, out pos))
        {
            pos.x = pos.x / background.sizeDelta.x;
            pos.y = pos.y / background.sizeDelta.y;

            inputVector = new Vector2(pos.x *2, pos.y * 2);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            handle.anchoredPosition = new Vector2(
                inputVector.x * (background.sizeDelta.x / 2f),
                inputVector.y * (background.sizeDelta.y / 2f)
            );

            Horizontal = (inputVector.x > 0.2f) ? 1 : (inputVector.x < -0.2f) ? -1 : 0;
            Vertical = (inputVector.y > 0.2f) ? 1 : (inputVector.y < -0.2f) ? -1 : 0;
        }
    }

    public void OnPointerDown(PointerEventData eventData) // 누르는 순간 실행 함수
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData) // 화면에 손을 땔때 실행 함수
    {
        inputVector = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;

        Horizontal = 0;
        Vertical = 0;
    }
}
