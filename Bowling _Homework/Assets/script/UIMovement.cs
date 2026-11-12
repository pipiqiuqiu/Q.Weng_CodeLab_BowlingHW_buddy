using UnityEngine;
using UnityEngine.InputSystem;
public class UIMovement : MonoBehaviour
{
    public float minY = -1200f;
    public float maxY = -245f;
    public float speed = 300f;

    private RectTransform rectTransform;
    private bool movingUp = true;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (!Mouse.current.leftButton.isPressed)
        {
            return;
        }
        
        
        float targetY;

        if (movingUp)
        {
            targetY = maxY; 
            
        }else targetY = minY;

        Vector2 pos = rectTransform.anchoredPosition;

        pos.y = Mathf.MoveTowards(pos.y, targetY, speed * Time.deltaTime);

        rectTransform.anchoredPosition = pos;

        if (Mathf.Approximately(pos.y, targetY))
        {
            movingUp = !movingUp;
        }
    }
}