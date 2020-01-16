using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using GameCore.Base;

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
            // add and sub will do carry(), but copy won't
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
            public bool Copy(Time time)
            {
                if (time == null)
                    return true;
                Year = time.Year;
                Month = time.Month;
                Day = time.Day;
                Hour = time.Hour;
                return false;
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
                if (Hour > Core.Rules.TimeRule.HourMax)
                {
                    Day += Hour / (Core.Rules.TimeRule.HourMax + 1);
                    Hour = Hour % (Core.Rules.TimeRule.HourMax + 1);
                }
                if (Hour < 0) 
                {
                    Day -= ((-Hour) / (Core.Rules.TimeRule.HourMax + 1)) + 1;
                    Hour = (-Hour) % (Core.Rules.TimeRule.HourMax + 1);
                    if (Hour == 0)
                    {
                        Day++;
                    }
                    else
                    {
                        Hour = (Core.Rules.TimeRule.HourMax + 1) - Hour;
                    }
                }
                if (Day > Core.Rules.TimeRule.DayMax)
                {
                    Month += Day / Core.Rules.TimeRule.DayMax;
                    Day = Day % Core.Rules.TimeRule.DayMax;
                    if (Day == 0)
                    {
                        Day = Core.Rules.TimeRule.DayMax;
                        Month--;
                    }
                }
                if (Day < 1) 
                {
                    Month -= (-Day) / Core.Rules.TimeRule.DayMax + 1;
                    Day = (-Day) % Core.Rules.TimeRule.DayMax;
                    Day = Core.Rules.TimeRule.DayMax - Day;
                }
                if (Month > Core.Rules.TimeRule.MonthMax)
                {
                    Year += Month / Core.Rules.TimeRule.MonthMax;
                    Month = (Month % Core.Rules.TimeRule.MonthMax) + 1;
                    if (Month == 0)
                    {
                        Month = Core.Rules.TimeRule.MonthMax;
                        Year--;
                    }
                }
                if (Month < 1)
                {
                    Year -= (-Month) / Core.Rules.TimeRule.MonthMax + 1;
                    Month = (-Month) % Core.Rules.TimeRule.MonthMax;
                    Month = Core.Rules.TimeRule.MonthMax - Month;
                }
                if (Year < 1)
                {
                    Year = 1;
                }
            }
            public JObject ToJsonObject()
            {
                JObject json = null;
                try
                {
                    json = JObject.FromObject(this);
                }
                catch (Exception)
                {
                    return null;
                }
                return json;
            }
            public bool FromJsonObject(JObject json)
            {
                try
                {
                    Time t = json.ToObject<Time>();
                    if(Copy(t))
                        return true;
                }catch(Exception)
                {
                    return true;
                }
                return false;
            }
            public override bool Equals(object obj)
            {
                return obj is Time time &&
                       Year == time.Year &&
                       Month == time.Month &&
                       Day == time.Day &&
                       Hour == time.Hour;
            }
            public override int GetHashCode()
            {
                var hashCode = -1541897947;
                hashCode = hashCode * -1521134295 + Year.GetHashCode();
                hashCode = hashCode * -1521134295 + Month.GetHashCode();
                hashCode = hashCode * -1521134295 + Day.GetHashCode();
                hashCode = hashCode * -1521134295 + Hour.GetHashCode();
                return hashCode;
            }
        }
        public class CTime : Concept
        {
            public override string TypeName => "CTime";
            public Time Time { get; set; }
            public override bool FromJsonObject(JObject js)
            {
                if(base.FromJsonObject(js))
                    return true;
                try{
                    Time = new Time();
                    if (Time.FromJsonObject((JObject)js["Time"]))
                        return true;
                }catch(Exception){
                    return true;
                }
                return false;
            }
            public override JObject ToJsonObject()
            {
                JObject js = base.ToJsonObject();
                if(js == null)
                    return null;
                try{
                    if (Time == null)
                        return null;
                    JObject t = Time.ToJsonObject();
                    if (t == null)
                        return null;
                    js.Add("Time", t);
                }catch(Exception){
                    return null;
                }
                return js;
            }
        }
        private readonly Hook<object, object> HHourChange = new Hook<object, object>("TimeRule.HHourChangee");
        private readonly Hook<object, object> HDayChange = new Hook<object, object>("TimeRule.DayChange");
        private readonly Hook<object, object> HMonthChange = new Hook<object, object>("TimeRule.MonthChange");
        private readonly Hook<object, object> HYearChange = new Hook<object, object>("TimeRule.YearChange");
        public int _ctn_time = -1;
        public Time NowTime { get; set; }
        public TimeRule() 
        {
            _ctn_time = ConceptSpawner<CTime>.GetSpawner().Type_Number;
        }
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
        public int HourMax { get; private set; } = 23; // hour will be 0 ~ 23
        public int DayMax { get; private set; } = 30; // day will be 1 ~ 30
        public int MonthMax { get; private set; } = 12; // month will be 1 ~ 12, year will be 1 ~ infinite
        private bool[] _time_changed = new bool[4] {false, false, false, false}; //hour, day, month, year, used by _CallTimeChange()
        private void _CallTimeChange()
        {
            if (_time_changed[0])
                HHourChange.CallAll(null);
            if (_time_changed[1])
                HDayChange.CallAll(null);
            if (_time_changed[2])
                HMonthChange.CallAll(null);
            if (_time_changed[3])
                HYearChange.CallAll(null);
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
        public override bool Init()
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
        public override bool FromJsonObject(JObject js)
        {
            if(base.FromJsonObject(js))
                return true;
            try{
                SetNowTime((int)js["NowTime"][0],
                    (int)js["NowTime"][1],
                    (int)js["NowTime"][2],
                    (int)js["NowTime"][3]);
            }catch(Exception){
                return true;
            }
            return false;
        }
        public override JObject ToJsonObject()
        {
            var js = base.ToJsonObject();
            try{
                JArray t = new JArray();
                t.Add(new JValue(NowTime.Year));
                t.Add(new JValue(NowTime.Month));
                t.Add(new JValue(NowTime.Day));
                t.Add(new JValue(NowTime.Hour));
                js.Add("NowTime", t);
            }catch(Exception){
                return null;
            }
            return js;
        }
    }
}
