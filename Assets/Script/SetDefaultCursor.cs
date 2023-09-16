using UnityEngine;

public class SetDefaultCursor : MonoBehaviour
{
    public Texture2D defaultCursor;
    private Vector2 hotSpot = new Vector2(16f, 2f);

    private void Awake()
    {
        SetCursorToDefault();
    }

    public void SetCursorToDefault()
    {
        Cursor.SetCursor(defaultCursor, hotSpot, CursorMode.Auto);
    }
}
