using System;
using SityBuilder.Reactive;
using UnityEngine;
using ViewSystem;

namespace BuildingSystem
{
    public class EmptyContent : ICellContent
    {
        private static ICellContent Empty { get; } = new EmptyContent();
        public GameObject View => default;
        public bool CanBeMoved => false;
        public bool IsEmpty => true;
    }
    
    public class Building : ICellContent, IViewModel
    {
        public ReactiveProperty<int> Level { get; } = new();
        public ReactiveProperty<int> Rotation { get; } = new();
        
        public BuildingConfig Config { get; }
        public GameObject View { get; private set; }
        // 0-4 
        public readonly Guid Id = Guid.NewGuid();
        
        public bool CanBeMoved => true;
        public bool IsEmpty => false;
        
        public Building(int level, BuildingConfig config)
        {
            Level.Set(level);
            Config = config;
        }

        public void IncreaseLevel()
        {
            Level.Set(Level.Value + 1);
        }

        public void SetView(GameObject view)
        {
            View = view;
        }
    }
}
