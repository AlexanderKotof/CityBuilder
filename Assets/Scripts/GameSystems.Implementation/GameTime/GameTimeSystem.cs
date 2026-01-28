using System;
using UnityEngine;
using VContainer.Unity;

namespace GameSystems.Implementation.GameTime
{
    public class GameTimeSystem : ITickable
    {
        private int SecondsInDay = 5;
        
        private float TimeFromStart => Time.timeSinceLevelLoad;

        public int CurrentDay => Date.DayCounter;

        public DateModel Date { get; }
        
        public event Action<int> NewDayStarted;
        
        public GameTimeSystem(DateModel date)
        {
            Date = date;
        }
        
        public void Tick()
        {
            float nextDayAt = (CurrentDay + 1) * SecondsInDay;
            Date.UpdateDayProgress((TimeFromStart - nextDayAt) / SecondsInDay);
            
            if (TimeFromStart < nextDayAt)
            {
                return;
            }

            Date.IncrementDay();
            
            NewDayStarted?.Invoke(CurrentDay);
        }
    }
}