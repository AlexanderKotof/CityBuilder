using System;
using UniRx;

namespace Utilities.Extensions
{
    public static class ReactiveCollectionExtensions
    {
        public static IDisposable SubscribeToCollection<T>(this ReactiveCollection<T> collection, Action<T> onAdd,
            Action<T> onRemove, bool handleExist = true)
        {
            var subscriptions = new CompositeDisposable();
            collection.ObserveAdd().Subscribe(e => onAdd(e.Value)).AddTo(subscriptions);
            collection.ObserveRemove().Subscribe(e => onRemove(e.Value)).AddTo(subscriptions);

            if (handleExist)
            {
                foreach (var data in collection)
                {
                    onAdd(data);
                }
            }
            return subscriptions;
        }
    }

    public static class ReactivePropertyExtensions
    {
        public static void Set<T>(this ReactiveProperty<T> property, T value)
        {
            property.Value = value;
        }
    }
}