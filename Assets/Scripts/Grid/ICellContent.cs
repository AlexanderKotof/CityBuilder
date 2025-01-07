using UnityEngine;

public interface ICellContent
{
    GameObject View { get; }
    
    bool CanBeMoved { get; }
    
    bool IsEmpty { get; }
}
