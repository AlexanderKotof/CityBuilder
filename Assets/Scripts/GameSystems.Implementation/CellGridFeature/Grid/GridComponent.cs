using System;
using UnityEngine;

namespace CityBuilder.Grid
{
    public class GridComponent : MonoBehaviour, IGridComponent
    {
        public Vector2Int GridSize;
        public Transform Transform => transform;
    
        public Vector2Int Size => GridSize;
    
        private bool Equals(GridComponent other)
        {
            return this.GetInstanceID() == other.GetInstanceID();
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
            return Equals((GridComponent)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), this.GetInstanceID());
        }

        public override string ToString()
        {
            return name;
        }
    }
}