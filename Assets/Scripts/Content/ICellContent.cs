using UnityEngine;

namespace CityBuilder.Content
{
    public interface ICellContent
    {
        bool CanBeMoved { get; }
    
        bool IsEmpty { get; }
    }
}