using System;
using System.Threading.Tasks;
using GameSystems;
using UnityEngine;

namespace GameTimeSystem
{
    public class GameTimeSystem : IGameSystem, IUpdateGamSystem
    {
        [SerializeField]
        private int SecondsInDay = 5;

        public int CurrentDay => Date.DayCounter;

        public DateModel Date { get; } = new DateModel(1000, 1, 1);
        
        public event Action<int> NewDayStarted;

        public Task Init()
        {
            return Task.CompletedTask;
        }

        public Task Deinit()
        {
            return Task.CompletedTask;
        }

        public void Update()
        {
            float nextDayAt = (CurrentDay + 1) * SecondsInDay;
            Date.UpdateDayProgress((Time.timeSinceLevelLoad - nextDayAt) / SecondsInDay);
            
            if (Time.timeSinceLevelLoad < nextDayAt)
            {
                return;
            }

            Date.IncrementDay();
            
            NewDayStarted?.Invoke(CurrentDay);
        }
    }
}