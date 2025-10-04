using UnityEngine;

namespace Configs.Schemes
{
    public record Size(int X, int Y)
    {
        public int X { get; set; } = X;
        public int Y { get; set; } = Y;
        

    }

    public static class SizeExtensions
    {
        public static Vector2Int ToVector(this Size reference) => new Vector2Int(reference.X, reference.Y);
    }
}