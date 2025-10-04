using System;

public interface IUnityUpdate
{
    void SubscribeOnUpdate(Action action);

    void UnsubscribeOnUpdate(Action action);
}

public interface IUnityFixedUpdate
{
    void SubscribeOnFixedUpdate(Action action);

    void UnsubscribeOnFixedUpdate(Action action);
}