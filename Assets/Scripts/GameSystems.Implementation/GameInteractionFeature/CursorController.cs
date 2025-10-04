using UnityEngine;

namespace GameSystems.Implementation.GameInteractionFeature
{
    public class CursorController
    {
        private readonly Transform cursor;

        public CursorController(Transform cursor)
        {
            this.cursor = cursor;
        }

        public void SetPosition(Vector3 position, Vector2Int selectionSize)
        {
            cursor.position = position;
            cursor.localScale = new Vector3(selectionSize.x, 1, selectionSize.y);
        }

        public void SetActive(bool active)
        {
            cursor.gameObject.SetActive(active);
        }
    }
}