using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomCursor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public Texture2D customCursor;
    private Vector2 hotSpot = Vector2.zero;
    private bool isPointerOver;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(customCursor, hotSpot, CursorMode.Auto);
        isPointerOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        isPointerOver = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StartCoroutine(ResetCursorAfterClick());
    }

    private IEnumerator ResetCursorAfterClick()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        yield return null;

        if (isPointerOver)
        {
            Cursor.SetCursor(customCursor, hotSpot, CursorMode.Auto);
        }
    }
}
