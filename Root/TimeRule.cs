using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore.Root
{
    public class TimeRule : Base.Rule
    {
        // A world need a Time Engine.
        // After create instance, you got a NowTime(not static)
        // Use GoOneDay() to drive the time
        // SetNowTime() is jumping into that time point, without triggering hooks.
        // A day has 24 hours, so HourMax is 23, use SetTimeMax() to change it.
        // A month has 30 days, from 1 to 30, so DayMax is 30, and MonthMax is 12
        public class Time
        {
            // Purely store time data 
            // year, month, day, hour
            public int Year { get; set; } = -1;
            public int Month { get; set; } = -1;
            public int Day { get; set; } = -1;
            public int Hour { get; set; } = -1;
            public Time() { }
            public Time(int year, int month, int day, int hour)
            {
                Year = year;
                Month = month;
                Day = day;
                Hour = hour;
                Carry();
            }
            public Time(Time time)
            {
                Year = time.Year;
                Month = time.Month;
                Day = time.Day;
                Hour = time.Hour;
                Carry();
            }
            public void Add(int year, int month, int day, int hour)
            {
                Year += year;
                Month += month;
                Day += day;
                Hour += hour;
                Carry();
            }
            public void Add(Time time)
            {
                Year += time.Year;
                Month += time.Month;
                Day += time.Day;
                Hour += time.Hour;
                Carry();
            }
            public void Sub(int year, int month, int day, int hour)
            {
                Year -= year;
                Month -= month;
                Day -= day;
                Hour -= hour;
                Carry();
            }
            public void Sub(Time time)
            {
                Year -= time.Year;
                Month -= time.Month;
                Day -= time.Day;
                Hour -= time.Hour;
                Carry();
            }
            public static bool operator ==(Time a, Time b)
            {
                return  a.Year == b.Year &&
                        a.Month == b.Month &&
                        a.Day == b.Day &&
                        a.Hour == b.Hour;
            }
            public static bool operator !=(Time a, Time b)
            {
                return a.Year != b.Year &&
                        a.Month != b.Month &&
                        a.Day != b.Day &&
                        a.Hour != b.Hour;
            }
            public void Carry()
            {
                if (Hour > Core.Instance.Rules.TimeRule.HourMax)
                {
                    Day += Hour / (Core.Instance.Rules.TimeRule.HourMax + 1);
                    Hour = Hour % (Core.Instance.Rules.TimeRule.HourMax + 1);
                }
                if (Hour < 0) 
                {
                    Day -= ((-Hour) / (Core.Instance.Rules.TimeRule.HourMax + 1)) + 1;
                    Hour = (-Hour) % (Core.Instance.Rules.TimeRule.HourMax + 1);
                    if (Hour == 0)
                    {
                        Day++;
                    }
                    else
                    {
                        Hour = (Core.Instance.Rules.TimeRule.HourMax + 1) - Hour;
                    }
                }
                if (Day > Core.Instance.Rules.TimeRule.DayMax)
                {
                    Month += Day / Core.Instance.Rules.TimeRule.DayMax;
                    Day = Day % Core.Instance.Rules.TimeRule.DayMax;
                    if (Day == 0)
                    {
                        Day = Core.Instance.Rules.TimeRule.DayMax;
                        Month--;
                    }
                }
                if (Day < 1) 
                {
                    Month -= (-Day) / Core.Instance.Rules.TimeRule.DayMax + 1;
                    Day = (-Day) % Core.Instance.Rules.TimeRule.DayMax;
                    Day = Core.Instance.Rules.TimeRule.DayMax - Day;
                }
                if (Month > Core.Instance.Rules.TimeRule.MonthMax)
                {
                    Year += Month / Core.Instance.Rules.TimeRule.MonthMax;
                    Month = (Month % Core.Instance.Rules.TimeRule.MonthMax) + 1;
                    if (Month == 0)
                    {
                        Month = Core.Instance.Rules.TimeRule.MonthMax;
                        Year--;
                    }
                }
                if (Month < 1)
                {
                    Year -= (-Month) / Core.Instance.Rules.TimeRule.MonthMax + 1;
                    Month = (-Month) % Core.Instance.Rules.TimeRule.MonthMax;
                    Month = Core.Instance.Rules.TimeRule.MonthMax - Month;
                }
                if (Year < 1)
                {
                    Year = 1;
                }
            }
        }
        public Time NowTime { get; private set; }
        private readonly Base.Hook HHourChange = new Base.Hook("TimeRule.HourChange");
        private readonly Base.Hook HDayChange = new Base.Hook("TimeRule.DayChange");
        private readonly Base.Hook HMonthChange = new Base.Hook("TimeRule.MonthChange");
        private readonly Base.Hook HYearChange = new Base.Hook("TimeRule.YearChange");
        public TimeRule() {}
        public void SetNowTime(int year = -1, int month = -1 , int day = -1, int hour = -1)
        {
            // If not assign the value, or value < 0, the entry will not change.
            // Do not trigger the hook.
            // day and month need be > 0
            if (hour >= 0)
                NowTime.Hour = hour;
            if (day > 0)
                NowTime.Day = day;
            if (month > 0)
                NowTime.Month = month;
            if (year > 0)
                NowTime.Year = year;
            NowTime.Carry();
        }
        public void SetNowTime(Time time)
        {
            if (time.Hour >= 0)
                NowTime.Hour = time.Hour;
            if (time.Day > 0)
                NowTime.Day = time.Day;
            if (time.Month > 0)
                NowTime.Month = time.Month;
            if (time.Year >= 0)
                NowTime.Year = time.Year;
            NowTime.Carry();
        }
        public void SetTimeMax(int month_max = -1, int day_max = -1, int hour_max = -1)
        {
            // If not assign the values, do no change it.
            // Year has no max
            if (month_max >= 1)
                MonthMax = month_max;
            if (day_max >= 1)
                DayMax = day_max;
            if (hour_max >= 1)
                HourMax = hour_max;
        }
        public Base.IHookUser IHHourChange => HHourChange.GetHookUser();
        public Base.IHookUser IHDayChange => HDayChange.GetHookUser();
        public Base.IHookUser IHMonthChange => HMonthChange.GetHookUser();
        public Base.IHookUser IHYearChange => HYearChange.GetHookUser();
        public int HourMax { get; private set; } = 23; // hour will be 0 ~ 23
        public int DayMax { get; private set; } = 30; // day will be 1 ~ 30
        public int MonthMax { get; private set; } = 12; // month will be 1 ~ 12, year will be 1 ~ infinite
        private bool[] _time_changed = new bool[4] {false, false, false, false}; //hour, day, month, year, used by _CallTimeChange()
        private void _CallTimeChange()
        {
            if (_time_changed[0])
                HHourChange.CallAll();
            if (_time_changed[1])
                HDayChange.CallAll();
            if (_time_changed[2])
                HMonthChange.CallAll();
            if (_time_changed[3])
                HYearChange.CallAll();
            _time_changed[3] = _time_changed[2] = _time_changed[1] = _time_changed[0] = false;
        }
        public void GoOneHour()
        {
            NowTime.Hour++;
            _time_changed[0] = true;
            if (NowTime.Hour >= HourMax)
            {
                NowTime.Hour = 0;
                GoOneDay();
            }
            else
            {
                _CallTimeChange();
            }
        }
        public void GoOneDay()
        {
            NowTime.Day++;
            _time_changed[1] = true;
            if (NowTime.Day >= DayMax)
            {
                NowTime.Day = 1;
                GoOneMonth();
            }
            else
            {
                _CallTimeChange();
            }
        }
        public void GoOneMonth()
        {
            NowTime.Month++;
            _time_changed[2] = true;
            if (NowTime.Month >= MonthMax)
            {
                NowTime.Month = 1;
                GoOneYear();
            }
            else
            {
                _CallTimeChange();
            }
        }
        public void GoOneYear()
        {
            NowTime.Year++;
            _time_changed[3] = true;
            _CallTimeChange();
        }
        public bool Init()
        {
            NowTime = new Time();
            NowTime.Hour = 0;
            NowTime.Day = 1;
            NowTime.Month = 1;
            NowTime.Year = 1;
            return false;
        }
        public override bool IsUsable()
        {
            if(NowTime != null)
            {
                if (NowTime.Hour >= 0
                    && NowTime.Day > 0
                    && NowTime.Month > 0
                    && NowTime.Year > 0)
                    return true;
            }
            return false;
        }
    }
}
