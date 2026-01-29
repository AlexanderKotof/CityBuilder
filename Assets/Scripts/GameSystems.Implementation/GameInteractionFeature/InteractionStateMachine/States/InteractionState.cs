using CityBuilder.BuildingSystem;
using CityBuilder.Dependencies;
using CityBuilder.Grid;
using PlayerInput;
using StateMachine;
using UnityEngine;
using VContainer;

namespace GameSystems.Implementation.GameInteractionFeature.InteractionStateMachine.States
{
    public abstract class InteractionState : StateBase, IUpdateState
    {
        [Inject]
        private readonly PlayerInputManager _playerInput;
        [Inject]
        private readonly CursorController _cursorController;
        [Inject]
        private readonly Raycaster _raycastController;
        [Inject]
        private readonly BuildingManager _buildingManager;
        [Inject]
        protected InteractionModel InteractionModel { get; private set; }
        protected Raycaster Raycaster => _raycastController;
        protected BuildingManager BuildingManager => _buildingManager;
        
        public virtual void Update()
        {
            
        }

        //TODO: pass content size
        protected void LightenCellUnderCursor(Vector2Int cursorSize)
        {
            bool showCursor = Raycaster.TryGetCursorPositionFromScreenPoint(Input.mousePosition, out Vector3? cursorPosition);
            _cursorController.SetActive(showCursor);
            if (showCursor)
            {
                _cursorController.SetPosition(cursorPosition.Value, cursorSize);
            }
        }
        
        protected void LightenCell(CellModel cellModel)
        {
            if (cellModel != null)
            {
                _cursorController.SetActive(true);
                _cursorController.SetPosition(cellModel.WorldPosition, Vector2Int.one);
            }
            else
            {
                _cursorController.SetActive(false);
            }
        }

        protected override void OnEnterState()
        {
            _playerInput.OnMouseClick += OnMouseClick;
            _playerInput.OnMouseDragStarted += OnMouseDragStarted;
            _playerInput.OnMouseDragging += OnMouseDragging;
            _playerInput.OnMouseDragEnded += OnMouseDragEnded;
            _playerInput.OnMouseRightClick += OnMouseRightClick;
        }

        protected override void OnExitState()
        {
            _playerInput.OnMouseClick -= OnMouseClick;
            _playerInput.OnMouseDragStarted -= OnMouseDragStarted;
            _playerInput.OnMouseDragging -= OnMouseDragging;
            _playerInput.OnMouseDragEnded -= OnMouseDragEnded;
            _playerInput.OnMouseRightClick -= OnMouseRightClick;
        }

        private void OnMouseClick(Vector3 vector)
        {
            if (!_raycastController.TryGetCellFromScreenPoint(vector, out CellModel? cell))
            {
                return;
            }

            ProcessClick(cell);
        }
        
        private void OnMouseRightClick(Vector3 position)
        {
            if (!_raycastController.TryGetCellFromScreenPoint(position, out CellModel? cell))
            {
                TryCancel();
                return;
            }

            ProcessRightClick(cell);
        }
        
        private void OnMouseDragStarted(Vector3 vector)
        {
            if (!_raycastController.TryGetCellFromScreenPoint(vector, out CellModel? cell))
            {
                return;
            }

            if (CanBeDragged(cell))
            {
                ProcessDragStarted(cell);
            }
        }

        private void OnMouseDragEnded(Vector3 vector)
        {
            if (!_raycastController.TryGetCellFromScreenPoint(vector, out CellModel? cell))
            {
                return;
            }

            ProcessDragEnded(cell);
        }

        private void OnMouseDragging(Vector3 vector)
        {
            ProcessDragging(vector);
        }
        
        protected virtual void ProcessClick(CellModel cellModel)
        {
            Debug.Log($"Process click on {cellModel.Position}");
        }
        protected virtual void ProcessRightClick(CellModel cellModel)
        {
            Debug.Log($"Process right click on {cellModel.Position}");
        }
        protected virtual void ProcessDragStarted(CellModel cellModel)
        {
            Debug.Log($"Process drag started on {cellModel.Position}");
        }
        protected virtual void ProcessDragEnded(CellModel cellModel)
        {
            Debug.Log($"Process drag ended on {cellModel.Position}");
        }
        protected virtual void ProcessDragging(Vector3 mousePosition) { }

        protected void TrySelectCell(CellModel cellModel)
        {
            if (CellCanBeSelected(cellModel))
            {
                SelectCell(cellModel);
            }
        }

        protected void StartDragCell(CellModel cellModel)
        {
            InteractionModel.DraggedCell.Set(cellModel);
            ChangeState<DraggingInteractionState>();
        }

        protected void SelectCell(CellModel cellModel)
        {
            InteractionModel.SelectedCell.Set(cellModel);
            
            OnCellSelected();
        }
        
        protected void DeselectCell()
        {
            if (InteractionModel.SelectedCell == null)
            {
                return;
            }
            
            InteractionModel.SelectedCell.Set(null);
            _cursorController.SetActive(false);
            OnCellSelected();
        }

        protected virtual void OnCellSelected() { }

        protected virtual bool CellCanBeSelected(CellModel cellModel)
        {
            return true;
        }
        
        protected virtual bool CanBeDragged(CellModel cellModel)
        {
            return cellModel.Content.HasValue() && cellModel.Content.Value.CanBeMoved;
        }

        protected virtual void TryCancel()
        {
            
        }
    }
}