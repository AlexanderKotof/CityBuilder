using CityBuilder.Grid;
using GameSystems.Implementation.GameInteractionFeature.InteractionStateMachine.States;
using UnityEngine;

namespace GameSystems.Implementation.GameInteractionFeature
{
    public class CursorComponent : MonoBehaviour
    {
        public Color AcceptedStateColor = Color.white;
        public Color RejectedStateColor = Color.red;
        public Color MergeStateColor = Color.red;
        
        public Renderer Renderer;
        
        public void Setup(CellModel cellModel, CursorStateEnum cursorState)
        {
            SetCursor(cellModel.WorldPosition, Vector3.one, cursorState);
        }

        public void SetCursor(Vector3 position, Vector3 scale, CursorStateEnum state = CursorStateEnum.Accepted)
        {
            transform.position = position;
            transform.localScale = scale;
            
            Renderer.material.color = GetColor(state);
        }

        private Color GetColor(CursorStateEnum state)
        {
            return state switch
            {
                CursorStateEnum.Accepted => AcceptedStateColor,
                CursorStateEnum.Merge => MergeStateColor,
                CursorStateEnum.Rejected => RejectedStateColor,
                _ => AcceptedStateColor,
            };
        }
    }
}