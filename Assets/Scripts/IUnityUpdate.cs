using System;

public interface IUnityUpdate
{
    void SubscribeOnUpdate(Action action);

    void UnsubscribeOnUpdate(Action action);
}