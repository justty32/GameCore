using System.Data;
using System;
using System.Collections.Generic;
using System.Text;

// using a character to descibe the action source
// using big character of action to descibe the action result

//TODO: if any crash scene happened, export logs and action results into i_need

namespace GameCore.Interface
{
    public class State
    {
        // u:util
        // h:hook
        // c:concept
        // d:card
        // r:rule
        // i:interface
        // l:load
        // s:save
        // Action Result
        public enum Ar
        {
        // good, bad, execption, parameter null, parameter bad, other
            G, B, E, PN, PB, O
        //
            ,u = 20
        //
            ,h = 30
        //
            ,c = 40
        // not have concept,
            ,d = 50 ,dNHC
        //
            ,r = 60
        //
            ,i = 70
        // save dir not exist, save dir illegal
            ,l = 80 ,lSDNE,lSDI
        //
            ,s = 90
        }
        private LinkedList<Ar> ars = new LinkedList<Ar>();
        public Ar AR
        {
            get
            {
                return ars.First.Value;
            }
            set
            {
                if (ars.Count > 29)
                    ars.RemoveLast();
                ars.AddFirst(value);
            }
        }
        private StringBuilder log = new StringBuilder(10000);
        // if log's length bigger than 20000, clear it at next calling
        public StringBuilder Log { get { if (log.Length > 20000) { log.Clear(); log.AppendLine("log clear."); }return log; } private set { log = value; } }
        public string LogPop { get { var str = log.ToString(); log.Clear(); return str; } }
        public bool AppendLogLine(string str)
        {
            if (str == null)
                return true;
            Log.AppendLine(str);
            return false;
        }
        public bool AppendLog(string str, bool is_append_line = false)
        {
            if (str == null)
                return true;
            if (is_append_line)
                Log.AppendLine(str);
            else
                Log.Append(str);
            return false;
        }
        public bool WriteException(Exception e, bool result = true) { AppendLogLine(e.ToString()); return result; }
        public T WriteException<T>(Exception e, T result = default){ AppendLogLine(e.ToString()); return result; }
        public object N(Ar action_result = Ar.B, string log = null)
        {
            // set action result, and return null
            Core.State.AR = action_result;
            if (log != null)
                Log.AppendLine(log);
            return null;
        }
        public bool T(Ar action_result = Ar.B, string log = null) 
        {
            // set action result, and return true
            Core.State.AR = action_result;
            if (log != null)
                Log.AppendLine(log);
            return true;
        }
        public bool F(Ar action_result = Ar.G, string log = null)
        {
            // set action result, and return false;
            Core.State.AR = action_result;
            if (log != null)
                Log.AppendLine(log);
            return false;
        }
    }
}
