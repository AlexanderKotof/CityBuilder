using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ViewSystem
{
    // public class ViewPool : ObjectsPool<ViewBase>
    // {
    //     public ViewPool(GameObject prefabReference) : base(prefabReference)
    //     {
    //     }
    //
    //     protected override void OnPool(ViewBase gameobject)
    //     {
    //         gameobject.gameObject.SetActive(true);
    //     }
    //
    //     protected override void OnReturn(ViewBase gameobject)
    //     {
    //         gameobject.gameObject.SetActive(false);
    //     }
    // }

    public class GameObjectPool : NewObjectsPool<GameObject>
    {
        public GameObjectPool(string assetKey) : base(assetKey)
        {
        }

        protected override void OnPool(GameObject gameobject)
        {
            gameobject.SetActive(true);
        }

        protected override void OnReturn(GameObject gameobject)
        {
            gameobject.SetActive(false);
        }
    }

    public class ComponentObjectPool : NewObjectsPool<Component>
    {
        public ComponentObjectPool(string assetKey) : base(assetKey)
        {
        }

        protected override void OnPool(Component gameobject)
        {

        }

        protected override void OnReturn(Component gameobject)
        {

        }
    }
}