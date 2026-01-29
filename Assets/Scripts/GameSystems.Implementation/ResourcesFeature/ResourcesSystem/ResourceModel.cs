using System;
using CityBuilder.Reactive;
using Configs.Implementation.Common;

namespace ResourcesSystem
{
    public class ResourceModel : IEquatable<ResourceModel>
    {
        private readonly ResourceConfig Config;
        public ReactiveProperty<int> Amount { get; } = new();
        
        public ResourceType Id => Config.Type;

        public ResourceModel(ResourceConfig config)
        {
            Config = config;
            Amount.Set(config.Amount);
        }

        public void AddAmount(int amount)
        {
            Amount.Value += amount;
        }

        public bool RemoveAmount(int amount)
        {
            if (amount <= Amount.Value)
            {
                Amount.Value -= amount;
                return true;
            }
            
            return false;
        }

        public bool HasAmount(int amount)
        {
            return Amount.Value >= amount;
        }

        public bool Equals(ResourceModel other)
        {
            return other is not null && Config.Type.Equals(other.Config.Type);
        }
    }
}