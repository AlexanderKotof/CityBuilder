using CityBuilder.Dependencies;
using CityBuilder.Grid;
using PlayerInput;
using StateMachine;
using UnityEngine;

namespace InteractionStateMachine
{
    public abstract class InteractionState : StateBase, IUpdateState
    {
        private readonly PlayerInputManager _playerInput;
        private readonly CursorController _cursorController;
        private readonly Raycaster _raycastController;

        protected InteractionModel InteractionModel { get; }
        protected Raycaster Raycaster => _raycastController;

        protected InteractionState(DependencyContainer dependencyContainer)
        {
            _playerInput = dependencyContainer.Resolve<PlayerInputManager>();
            _cursorController = dependencyContainer.Resolve<CursorController>();
            _raycastController = dependencyContainer.Resolve<Raycaster>();

            InteractionModel = dependencyContainer.Resolve<InteractionModel>();
        }

        public void Update()
        {
            //ToDo Lighten cells under cursor position
            OnUpdate();
        }

        protected virtual void OnUpdate()
        {
            
        }

        protected override void OnEnterState()
        {
            _playerInput.OnMouseClick += OnMouseClick;
            _playerInput.OnMouseDragStarted += OnMouseDragStarted;
            _playerInput.OnMouseDraging += OnMouseDraging;
            _playerInput.OnMouseDragEnded += OnMouseDragEnded;
            _playerInput.OnMouseRightClick += OnMouseRightClick;
        }

        protected override void OnExitState()
        {
            _playerInput.OnMouseClick -= OnMouseClick;
            _playerInput.OnMouseDragStarted -= OnMouseDragStarted;
            _playerInput.OnMouseDraging -= OnMouseDraging;
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

        private void OnMouseDraging(Vector3 vector)
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
            InteractionModel.DraggedCell = cellModel;
            ChangeState<DraggingInteractionState>();
        }

        protected void SelectCell(CellModel cellModel)
        {
            InteractionModel.SelectedCell = cellModel;

            _cursorController.SetActive(true);
            _cursorController.SetPosition(GetCursorPositionFromCell(cellModel));
            
            OnCellSelected();
        }
        
        private Vector3 GetCursorPositionFromCell(CellModel cellModel)
        {
            Vector3 hitPosition2d = new Vector3(
                Mathf.FloorToInt(cellModel.Position.X),
                0,
                Mathf.FloorToInt(cellModel.Position.Y));

            return cellModel.GridModel.Transform.TransformPoint(hitPosition2d);
        }
        
        protected void DeselectCell()
        {
            if (InteractionModel.SelectedCell == null)
            {
                return;
            }
            
            InteractionModel.SelectedCell = null;
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