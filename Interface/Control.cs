using System;
using GameCore.Base;

// so the thinking of CreateNew, is prepare a complete a complete save dir in common
// which store the needed cards and ruledata of game start.
// copy it all into savedir, and rename it. and Load() it to be a new world.

namespace GameCore.Interface
{
    public class Control
    {
        public bool Load(string save_name)
        {
            // check save dir exist
            if(!Core.Instance.INeed.IsSaveExist(save_name) || !Core.Instance.INeed.IsSaveDirLegal(save_name))
                return true;
            // set save name
            Core.Instance.Save_Name = save_name;
            // load info of world
            var world_info = Core.Instance.Load.WorldInfo();
            // init data
            if(Core.Instance.DataInit(world_info))
                return true;
            // load rule data
            if(Core.Instance.Load.Rules())
                return true;
            return false;
        }
        public bool Save(string save_name = null)
        {
            // must specific the save dir name
            if(save_name == null)
                return true;
            // change target
            Core.Instance.Save_Name = save_name;
            // if not cover, create new dir
            if(!Core.Instance.INeed.IsSaveExist(save_name)){
                if(Core.Instance.INeed.NewSaveDir(save_name))
                    return true;
                if(Core.Instance.INeed.IsSaveDirLegal(save_name))
                    return true;
            }
            // order: info, ruledata, cards
            if(Core.Instance.Save.WorldInfo())
                return true;
            if(Core.Instance.Save.Rules())
                return true;
            if(Core.Instance.Save.CardAll())
                return true;
            return false;
        }
        public bool CreateNew(WorldInfo world_info)
        {
            if(Core.Instance.DataInit(world_info))
                return true;
            // call many rule's function about newing world
            return false;
        }
        public bool Exit()
        {
            Core.Instance.DataRemove();
            return false;
        }
    }
}