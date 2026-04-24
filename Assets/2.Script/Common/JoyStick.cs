using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour,IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private RectTransform background;
    private RectTransform handle;
    private Vector2 inputVector;

    public float Horizontal => inputVector.x;
    public float Vertical => inputVector.y;

    void Awake()
    {
        background = GetComponent<RectTransform>();
        handle = transform.GetChild(0).GetComponent<RectTransform>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;

        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(background, eventData.position, eventData.pressEventCamera, out pos))
        {
            pos.x = (pos.x / background.sizeDelta.x);
            pos.y = (pos.y / background.sizeDelta.y);

            inputVector = new Vector2(pos.x *2, pos.y * 2);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            handle.anchoredPosition = new Vector2();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
    }
}
