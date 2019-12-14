using System;
using System.Collections.Generic;

// TODO : seperate the animal's and intelligence's intelligence, make the hash, and make the rule of action decision

namespace GameCore.AI
{
    public class IntelligenceRule : Base.Rule
    {
        public class CIntelligence : Base.Component
        {
            private string _type_name = "CIntelligence";
            public override string TypeName => _type_name;
        }
        public class CCogitation : Base.Component
        {
            private string _type_name = "CCogitation";
            public override string TypeName => _type_name;
        }
    }
}