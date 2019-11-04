using System;

namespace GameCore
{
    class Program
    {
        public class Tee : Base.Component
        {
            public override string TypeName => "Tee";
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var e = Base.Component.GetSpawner<Tee>().Spawn();
            Base.Component.GetSpawner(e.TypeNumber);
            Base.Component.GetTypeName(1);
            Base.ComponentSet componentSet = new Base.ComponentSet();
        }
    }
}
