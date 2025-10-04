using System;
using UnityEngine;

public class UnityUpdate : MonoBehaviour, IUnityUpdate, IUnityFixedUpdate
{
    private Action? UpdateHandler;
    private Action? FixedUpdateHandler;
    
    private void Update() => UpdateHandler?.Invoke();

    private void FixedUpdate() => FixedUpdateHandler?.Invoke();

    private void OnDestroy()
    {
        UpdateHandler = null;
        FixedUpdateHandler = null;
    }

    public void SubscribeOnUpdate(Action action)
    {
        UpdateHandler += action;
    }

    public void UnsubscribeOnUpdate(Action action)
    {
        UpdateHandler -= action;
    }

    public void SubscribeOnFixedUpdate(Action action)
    {
        FixedUpdateHandler += action;
    }

    public void UnsubscribeOnFixedUpdate(Action action)
    {
        FixedUpdateHandler -= action;
    }
}