using System;
using System.Collections.Generic;
using GameCore.Base;

namespace GameCore.Map
{
    public class MapRule : Rule
    {
        public TileRule TileRule {get; private set;}
        public TerrainRule TerrainRule {get; private set;}
        public override bool Init()
        {
            TileRule = new TileRule();
            TerrainRule = new TerrainRule();
            if(Util.HasAnyNull(
                TileRule, TerrainRule))
                return true;
            if(Util.HasAnyTrue(
                TileRule.Init()
                ,TerrainRule.Init()))
                return true;
            return false;
        }
        public override bool IsUsable()
        {
            if(Util.HasAnyNull(
                TileRule, TerrainRule))
                return false;
            if(Util.HasAnyFalse(
                TileRule.IsUsable()
                ,TerrainRule.IsUsable()
            ))
                return false;
            return true;
        }
    }
}