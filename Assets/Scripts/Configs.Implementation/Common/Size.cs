using System;
using UnityEngine;

namespace Configs.Schemes
{
    [Serializable]
    public record Size(int X, int Y)
    {
        [field: SerializeField]
        public int X { get; set; } = X;
        [field: SerializeField]
        public int Y { get; set; } = Y;
    }

    public static class SizeExtensions
    {
        public static Vector2Int ToVector(this Size reference) => new Vector2Int(reference.X, reference.Y);
    }
}