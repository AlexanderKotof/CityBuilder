using System;
using UniRx;

namespace CityBuilder.GameSystems.Implementation.GameTime
{
    public class DateModel
    {
        public int Year { get; private set; }
        public int Month { get; private set; }
        public int Day { get; private set; }
        public int Week { get; private set; } = 0;
        public int DayCounter { get; private set; } = 0;

        public ReactiveProperty<float> DayProgress { get; } = new();

        public IObservable<Unit> DayChanged => _dayChanged;
        private readonly Subject<Unit> _dayChanged = new();
        public event Action OnDayChanged;
        public event Action OnMonthChanged;
        public event Action OnYearChanged;
        public event Action OnWeekChanged;

        public DateModel() : this(100, 1, 1){}
        private DateModel(int year, int month, int day)
        {
            Year = year;
            Month = month;
            Day = day;
        }

        public float UpdateDayProgress(float progress) => DayProgress.Value = progress;
        
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
            _dayChanged.OnNext(Unit.Default);
            
            if (DayCounter % 7 == 0)
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

        public string MonthName()
        {
            return Month switch
            {
                1 => "January",
                2 => "February",
                3 => "March",
                4 => "April",
                5 => "May",
                6 => "June",
                7 => "July",
                8 => "August",
                9 => "September",
                10 => "October",
                11 => "November",
                12 => "December",
                _ => string.Empty,
            };
        }
    }
}