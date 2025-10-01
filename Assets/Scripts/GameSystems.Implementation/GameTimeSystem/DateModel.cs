using System;

namespace GameTimeSystem
{
    public class DateModel
    {
        public int Year { get; private set; }
        public int Month { get; private set; }
        public int Day { get; private set; }
        public int Week { get; private set; } = 0;
        public int DayCounter { get; private set; } = 0;
        
        public float DayProgress { get; private set; }
        
        public event Action OnDayChanged;
        public event Action OnMonthChanged;
        public event Action OnYearChanged;
        public event Action OnWeekChanged;
        
        public DateModel(int year, int month, int day)
        {
            Year = year;
            Month = month;
            Day = day;
        }

        public float UpdateDayProgress(float progress) => DayProgress = progress;
        
        public void IncrementDay()
        {
            DayCounter++;
            Day++;

            if (Day > DaysInMonth())
            {
                Day = 1;
                IncrementMonth();
            }

            OnDayChanged?.Invoke();
            
            if (DayCounter % 7 == 1)
            {
                Week = (DayCounter - 1) / 7;
                OnWeekChanged?.Invoke();
            }
        }

        public override string ToString()
        {
            return $"Day {Day} Month {Month} Year {Year}";
        }

        private void IncrementMonth()
        {
            Month++;

            if (Month > 12)
            {
                Month = 1;
                IncrementYear();
            }
        }

        private void IncrementYear()
        {
            Year++;
        }

        private int DaysInMonth() => Month switch
        {
            1 => 31,
            2 => 28,
            3 => 31,
            4 => 30,
            5 => 31,
            6 => 30,
            7 => 31,
            8 => 31,
            9 => 30,
            10 => 31,
            11 => 30,
            12 => 31,
            _ => 0
        };
    }
}