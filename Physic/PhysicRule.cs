using System;
using System.Collections.Generic;

// TODO : fill more attribute into CPhysical

namespace GameCore.Physic
{
    public class PhysicRule : Base.Rule
    {
        public class CPhysical : Base.Concept
        {
            private string _type_name = "CPhysical";
            public override string TypeName => _type_name;
        }
        public class RigidBody
        { }
        public class Collider
        { }

    }
}