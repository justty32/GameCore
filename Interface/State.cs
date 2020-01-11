using System.Data;
using System;
using System.Collections.Generic;
using System.Text;

// using a character to descibe the action source
// using big character of action to descibe the action result

namespace GameCore.Interface
{
    public class State
    {
        // u:util
        // h:hook
        // c:component
        // d:card
        // r:rule
        // i:interface
        // l:load
        // s:save
        // Action Result
        public enum Ar
        {
        // good, bad, execption, parameter null, parameter bad, custom(other)
            G, B, E, PN, PB, C
        //
            ,u
        //
            ,h
        //
            ,c
        // not have component,
            ,dNHC
        //
            ,r
        //
            ,i
        // save dir not exist, save dir illegal
            ,lSDNE,lSDI
        //
            ,s
        }
        public Ar AR { get; set; } = Ar.G;
        public object N(Ar action_result)
        {
            // set action result, and return null
            Core.State.AR = action_result;
            return null;
        }
        public bool T(Ar action_result) 
        {
            // set action result, and return true
            Core.State.AR = action_result;
            return true;
        }
        public bool F(Ar action_result = Ar.G)
        {
            // set action result, and return false;
            Core.State.AR = action_result;
            return false;
        }
    }
}
