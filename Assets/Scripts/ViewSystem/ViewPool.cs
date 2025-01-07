using UnityEngine;

namespace ViewSystem
{
    public class ViewPool : ObjectsPool<ViewBase>
    {
        public ViewPool(ViewBase prefabReference) : base(prefabReference)
        {
        }

        protected override void OnPool(ViewBase gameobject)
        {
            gameobject.gameObject.SetActive(true);
        }

        protected override void OnReturn(ViewBase gameobject)
        {
            gameobject.gameObject.SetActive(false);
        }
    }

    public class GameObjectPool : ObjectsPool<GameObject>
    {
        public GameObjectPool(GameObject prefabReference) : base(prefabReference)
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
}