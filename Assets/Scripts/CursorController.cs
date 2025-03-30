using UnityEngine;

public class CursorController
{
    private readonly Transform cursor;

    public CursorController(Transform cursor)
    {
        this.cursor = cursor;
    }

    public void SetPosition(Vector3 position)
    {
        cursor.position = position;
    }

    public void SetActive(bool active)
    {
        cursor.gameObject.SetActive(active);
    }
}