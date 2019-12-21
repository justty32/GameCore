using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore.Interface
{
    public class Rules
    {
        public Root.TimeRule TimeRule { get; private set; }
        public Root.LocationRule LocationRule { get; private set; }
        public Map.LandRule LandRule { get; private set; }
        public Map.TileRule TileRule { get; private set; }
        public Map.WorldRule WorldRule { get; private set; }
        public Map.PlanetRule PlanetRule { get; private set; }
        public Map.TerrainRule TerrainRule { get; private set; }
        public Map.LandformRule LandformRule { get; private set; }
        public Rules()
        {
            // make instances
            TimeRule = new Root.TimeRule();
            LocationRule = new Root.LocationRule();
            LandRule = new Map.LandRule();
            TileRule = new Map.TileRule();
            WorldRule = new Map.WorldRule();
            PlanetRule = new Map.PlanetRule();
            TerrainRule = new Map.TerrainRule();
            LandformRule = new Map.LandformRule();
        }
        public void Init( // many parameters
            Root.TimeRule.Time now_time
        )
        {
            // do Init
            TimeRule.Init();
            TimeRule.SetNowTime(now_time);
            LocationRule.Init();
            LandRule.Init();
            TileRule.Init();
            WorldRule.Init();
            PlanetRule.Init();
            LandformRule.Init();
            TerrainRule.Init();
        }
    }
}
