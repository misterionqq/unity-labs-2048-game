using UnityEngine;
using UnityEngine.InputSystem;

public class SwipeInput : MonoBehaviour
{
    public TileBoard board;
    public float swipeThreshold = 50f;
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private bool isSwiping = false;

    private void Start()
    {
        if (board == null)
        {
            Debug.LogError("Ошибка: board == null в Start()! Возможно, он удаляется или не инициализирован.");
        }
        else
        {
            Debug.Log("board успешно привязан в Start()");
        }
    }
    private void Update()
    {
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            if (!isSwiping)
            {
                startTouchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
                isSwiping = true;
            }
        }
        else if (isSwiping)
        {
            if (Touchscreen.current != null) // Проверяем перед чтением
            {
                endTouchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
                DetectSwipe();
            }
            isSwiping = false;
        }

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            startTouchPosition = Mouse.current.position.ReadValue();
            isSwiping = true;
        }
        else if (Mouse.current != null && Mouse.current.leftButton.wasReleasedThisFrame)
        {
            endTouchPosition = Mouse.current.position.ReadValue();
            DetectSwipe();
            isSwiping = false;
        }
    }




    private void DetectSwipe()
    {
        if (board == null)
        {
            Debug.LogError("Ошибка: board == null в DetectSwipe()! Невозможно вызвать MoveTiles.");
            return;
        }

        Vector2 swipeDelta = endTouchPosition - startTouchPosition;
        if (swipeDelta.magnitude < swipeThreshold) return;

        Vector2Int direction;
        if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
        {
            direction = swipeDelta.x > 0 ? Vector2Int.right : Vector2Int.left;
        }
        else
        {
            direction = swipeDelta.y > 0 ? Vector2Int.up : Vector2Int.down;
        }

        Debug.Log($"Определён свайп в направлении: {direction}");
    
        board.MoveTiles(direction); // <- Возможный источник ошибки
    }

}