using System;
using UnityEngine;

namespace CityBuilder.GameSystems.Implementation.CellGridFeature.Grid
{
    public class GridComponent : MonoBehaviour, IGridComponent, IEquatable<GridComponent>
    {
        [SerializeField]
        private Vector2Int _gridSize;
        [SerializeField]
        private Renderer _renderer;
        [SerializeField]
        private BoxCollider _collider;
        
        public Transform Transform => transform;
    
        public Vector2Int Size => _gridSize;

        private void Awake()
        {
            SetGridSize(Size);
        }

        public void ShowGrid(bool show)
        {
            //TODO: show hide grid
        }

        private void SetGridSize(Vector2Int gridSize)
        {
            var material = _renderer.material;
            material.mainTextureScale = new Vector2(_gridSize.x, _gridSize.y);
            
            _collider.size = new Vector3(_gridSize.x, 0.2f, _gridSize.y);
        }

        public bool Equals(GridComponent other)
        {
            return Equals((IGridComponent)other);  
        }

        public bool Equals(IGridComponent other)
        {
            return other != null && this.GetInstanceID() == other.GetInstanceID();
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((IGridComponent)obj);
        }

        public override int GetHashCode()
        {
            return GetInstanceID();
        }

        public override string ToString()
        {
            return name;
        }
    }
}