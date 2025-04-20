using System;
using UnityEngine;

namespace GameTimeSystem
{
    public class GameTimeSystem : IGameSystem
    {
        [SerializeField]
        private int SecondsInDay = 5;

        public int CurrentDay { get; private set; } = 0;

        public event Action<int> NewDayStarted;
        
        public GameTimeSystem(){}

        public void Init()
        {
            
        }

        public void Deinit()
        {
            
        }

        public void Update()
        {
            if (Time.timeSinceLevelLoad < CurrentDay * SecondsInDay)
            {
                return;
            }

            CurrentDay++;
            NewDayStarted?.Invoke(CurrentDay);
        }
    }
}