using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ViewSystem;

public class Test : MonoBehaviour
{
    public Component Component;

    [SerializeField] private ComponentCollection ComponentsCollection;

    public Color color1;
    public Color color2;

    private void Awake()
    {
        //var newObj = Instantiate(Component, transform);
        
        //Debug.Log("[Test] Created object : " + newObj.name);

        foreach (var component in ComponentsCollection)
        {
            Debug.Log("[Test] Component found " + component.GetType().Name);
        }
    }
}

[Serializable]
public class ComponentCollection : IEnumerable<Component>, ICollection<Component>
{
    [SerializeField]
    private List<Component> _components = new List<Component>();

    public int Count => _components.Count;
    public bool IsReadOnly => false;
    
    public List<Component> Components => _components;
    
    public IEnumerator<Component> GetEnumerator()
    {
        return _components.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(Component item)
    {
        _components.Add(item);
    }

    public void Clear()
    {
        _components.Clear();
    }

    public bool Contains(Component item)
    {
        return _components.Contains(item);
    }

    public void CopyTo(Component[] array, int arrayIndex)
    {
        _components.CopyTo(array, arrayIndex);
    }

    public bool Remove(Component item)
    {
        return _components.Remove(item);
    }

}


