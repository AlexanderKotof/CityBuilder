using System;
using UnityEngine;
using VContainer.Unity;

namespace GameSystems.Implementation.GameTimeSystem
{
    public class GameTimeSystem : ITickable
    {
        private int SecondsInDay = 5;

        public GameTimeSystem(DateModel date)
        {
            Date = date;
        }

        public int CurrentDay => Date.DayCounter;

        public DateModel Date { get; }
        
        public event Action<int> NewDayStarted;
        
        public void Tick()
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