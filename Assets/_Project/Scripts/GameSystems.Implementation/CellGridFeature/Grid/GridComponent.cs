using UnityEngine;

namespace CityBuilder.GameSystems.Implementation.CellGridFeature.Grid
{
    public class GridComponent : MonoBehaviour, IGridComponent
    {
        [SerializeField]
        private Vector2Int _gridSize;
        public Transform Transform => transform;
    
        public Vector2Int Size => _gridSize;
    
        private bool Equals(GridComponent other)
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