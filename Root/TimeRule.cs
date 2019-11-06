using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore.Root
{
    class TimeRule
    {
        // A world need a Time Engine.
        // After create entity, you got a NowTime(not static)
        // Use GoOneDay() to drive the time
        // SetNowTime() is jumping into that time point, without triggering hooks.
        // A day has 24 hours, so HourMax is 24, use SetTimeMax() to change it.
        public class CTime : Base.Component
        {
            // Purely store time data 
            // year, month, day, hour
            private const string type_name = "CTime";
            public override string TypeName => type_name;
            public int year { get; set; } = -1;
            public int month { get; set; } = -1;
            public int day { get; set; } = -1;
            public int hour { get; set; } = -1;
        }
        public CTime NowTime { get; }
        private readonly Base.Hook HHourChange = new Base.Hook("TimeRule.HourChange");
        private readonly Base.Hook HDayChange = new Base.Hook("TimeRule.DayChange");
        private readonly Base.Hook HMonthChange = new Base.Hook("TimeRule.MonthChange");
        private readonly Base.Hook HYearChange = new Base.Hook("TimeRule.YearChange");
        public TimeRule() {
            NowTime = Base.Component.GetSpawner<CTime>().Spawn();
            SetNowTime(0, 1, 1, 0);
        }
        public void SetNowTime(int year = -1, int month = -1 , int day = -1, int hour = -1)
        {
            // If not assign the value, or value < 0, the entry will not change.
            // Do not trigger the hook.
            // day and month need be > 0
            if (hour >= 0)
                NowTime.hour = hour;
            if (day > 0)
                NowTime.day = day;
            if (month > 0)
                NowTime.month = month;
            if (year >= 0)
                NowTime.year = year;
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
        public int HourMax { get; private set; } = 24; // Initial be 24
        public int DayMax { get; private set; } = 30; // Initial be 30
        public int MonthMax { get; private set; } = 12; // Initial be 12
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
            NowTime.hour++;
            _time_changed[0] = true;
            if (NowTime.hour >= HourMax)
            {
                NowTime.hour = 0;
                GoOneDay();
            }
            else
            {
                _CallTimeChange();
            }
        }
        public void GoOneDay()
        {
            NowTime.day++;
            _time_changed[1] = true;
            if (NowTime.day >= DayMax)
            {
                NowTime.day = 1;
                GoOneMonth();
            }
            else
            {
                _CallTimeChange();
            }
        }
        public void GoOneMonth()
        {
            NowTime.month++;
            _time_changed[2] = true;
            if (NowTime.month >= MonthMax)
            {
                NowTime.month = 1;
                GoOneYear();
            }
            else
            {
                _CallTimeChange();
            }
        }
        public void GoOneYear()
        {
            NowTime.year++;
            _time_changed[3] = true;
            _CallTimeChange();
        }
    }
}
