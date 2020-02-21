using GameCore.Base;
using Newtonsoft.Json.Linq;
using System;

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
            public int Year { get; set; } = 1;
            public int Month { get; set; } = 1;
            public int Day { get; set; } = 1;
            public int Hour { get; set; } = 0;
            public bool CarryDay = false;
            public bool CarryMonth = false;
            public bool CarryYear = false;
            public Time() { }
            public Time(int year = 1, int month = 1, int day = 1, int hour = 0)
            {
                Year = year;
                Month = month;
                Day = day;
                Hour = hour;
                Carry();
            }
            public Time(Time time)
            {
                if (time != null)
                {
                    Year = time.Year;
                    Month = time.Month;
                    Day = time.Day;
                    Hour = time.Hour;
                    Carry();
                }
            }
            public void Set(int year, int month, int day, int hour)
            {
                ResetCarryFlag();
                Year = year;
                Month = month;
                Day = day;
                Hour = hour;
                Carry();
            }
            public void Add(int year, int month, int day, int hour)
            {
                ResetCarryFlag();
                Year += year;
                Month += month;
                Day += day;
                Hour += hour;
                Carry();
            }
            public void Add(Time time)
            {
                ResetCarryFlag();
                Year += time.Year;
                Month += time.Month;
                Day += time.Day;
                Hour += time.Hour;
                Carry();
            }
            public void Sub(int year, int month, int day, int hour)
            {
                ResetCarryFlag();
                Year -= year;
                Month -= month;
                Day -= day;
                Hour -= hour;
                Carry();
            }
            public void Sub(Time time)
            {
                ResetCarryFlag();
                Year -= time.Year;
                Month -= time.Month;
                Day -= time.Day;
                Hour -= time.Hour;
                Carry();
            }
            public void Mul(float multiple = 1.0f)
            {
                ResetCarryFlag();
                Year = (int)(Year * multiple);
                Month = (int)(Month * multiple);
                Day = (int)(Day * multiple);
                Hour = (int)(Hour * multiple);
                Carry();
            }
            public bool IsEqual(Time time)
            {
                if (time == null)
                    return false;
                return Year == time.Year &&
                        Month == time.Month &&
                        Day == time.Day &&
                        Hour == time.Hour;
            }
            public bool Copy(Time time)
            {
                // not useful for JObject.ToObject<Time>()
                if (time == null)
                    return true;
                ResetCarryFlag();
                Year = time.Year;
                Month = time.Month;
                Day = time.Day;
                Hour = time.Hour;
                return false;
            }
            public void ResetCarryFlag()
            {
                CarryDay = false;
                CarryMonth = false;
                CarryYear = false;
            }
            public void Carry()
            {
                if (Hour > Core.Rules.TimeRule.HourMax)
                {
                    Day += Hour / (Core.Rules.TimeRule.HourMax + 1);
                    Hour = Hour % (Core.Rules.TimeRule.HourMax + 1);
                    CarryDay = true;
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
                    CarryMonth = true;
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
                    CarryYear = true;
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
                    json = new JObject();
                    json.Add("Year", Year);
                    json.Add("Month", Month);
                    json.Add("Day", Day);
                    json.Add("Hour", Hour);
                }
                catch (Exception e)
                {
                    return Core.State.WriteException<JObject>(e);
                }
                return json;
            }
            public bool FromJsonObject(JObject json)
            {
                try
                {
                    Year = (int)json["Year"];
                    Month = (int)json["Month"];
                    Day = (int)json["Day"];
                    Hour = (int)json["Hour"];
                }
                catch (Exception e)
                {
                    return Core.State.WriteException(e);
                }
                return false;
            }
        }
        public class CTime : Concept
        {
            public override string TypeName => "CTime";
            public Time Time { get; set; } = new Time();
            public override Concept FromJsonObject(JObject ojs)
            {
                var js = AlignJsonOjbect(ojs);
                if (js == null)
                    return null;
                try
                {
                    var c = Spawn<CTime>();
                    c.Time.FromJsonObject((JObject)js["Time"]);
                    return c;
                }
                catch (Exception e)
                {
                    return Core.State.WriteException<Concept>(e);
                }
            }
            public override JObject ToJsonObject()
            {
                JObject js = base.ToJsonObject();
                if (js == null)
                    return null;
                try
                {
                    if (Time == null)
                        this.Time = new Time();
                    JObject t = Time.ToJsonObject();
                    js.Add("Time", t);
                }
                catch (Exception e)
                {
                    return Core.State.WriteException<JObject>(e);
                }
                return js;
            }
        }
        public int _ctn_time = -1;
        // hook's parameter is card's number
        public Hook<int, object> HGoOneHour = new Hook<int, object>("HGoOneHour");
        public Hook<int, object> HGoOneDay = new Hook<int, object>("HGoOneDay");
        public Hook<int, object> HGoOneMonth = new Hook<int, object>("HGoOneMonth");
        public Hook<int, object> HGoOneYear = new Hook<int, object>("HGoOneYear");
        public Hook<int, object> HTimeChange = new Hook<int, object>("HTimeChange");
        public int HourMax { get; private set; } = 23; // hour will be 0 ~ 23
        public int DayMax { get; private set; } = 30; // day will be 1 ~ 30
        public int MonthMax { get; private set; } = 12; // month will be 1 ~ 12, year will be 1 ~ infinite
        public TimeRule()
        {
            _ctn_time = Concept.Spawn<CTime>().TypeNumber;
        }
        public override bool Init()
        {
            HourMax = 23;
            DayMax = 30;
            MonthMax = 12;
            return false;
        }
        public bool SetTime(Card card, int year = -1, int month = -1
                                , int day = -1, int hour = -1)
        {
            if (!HasConcept(card, _ctn_time))
                return true;
            var t = card.Get<CTime>().Time;
            if (t == null)
                t = new Time();
            if (year != -1)
                t.Year = year;
            if (month != -1)
                t.Month = month;
            if (day != -1)
                t.Day = day;
            if (hour != -1)
                t.Hour = hour;
            t.Carry();
            HTimeChange.CallAll(card.Number);
            return false;
        }
        public bool GoOneHour(Card card, bool is_call_hook = false)
        {
            if (!HasConcept(card, _ctn_time))
                return true;
            var t = card.Get<CTime>().Time;
            if(t == null)
                return true;
            t.Add(0, 0, 0, 1);
            HTimeChange.CallAll(card.Number);
            HGoOneHour.CallAll(card.Number);
            if (t.CarryDay)
                HGoOneDay.CallAll(card.Number);
            if (t.CarryMonth)
                HGoOneMonth.CallAll(card.Number);
            if (t.CarryYear)
                HGoOneYear.CallAll(card.Number);
            return false;
        }
        public override bool IsUsable()
        {
            return HourMax > 2
                    && DayMax > 2
                    && MonthMax > 2
                    && HGoOneHour != null
                    && HGoOneDay != null
                    && HGoOneMonth != null
                    && HGoOneYear != null;
        }
        public override bool FromJsonObject(JObject js)
        {
            if (base.FromJsonObject(js))
                return true;
            try
            {
                HourMax = (int)js["HourMax"];
                DayMax = (int)js["DayMax"];
                MonthMax = (int)js["MonthMax"];
                if (!IsUsable())
                    return true;
            }
            catch (Exception e)
            {
                return Core.State.WriteException(e);
            }
            return false;
        }
        public override JObject ToJsonObject()
        {
            var js = base.ToJsonObject();
            try
            {
                if (!IsUsable())
                {
                    HourMax = 23;
                    DayMax = 30;
                    MonthMax = 12;
                }
                js.Add("HourMax", HourMax);
                js.Add("DayMax", DayMax);
                js.Add("MonthMax", MonthMax);
            }
            catch (Exception e)
            {
                return Core.State.WriteException<JObject>(e);
            }
            return js;
        }
    }
}