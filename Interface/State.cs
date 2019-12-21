using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore.Interface
{
    public class State
    {
        // ActionResult means the result after doing something
        // Function can change AR, to descript the state of its processing
        public enum ActionResult
        {
            Good, ProcessBad, ParameterNull, ParameterIllegal, NotHasComponent
        }
        public ActionResult AR { get; internal set; } = ActionResult.Good;

    }
}
